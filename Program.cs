using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
var projectId = Environment.GetEnvironmentVariable("FIREBASE_PROJECT_ID") ?? throw new Exception("Configure FIREBASE_PROJECT_ID");
var credentialJson = Environment.GetEnvironmentVariable("FIREBASE_SERVICE_ACCOUNT_JSON") ?? throw new Exception("Configure FIREBASE_SERVICE_ACCOUNT_JSON");
var credential = GoogleCredential.FromJson(credentialJson);
FirebaseApp.Create(new AppOptions { Credential = credential, ProjectId = projectId });
var firestore = new FirestoreDbBuilder { ProjectId = projectId, Credential = credential }.Build();
builder.Services.AddSingleton(firestore);
builder.Services.AddCors(o => o.AddPolicy("frontend", p => p.WithOrigins((Environment.GetEnvironmentVariable("FRONTEND_ORIGINS") ?? "http://localhost:5500").Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)).AllowAnyHeader().AllowAnyMethod()));
var app = builder.Build();
app.UseHttpsRedirection();
app.UseCors("frontend");
app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

// Each authenticated request is checked with Firebase Auth; authorization roles are read only from Firestore.
async Task<(string? uid, string? role)> Identity(HttpRequest request) {
    var auth = request.Headers.Authorization.ToString();
    if (!auth.StartsWith("Bearer ")) return (null, null);
    try {
        var token = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(auth[7..], true);
        var doc = await firestore.Collection("users").Document(token.Uid).GetSnapshotAsync();
        var role = doc.Exists && doc.TryGetValue<string>("role", out var r) ? r : "patient";
        return (token.Uid, role);
    } catch { return (null, null); }
}
app.MapPost("/api/profile", async (HttpRequest request, ProfileInput input) => {
    var (uid, role) = await Identity(request);
    if (uid is null) return Results.Unauthorized();
    var doc = firestore.Collection("users").Document(uid);
    var existing = await doc.GetSnapshotAsync();
    if (existing.Exists) return Results.Conflict(new { error = "Profile already exists" });
    await doc.CreateAsync(new { displayName = input.DisplayName.Trim(), role = "patient", createdAt = Timestamp.GetCurrentTimestamp() });
    return Results.Ok(new { uid, role = "patient" });
});
app.MapGet("/api/me", async (HttpRequest request) => {
    var (uid, role) = await Identity(request);
    if (uid is null) return Results.Unauthorized();
    return Results.Ok(new { uid, role });
});
app.MapPost("/api/appointments", async (HttpRequest request, AppointmentInput input) => {
    var (uid, role) = await Identity(request);
    if (uid is null) return Results.Unauthorized();
    if (role != "patient") return Results.Forbid();
    if (input.DateUtc <= DateTime.UtcNow || input.DateUtc > DateTime.UtcNow.AddMonths(12)) return Results.BadRequest(new { error = "Invalid date" });
    if (string.IsNullOrWhiteSpace(input.Reason) || input.Reason.Length > 300) return Results.BadRequest(new { error = "Reason must be 1–300 characters" });
    var doc = await firestore.Collection("appointments").AddAsync(new Dictionary<string, object> {
      ["patientId"] = uid, ["doctorId"] = "", ["dateUtc"] = Timestamp.FromDateTime(input.DateUtc.ToUniversalTime()),
      ["reason"] = input.Reason.Trim(), ["status"] = "requested", ["createdAt"] = Timestamp.GetCurrentTimestamp()
    });
    return Results.Created($"/api/appointments/{doc.Id}", new { id = doc.Id });
});
app.MapGet("/api/appointments", async (HttpRequest request) => {
    var (uid, role) = await Identity(request);
    if (uid is null) return Results.Unauthorized();
    if (role is not ("patient" or "doctor")) return Results.Forbid();
    var field = role == "doctor" ? "doctorId" : "patientId";
    var snapshot = await firestore.Collection("appointments").WhereEqualTo(field, uid).Limit(50).GetSnapshotAsync();
    return Results.Ok(snapshot.Documents.Select(d => new { id = d.Id, patientId = d.GetValue<string>("patientId"), doctorId = d.GetValue<string>("doctorId"), reason = d.GetValue<string>("reason"), status = d.GetValue<string>("status"), dateUtc = d.GetValue<Timestamp>("dateUtc").ToDateTime() }));
});
app.MapPost("/api/records", async (HttpRequest request, RecordInput input) => {
    var (uid, role) = await Identity(request);
    if (uid is null) return Results.Unauthorized();
    if (role != "doctor") return Results.Forbid();
    if (string.IsNullOrWhiteSpace(input.PatientId) || string.IsNullOrWhiteSpace(input.Notes) || input.Notes.Length > 5000) return Results.BadRequest();
    // Only a doctor assigned to a confirmed appointment with this patient may create a record.
    var authorized = await firestore.Collection("appointments").WhereEqualTo("doctorId", uid).WhereEqualTo("patientId", input.PatientId).WhereEqualTo("status", "confirmed").Limit(1).GetSnapshotAsync();
    if (authorized.Count == 0) return Results.Forbid();
    var doc = await firestore.Collection("records").AddAsync(new Dictionary<string, object> { ["patientId"] = input.PatientId, ["doctorId"] = uid, ["notes"] = input.Notes.Trim(), ["createdAt"] = Timestamp.GetCurrentTimestamp() });
    return Results.Created($"/api/records/{doc.Id}", new { id = doc.Id });
});
app.MapGet("/api/records", async (HttpRequest request) => {
    var (uid, role) = await Identity(request);
    if (uid is null) return Results.Unauthorized();
    if (role != "patient") return Results.Forbid();
    var snapshot = await firestore.Collection("records").WhereEqualTo("patientId", uid).Limit(50).GetSnapshotAsync();
    return Results.Ok(snapshot.Documents.Select(d => new { id = d.Id, doctorId = d.GetValue<string>("doctorId"), notes = d.GetValue<string>("notes"), createdAt = d.GetValue<Timestamp>("createdAt").ToDateTime() }));
});
app.Run();
record ProfileInput(string DisplayName);
record AppointmentInput(DateTime DateUtc, string Reason);
record RecordInput(string PatientId, string Notes);
