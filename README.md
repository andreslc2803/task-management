# TaskManagement API

## Pasos para ejecutar el proyecto

1. Clona el repositorio y sitúate en la raíz del proyecto:

   ```bash
   git clone <repo-url>
   cd <repo-root>
   ```

2. Configura la cadena de conexión para desarrollo en `TaskManagement.Api/appsettings.Development.json` o mediante la variable de entorno `ConnectionStrings__DefaultConnection`.

3. (Opcional) Si hay migraciones pendientes, aplica las migraciones desde la raíz:

   ```bash
   dotnet ef database update --project TaskManagement.DAL --startup-project TaskManagement.Api
   ```

4. Restaurar dependencias y ejecutar la API:

   ```bash
   dotnet restore
   cd TaskManagement.Api
   dotnet run
   ```

5. Abre Swagger para probar los endpoints (ej.: `https://localhost:{puerto}/swagger`).

## Decisiones técnicas

- Plataforma: .NET 8 y C# 12.
- Arquitectura: solución en capas con proyectos `Api`, `BL`, `DAL`, `Entities`.
- Persistencia: EF Core con SQL Server y repositorios (`TaskRepository`, `UserRepository`).
- Validación: `DataAnnotations` en DTOs y validaciones adicionales en servicios/constructores donde procede.
- Manejo de errores: middleware centralizado que devuelve `ProblemDetails` y mapea excepciones de negocio y de persistencia a códigos HTTP adecuados.
- Documentación: uso de atributos `ProducesResponseType` en controladores y Swagger disponible en Development.
