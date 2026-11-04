# RuralMed API (.NET 8)

## Ejecutar
Instala .NET 8 y configura las variables de entorno `FIREBASE_PROJECT_ID`, `FIREBASE_SERVICE_ACCOUNT_JSON` (JSON completo de una cuenta de servicio) y `FRONTEND_ORIGINS` (orígenes separados por coma, sin barra final). Ejecuta `dotnet run`.

## Render
Sube **solo la carpeta backend** a un repositorio de GitHub, crea un Web Service Docker en Render y configura las tres variables como secretos en el panel. Usa un plan sin suspensión para producción. Render debe tener HTTPS. Prueba `GET /health`.

## Roles y seguridad
Los usuarios nuevos son pacientes. El rol `doctor` **solo** debe ser asignado por un administrador confiable desde una herramienta interna, nunca desde el frontend. Las consultas médicas y registros requieren asignaciones de citas confirmadas; este MVP **no incluye** todavía el flujo administrativo de asignación/confirmación, videollamadas ni mensajería. No usar con datos clínicos reales sin auditoría, controles de consentimiento, retención, backups y revisión legal.

## Endpoints
`GET /health`, `POST /api/profile`, `GET /api/me`, `POST /api/appointments`, `GET /api/appointments`, `POST /api/records` (médico asignado), `GET /api/records` (paciente). Todas las rutas `/api` requieren Firebase ID token en `Authorization: Bearer <token>`.
