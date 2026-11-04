# RuralMed API - Render

Este repositorio contiene solo un proyecto: `RuralMed.Api.csproj` (.NET 8).

## Despliegue
1. Sube el contenido de esta carpeta a la raíz de tu repositorio de GitHub.
2. En Render crea un **Web Service** con runtime **Docker**. Si el repositorio contiene otras carpetas, indica la ruta correcta del Dockerfile y el contexto de construcción.
3. Configura estas variables de entorno en Render:
   - `FIREBASE_PROJECT_ID`: identificador de proyecto Firebase.
   - `FIREBASE_SERVICE_ACCOUNT_JSON`: contenido JSON completo de la cuenta de servicio (secreto, nunca en GitHub).
   - `FRONTEND_ORIGINS`: orígenes autorizados, separados por comas, por ejemplo `https://tu-proyecto.web.app,https://tu-proyecto.firebaseapp.com`.
4. Despliega y comprueba `https://TU-SERVICIO.onrender.com/health`.

## Nota
El arranque requiere credenciales Firebase válidas y Firestore habilitado. La compilación se corrige, pero el despliegue real debe probarse en Render. No uses información médica real sin completar una revisión de seguridad y privacidad.
