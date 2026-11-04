# RuralMed API REST — demo académica

API de telemedicina .NET 8 sin Firebase, base de datos ni credenciales. Los datos se guardan en un archivo JSON dentro del contenedor; **no usar información médica real**. En Render Free los datos pueden perderse al reiniciar/re-desplegar y no se garantizan persistencia ni seguridad. No existe autenticación ni autorización: uso exclusivamente demostrativo.

## Render
Subir **solo el contenido de esta carpeta** a la raíz del repositorio GitHub conectado a Render. El Dockerfile se detecta automáticamente. No configurar variables Firebase. Ejecutar `Clear build cache & deploy` y abrir `https://TU-URL.onrender.com/health`.

## Local
`dotnet run --urls http://localhost:10000` y consultar `http://localhost:10000/health`.

## Endpoints
- `GET /`, `GET /health`, `GET /api`
- `GET /api/pacientes`, `GET /api/pacientes/{id}`, `POST /api/pacientes`, `PUT /api/pacientes/{id}`, `DELETE /api/pacientes/{id}`
- Mismas operaciones para `/api/medicos`, `/api/citas`, `/api/historiales`.

### Ejemplos de JSON
Paciente: `{ "nombre":"Ana Quispe", "documento":"00000001", "telefono":"999111222", "comunidad":"Tarma" }`
Médico: `{ "nombre":"Luis Perez", "especialidad":"Medicina general", "colegiatura":"CMP-DEMO" }`
Cita: `{ "pacienteId":"GUID_PACIENTE", "medicoId":"GUID_MEDICO", "fechaUtc":"2026-11-20T15:00:00Z", "motivo":"Consulta general", "estado":"solicitada" }`
Historial: `{ "pacienteId":"GUID_PACIENTE", "medicoId":"GUID_MEDICO", "diagnostico":"Ejemplo ficticio", "tratamiento":"Ejemplo ficticio", "fechaUtc":"2026-11-20T15:00:00Z" }`

La colección Postman incluida crea paciente y médico, guarda sus IDs y prueba CRUD. Configurar variable `baseUrl` con la URL de Render. La API devuelve 201 al crear, 200 al consultar/editar, 204 al eliminar, 400 por validación, 404 si no existe y 409 si un paciente/médico tiene referencias.

## Limitaciones
No es un sistema clínico listo para producción. Requiere autenticación, permisos, cifrado, auditoría, persistencia real, backups, privacidad y pruebas de concurrencia. La validación y las restricciones de integridad son simplificadas.
