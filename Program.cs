using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.Services.ConfigureHttpJsonOptions(o => o.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddCors(o => o.AddPolicy("demo", p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));
var app = builder.Build();
app.UseCors("demo");

// Demo only: JSON storage on the container's ephemeral filesystem; no authentication.
var store = new DemoStore(Path.Combine(AppContext.BaseDirectory, "data", "ruralmed.json"));
app.MapGet("/", () => Results.Ok(new { service = "RuralMed API", version = "1.0", documentation = "/api", health = "/health", warning = "Solo demostracion, no usar datos reales" }));
app.MapGet("/health", () => Results.Ok(new { status = "ok", service = "RuralMed API" }));
app.MapGet("/api", () => Results.Ok(new { endpoints = new[] { "/api/pacientes", "/api/medicos", "/api/citas", "/api/historiales" }, methods = new[] { "GET", "POST", "PUT", "DELETE" } }));

MapCrud<Patient>(app, store, "pacientes", p => string.IsNullOrWhiteSpace(p.Nombre) || string.IsNullOrWhiteSpace(p.Documento) ? "Nombre y documento son obligatorios" : null);
MapCrud<Doctor>(app, store, "medicos", d => string.IsNullOrWhiteSpace(d.Nombre) || string.IsNullOrWhiteSpace(d.Especialidad) ? "Nombre y especialidad son obligatorios" : null);
MapCrud<Appointment>(app, store, "citas", c =>
    !store.Exists<Patient>("pacientes", c.PacienteId) ? "El paciente no existe" :
    !store.Exists<Doctor>("medicos", c.MedicoId) ? "El medico no existe" :
    c.FechaUtc == default ? "FechaUtc es obligatoria" :
    !new[] { "solicitada", "confirmada", "cancelada", "atendida" }.Contains(c.Estado.ToLowerInvariant()) ? "Estado invalido" : null);
MapCrud<MedicalRecord>(app, store, "historiales", h =>
    !store.Exists<Patient>("pacientes", h.PacienteId) ? "El paciente no existe" :
    !store.Exists<Doctor>("medicos", h.MedicoId) ? "El medico no existe" :
    string.IsNullOrWhiteSpace(h.Diagnostico) ? "Diagnostico es obligatorio" : null);
app.Run();

static void MapCrud<T>(WebApplication app, DemoStore store, string name, Func<T, string?> validate) where T : Entity
{
    var route = "/api/" + name;
    app.MapGet(route, () => Results.Ok(store.All<T>(name)));
    app.MapGet(route + "/{id:guid}", (Guid id) => store.Get<T>(name, id) is { } item ? Results.Ok(item) : Results.NotFound(new { error = "No encontrado" }));
    app.MapPost(route, (T item) =>
    {
        var error = validate(item);
        if (error is not null) return Results.BadRequest(new { error });
        item.Id = Guid.NewGuid();
        store.Upsert(name, item);
        return Results.Created($"{route}/{item.Id}", item);
    });
    app.MapPut(route + "/{id:guid}", (Guid id, T item) =>
    {
        if (!store.Exists<T>(name, id)) return Results.NotFound(new { error = "No encontrado" });
        var error = validate(item);
        if (error is not null) return Results.BadRequest(new { error });
        item.Id = id;
        store.Upsert(name, item);
        return Results.Ok(item);
    });
    app.MapDelete(route + "/{id:guid}", (Guid id) =>
    {
        if (name == "pacientes" && (store.All<Appointment>("citas").Any(x => x.PacienteId == id) || store.All<MedicalRecord>("historiales").Any(x => x.PacienteId == id)))
            return Results.Conflict(new { error = "Paciente vinculado a citas o historiales" });
        if (name == "medicos" && (store.All<Appointment>("citas").Any(x => x.MedicoId == id) || store.All<MedicalRecord>("historiales").Any(x => x.MedicoId == id)))
            return Results.Conflict(new { error = "Medico vinculado a citas o historiales" });
        return store.Delete<T>(name, id) ? Results.NoContent() : Results.NotFound(new { error = "No encontrado" });
    });
}

public abstract class Entity { public Guid Id { get; set; } }
public sealed class Patient : Entity { public string Nombre { get; set; } = ""; public string Documento { get; set; } = ""; public string Telefono { get; set; } = ""; public string Comunidad { get; set; } = ""; }
public sealed class Doctor : Entity { public string Nombre { get; set; } = ""; public string Especialidad { get; set; } = ""; public string Colegiatura { get; set; } = ""; }
public sealed class Appointment : Entity { public Guid PacienteId { get; set; } public Guid MedicoId { get; set; } public DateTime FechaUtc { get; set; } public string Motivo { get; set; } = ""; public string Estado { get; set; } = "solicitada"; }
public sealed class MedicalRecord : Entity { public Guid PacienteId { get; set; } public Guid MedicoId { get; set; } public string Diagnostico { get; set; } = ""; public string Tratamiento { get; set; } = ""; public DateTime FechaUtc { get; set; } = DateTime.UtcNow; }

public sealed class DemoStore
{
    private readonly object _gate = new();
    private readonly string _path;
    private readonly JsonSerializerOptions _json = new() { WriteIndented = true, PropertyNameCaseInsensitive = true };
    private Dictionary<string, string> _tables = new();
    public DemoStore(string path)
    {
        _path = path;
        if (File.Exists(path))
        {
            try { _tables = JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(path), _json) ?? new(); }
            catch (JsonException) { throw new InvalidOperationException("Archivo de datos JSON corrupto"); }
        }
    }
    private List<T> Load<T>(string name) where T : Entity => _tables.TryGetValue(name, out var json) ? JsonSerializer.Deserialize<List<T>>(json, _json) ?? new() : new();
    private void Save<T>(string name, List<T> list) where T : Entity
    {
        _tables[name] = JsonSerializer.Serialize(list, _json);
        Directory.CreateDirectory(Path.GetDirectoryName(_path)!);
        var temp = _path + ".tmp";
        File.WriteAllText(temp, JsonSerializer.Serialize(_tables, _json));
        File.Move(temp, _path, true);
    }
    public List<T> All<T>(string name) where T : Entity { lock (_gate) return Load<T>(name); }
    public T? Get<T>(string name, Guid id) where T : Entity { lock (_gate) return Load<T>(name).FirstOrDefault(x => x.Id == id); }
    public bool Exists<T>(string name, Guid id) where T : Entity { lock (_gate) return Load<T>(name).Any(x => x.Id == id); }
    public void Upsert<T>(string name, T item) where T : Entity
    {
        lock (_gate) { var list = Load<T>(name); list.RemoveAll(x => x.Id == item.Id); list.Add(item); Save(name, list); }
    }
    public bool Delete<T>(string name, Guid id) where T : Entity
    {
        lock (_gate) { var list = Load<T>(name); if (list.RemoveAll(x => x.Id == id) == 0) return false; Save(name, list); return true; }
    }
}
