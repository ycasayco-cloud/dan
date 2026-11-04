using System;
public static class DemoSeeder
{
    public static void SeedIfEmpty(DemoStore store)
    {
        if (store.All<Patient>("pacientes").Count != 0 || store.All<Doctor>("medicos").Count != 0 || store.All<Appointment>("citas").Count != 0 || store.All<MedicalRecord>("historiales").Count != 0) return;
        // Identificadores estables para poder relacionar citas e historiales.
        var patients = new Patient[] {
            new() { Id = Guid.Parse("b1dab340-070e-52be-93e4-ecd122420280"), Nombre = "Ana Quispe", Documento = "70000001", Telefono = "980000001", Comunidad = "Acobamba" },
            new() { Id = Guid.Parse("3c6eeb58-0146-5d66-b238-b5165da91f67"), Nombre = "Luis Huaman", Documento = "70000002", Telefono = "980000002", Comunidad = "Palcamayo" },
            new() { Id = Guid.Parse("710bb281-04d1-5547-aae5-c4af6e66294f"), Nombre = "Rosa Condori", Documento = "70000003", Telefono = "980000003", Comunidad = "Huasahuasi" },
            new() { Id = Guid.Parse("4736d70c-40e1-532b-81ef-f99b6549b348"), Nombre = "Jose Flores", Documento = "70000004", Telefono = "980000004", Comunidad = "Tarma" },
            new() { Id = Guid.Parse("4d14bdc4-4f3b-5c5f-a8ac-36f74c7fcec7"), Nombre = "Maria Rojas", Documento = "70000005", Telefono = "980000005", Comunidad = "Palca" },
            new() { Id = Guid.Parse("9a855806-b5b8-5027-8e0f-aeaca4cbab18"), Nombre = "Pedro Ccanto", Documento = "70000006", Telefono = "980000006", Comunidad = "Tapo" },
            new() { Id = Guid.Parse("b9cc24ce-f8a4-5026-97f0-762b08d1314f"), Nombre = "Elena Ramos", Documento = "70000007", Telefono = "980000007", Comunidad = "San Pedro de Cajas" },
            new() { Id = Guid.Parse("e2636d44-d0e2-5374-a822-a1119b68d96b"), Nombre = "Carlos Vega", Documento = "70000008", Telefono = "980000008", Comunidad = "Huaricolca" },
            new() { Id = Guid.Parse("7f7147b7-4aff-5d3b-8424-fbaef62f179e"), Nombre = "Lucia Torres", Documento = "70000009", Telefono = "980000009", Comunidad = "La Union Leticia" },
            new() { Id = Guid.Parse("f80db158-d757-5121-aa19-d65da92f10d5"), Nombre = "Miguel Poma", Documento = "70000010", Telefono = "980000010", Comunidad = "Tarmatambo" },
            new() { Id = Guid.Parse("12cb9e57-ad75-5636-a080-cdd13516da70"), Nombre = "Sofia Huanca", Documento = "70000011", Telefono = "980000011", Comunidad = "Acobamba" },
            new() { Id = Guid.Parse("8bb7f094-f5c2-542e-93da-d40d6b9a9d94"), Nombre = "Diego Castro", Documento = "70000012", Telefono = "980000012", Comunidad = "Palcamayo" },
            new() { Id = Guid.Parse("2cd9686f-7381-55de-b0f0-e2cefd373037"), Nombre = "Julia Salazar", Documento = "70000013", Telefono = "980000013", Comunidad = "Huasahuasi" },
            new() { Id = Guid.Parse("cb104ea1-e3d6-50a9-97b8-a81034abbe00"), Nombre = "Jorge Medina", Documento = "70000014", Telefono = "980000014", Comunidad = "Tarma" },
            new() { Id = Guid.Parse("dbbbdbe2-c6b7-5ea3-9f95-312b6d325bf4"), Nombre = "Carmen Soto", Documento = "70000015", Telefono = "980000015", Comunidad = "Palca" },
            new() { Id = Guid.Parse("84187062-2c5a-58af-addb-6081aee919e2"), Nombre = "Raul Lopez", Documento = "70000016", Telefono = "980000016", Comunidad = "Tapo" },
            new() { Id = Guid.Parse("484ccd05-4c1a-5aad-8a5f-d4ed2f96f457"), Nombre = "Teresa Mendoza", Documento = "70000017", Telefono = "980000017", Comunidad = "San Pedro de Cajas" },
            new() { Id = Guid.Parse("d90889c0-bba3-5340-bc59-40e05cc34b0f"), Nombre = "Andres Navarro", Documento = "70000018", Telefono = "980000018", Comunidad = "Huaricolca" },
            new() { Id = Guid.Parse("91119ff8-ea6b-59ce-9bd8-d0e53e4eb13f"), Nombre = "Diana Paredes", Documento = "70000019", Telefono = "980000019", Comunidad = "La Union Leticia" },
            new() { Id = Guid.Parse("f022142a-516f-59eb-9610-4b57e87b0e71"), Nombre = "Victor Arias", Documento = "70000020", Telefono = "980000020", Comunidad = "Tarmatambo" },
            new() { Id = Guid.Parse("50f19936-c43e-5e19-9913-37497eca7081"), Nombre = "Paola Gutierrez", Documento = "70000021", Telefono = "980000021", Comunidad = "Acobamba" },
            new() { Id = Guid.Parse("9cf143a2-5e26-5c9a-9498-73c4730bc983"), Nombre = "Felipe Espinoza", Documento = "70000022", Telefono = "980000022", Comunidad = "Palcamayo" },
            new() { Id = Guid.Parse("3c3ecb5a-666d-5829-928d-332f524f9d8b"), Nombre = "Nadia Chavez", Documento = "70000023", Telefono = "980000023", Comunidad = "Huasahuasi" },
            new() { Id = Guid.Parse("212d2c62-7ef8-5940-bb57-56b7c2f094a0"), Nombre = "Ricardo Campos", Documento = "70000024", Telefono = "980000024", Comunidad = "Tarma" },
            new() { Id = Guid.Parse("9bdde8e9-5b69-56fe-b4a4-e49dbe17a2c2"), Nombre = "Gabriela Leon", Documento = "70000025", Telefono = "980000025", Comunidad = "Palca" },
            new() { Id = Guid.Parse("15b52a45-d829-59ef-a44d-697f9badc46a"), Nombre = "Oscar Valdez", Documento = "70000026", Telefono = "980000026", Comunidad = "Tapo" },
            new() { Id = Guid.Parse("b5ed11ef-9ecc-56b1-b423-1cef4766c133"), Nombre = "Milagros Cabrera", Documento = "70000027", Telefono = "980000027", Comunidad = "San Pedro de Cajas" },
            new() { Id = Guid.Parse("6ef68175-455a-5eed-8f81-68ae4a8ad847"), Nombre = "Hector Rivas", Documento = "70000028", Telefono = "980000028", Comunidad = "Huaricolca" },
            new() { Id = Guid.Parse("f32f1466-0957-5cd1-b6ba-f56754748fb1"), Nombre = "Patricia Luna", Documento = "70000029", Telefono = "980000029", Comunidad = "La Union Leticia" },
            new() { Id = Guid.Parse("87add57b-b668-5b6e-8fdd-b071ef735549"), Nombre = "Bruno Aguilar", Documento = "70000030", Telefono = "980000030", Comunidad = "Tarmatambo" },
        };
        var doctors = new Doctor[] {
            new() { Id = Guid.Parse("2a3815e0-680c-53f6-b439-899b981075b6"), Nombre = "Valeria Paredes", Especialidad = "Medicina general", Colegiatura = "CMP-DEMO-1000" },
            new() { Id = Guid.Parse("96e1453b-c535-5cfe-9ec5-6ced68a333ca"), Nombre = "Daniel Salas", Especialidad = "Pediatria", Colegiatura = "CMP-DEMO-1001" },
            new() { Id = Guid.Parse("44f8d179-2ce2-5c1f-8605-832ac26b9d85"), Nombre = "Camila Rios", Especialidad = "Medicina interna", Colegiatura = "CMP-DEMO-1002" },
            new() { Id = Guid.Parse("0dac7983-6224-58a1-a386-3e395af9b94b"), Nombre = "Alonso Vega", Especialidad = "Ginecologia", Colegiatura = "CMP-DEMO-1003" },
            new() { Id = Guid.Parse("7b68b296-2efa-5d8b-96fd-66633805453d"), Nombre = "Natalia Torres", Especialidad = "Dermatologia", Colegiatura = "CMP-DEMO-1004" },
            new() { Id = Guid.Parse("17875796-27e9-52ec-a786-faad3c9eb6fc"), Nombre = "Sebastian Cruz", Especialidad = "Cardiologia", Colegiatura = "CMP-DEMO-1005" },
            new() { Id = Guid.Parse("5b405ac5-6a61-55db-8726-d7bffc24a499"), Nombre = "Mariana Flores", Especialidad = "Psicologia", Colegiatura = "CMP-DEMO-1006" },
            new() { Id = Guid.Parse("1f0721fb-9809-5bc3-a58d-0d3e6473bb93"), Nombre = "Eduardo Leon", Especialidad = "Geriatria", Colegiatura = "CMP-DEMO-1007" },
        };
        foreach (var patient in patients) store.Upsert("pacientes", patient);
        foreach (var doctor in doctors) store.Upsert("medicos", doctor);
        var states = new[] { "solicitada", "confirmada", "atendida", "cancelada" };
        var reasons = new[] { "Control preventivo", "Consulta respiratoria", "Seguimiento de tratamiento", "Dolor de cabeza", "Orientacion medica", "Control de presion arterial", "Consulta pediatrica", "Evaluacion dermatologica" };
        for (var i = 0; i < 48; i++)
        {
            var cita = new Appointment
            {
                Id = Guid.Parse("6a13bfa0-5556-5d2c-a98f-649e8987bd53"),
                PacienteId = patients[i % patients.Length].Id,
                MedicoId = doctors[i % doctors.Length].Id,
                FechaUtc = new DateTime(2026, 11, 10, 14, 0, 0, DateTimeKind.Utc).AddDays(i / 6).AddHours(i % 6),
                Motivo = reasons[i % reasons.Length],
                Estado = states[i % states.Length]
            };
            cita.Id = DemoId(i);
            store.Upsert("citas", cita);
        }
        var diagnoses = new[] { "Control general sin hallazgos relevantes", "Sintomas respiratorios leves", "Seguimiento de hipertension", "Cefalea inespecifica", "Consejeria preventiva", "Evaluacion nutricional" };
        var treatments = new[] { "Recomendaciones generales y seguimiento", "Control en siete dias", "Monitoreo domiciliario", "Evaluacion presencial si persisten sintomas", "Educacion en salud", "Control en un mes" };
        for (var i = 0; i < 24; i++)
        {
            store.Upsert("historiales", new MedicalRecord
            {
                Id = DemoHistoryId(i),
                PacienteId = patients[i % patients.Length].Id,
                MedicoId = doctors[i % doctors.Length].Id,
                Diagnostico = diagnoses[i % diagnoses.Length],
                Tratamiento = treatments[i % treatments.Length],
                FechaUtc = new DateTime(2026, 10, 1, 15, 0, 0, DateTimeKind.Utc).AddDays(i)
            });
        }
    }
    private static Guid DemoId(int i) => GuidFromIndex("cita", i);
    private static Guid DemoHistoryId(int i) => GuidFromIndex("historial", i);
    private static Guid GuidFromIndex(string kind, int index)
    {
        var bytes = System.Security.Cryptography.MD5.HashData(System.Text.Encoding.UTF8.GetBytes("RuralMed-demo-2026-" + kind + "-" + index));
        return new Guid(bytes);
    }
}
