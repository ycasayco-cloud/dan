# RuralMed: demostración con datos ficticios

**Datos iniciales:** 30 pacientes, 8 médicos, 48 citas, 24 historiales = **110 registros**. Todos son inventados.

Al iniciar, `DemoSeeder.SeedIfEmpty(store)` carga registros solo si las cuatro colecciones están vacías. En Render, el sistema de archivos del contenedor es efímero y puede reiniciarse; por eso el sembrado se repetirá en una instancia nueva. Si ya tienes datos guardados, no se agregan automáticamente. Para empezar desde cero en una nueva instancia o limpiar el archivo `data/ruralmed.json` antes del despliegue. No usar datos reales.

## Endpoints

- GET `/health`
- GET `/api/pacientes` (30)
- GET `/api/medicos` (8)
- GET `/api/citas` (48)
- GET `/api/historiales` (24)
- GET `/api/{modulo}/{id}`
- POST `/api/{modulo}`
- PUT `/api/{modulo}/{id}`
- DELETE `/api/{modulo}/{id}`

Importa `RuralMed_Pruebas_Datos_Demo.postman_collection.json` en Postman, cambia `baseUrl` por la URL de Render, y ejecuta la colección en orden. El CRUD de prueba crea y elimina un paciente independiente.

## Integración con frontend

Consulta `/api/pacientes`, `/api/medicos`, `/api/citas`, `/api/historiales`; usa `pacienteId` y `medicoId` para relacionar citas e historiales. Las fechas están en UTC. El backend habilita CORS para cualquier origen **solo en demostración**.

## Evidencias para evaluación

Capturas: Render Live y `/health`, las cuatro listas, POST 201, GET 200, PUT 200, DELETE 204, GET 404, validación de cita con paciente inexistente 400, pantalla del frontend consumiendo la API, colección Postman ejecutada. Adjuntar diagrama de arquitectura, justificación PaaS, medidas de seguridad previstas, escalabilidad y costos.

**Advertencia:** sin autenticación, cifrado de registros a nivel de aplicación, ni almacenamiento durable. No es apto para información sanitaria real. Esta demostración no implementa todavía mensajería ni videoconsultas.
