# RuralMed API — corrección NETSDK1064

El error NETSDK1064 se produce cuando `dotnet publish --no-restore` usa archivos NuGet de `obj/` generados fuera del contenedor o cuando faltan paquetes en el entorno de publicación. Esta versión excluye `bin/` y `obj/` del contexto Docker y ejecuta `dotnet publish` con restauración habilitada.

1. Reemplaza el contenido de la raíz del repositorio GitHub `dan` con **los archivos de esta carpeta**, sin incluir una carpeta contenedora adicional.
2. Elimina del repositorio `RuralMed.csproj` (si todavía está), `bin/` y `obj/`. Git no elimina archivos antiguos solo porque falten en un ZIP.
3. Comprueba que `Dockerfile`, `Program.cs` y `RuralMed.Api.csproj` estén en la raíz.
4. En Render: Environment -> define `FIREBASE_PROJECT_ID`, `FIREBASE_SERVICE_ACCOUNT_JSON` y `FRONTEND_ORIGINS` (URL exacta del frontend sin `/` final). Nunca subas la clave de servicio a GitHub.
5. Render: Manual Deploy -> Clear build cache & deploy (si está disponible), para evitar capas antiguas.
6. Comprueba `https://TU-SERVICIO.onrender.com/health` cuando el despliegue esté activo.

**Nota:** el backend inicializa Firebase al arrancar. Sin variables válidas el contenedor puede compilar correctamente, pero fallará al iniciar. La corrección del Dockerfile no sustituye la configuración de Firebase.

No se ha ejecutado Docker ni dotnet en este entorno; valida el resultado en los logs del siguiente despliegue.
