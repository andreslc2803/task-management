# TaskManagement BD

## Pasos para ejecutar el proyecto por BD

### Prerrequisitos 
Instalar el motor de bd SQL Server y SQL Server Management Studio 

1. Crear Bd y tablas: Ejecutar los script que estan en la carpeta ScriptsBd -> CreateDbAndTables (es su respectivo orden numerico)

2. Alimentar las tablas: Ejecutar los script que estan en la carpeta ScriptsBd -> InsertData (es su respectivo orden numerico)

### Nota: En la carpeta ScriptsBd -> QueriesJSON estan las consultas utilizando los diferentes metodos JSON de Sql server (ISJSON, JSON_VALUE, JSON_QUERY, OPENJSON )

# TaskManagement API

## Pasos para ejecutar el proyecto

1. Clona el repositorio y sitúate en la raíz del proyecto:

   ```bash
   git clone https://github.com/andreslc2803/task-management.git
   ```
Ubicate en la rama:
   ```branch 
   master
   ```

### Nota: Tener presente las dependencias en las capas de Api y DAL

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

# Task Management Frontend

Una aplicación frontend de gestión de tareas construida con Angular 20, diseñada como una Single Page Application (SPA) para consumir una API REST de backend.

## Instalación

### Prerrequisitos
- Node.js 18+
- Angular CLI 20+
- Backend API corriendo (por defecto en `https://localhost:7177/api`)

### Instalación
```bash
npm install
```

### Configuración
Edita `src/environments/environment.ts` para desarrollo:
```typescript
export const environment = {
  production: false,
  apiUrl: 'https://localhost:7177/api'  // Cambia según el puerto dle backend
};
```

Para producción, edita `environment.prod.ts`.

## Uso

### Desarrollo
```bash
ng serve
```
Navega a `http://localhost:4200/`.

## Características

- **Interfaz de usuario moderna**: Utiliza Angular Material para una experiencia de usuario consistente y atractiva.
- **Gestión de usuarios y tareas**: Lista, crea y administra usuarios y sus tareas asignadas.
- **Estados de tareas**: Cambia el estado de las tareas (Pending, In Progress, Completed).
- **Filtros y paginación**: Filtra tareas por estado y pagina la lista de usuarios.
- **Manejo de errores**: Modales informativos para errores de API.
- **Arquitectura reactiva**: Usa RxJS y BehaviorSubjects para estado reactivo.
- **Configurable**: URLs de API configurables por entorno.

## Arquitectura y Tecnologías

### Tecnologías Principales
- **Angular 20**: Framework principal con componentes standalone.
- **Angular Material**: Biblioteca de componentes UI.
- **RxJS**: Programación reactiva para manejo de estado y llamadas HTTP.
- **TypeScript**: Tipado fuerte para mejor mantenibilidad.

### Estructura del Proyecto
```
src/
├── app/
│   ├── components/          # Componentes de la UI
│   │   ├── dashboard.component.ts     # Componente principal SPA
│   │   ├── user-list.component.ts     # Lista de usuarios con paginación
│   │   ├── task-list.component.ts     # Lista de tareas con cambio de estado
│   │   ├── task-filter.component.ts   # Filtro de tareas
│   │   ├── create-user-dialog.component.ts  # Modal creación usuario
│   │   ├── create-task-dialog.component.ts  # Modal creación tarea
│   │   └── error-dialog.component.ts        # Modal de errores
│   ├── services/            # Servicios para llamadas API
│   │   ├── user.service.ts
│   │   ├── task.service.ts
│   │   └── error.service.ts
│   ├── models/              # Interfaces TypeScript
│   │   ├── user.model.ts
│   │   └── task.model.ts
│   ├── interceptors/        # Interceptores HTTP
│   │   └── error.interceptor.ts
│   └── config/              # Configuración de la app
├── environments/            # Configuración por entorno
│   ├── environment.ts       # Desarrollo
│   └── environment.prod.ts  # Producción
```

### Por qué esta estructura
- **Standalone Components**: Facilita la modularidad y reduce el boilerplate de módulos.
- **Servicios inyectables**: Centraliza la lógica de negocio y llamadas API.
- **Interceptores**: Manejo global de errores HTTP.
- **Modelos tipados**: Mejora la robustez y autocompletado.
- **Environments**: Configuración flexible para diferentes entornos.

## Funcionalidades

### Dashboard Principal
- Panel dividido: Usuarios a la izquierda, tareas del usuario seleccionado a la derecha.
- Selección automática del primer usuario al cargar.

### Gestión de Usuarios
- Lista paginada (5, 10, 20 por página).
- Creación vía modal con validación.
- Visualización de tareas asignadas.

### Gestión de Tareas
- Lista filtrable por estado.
- Creación con selección de usuario, estado y prioridad.
- Cambio de estado con botones directos.
- Prioridad como JSON (ej: `{"priority": "high"}`).

### Manejo de Errores
- Modales informativos para errores de API.
- Logging en consola para debugging.
- Interceptor global como respaldo.

## API Endpoints Esperados

La aplicación consume los siguientes endpoints REST:

- `GET /User` - Lista de usuarios
- `POST /User` - Crear usuario
- `GET /Task?status={status}` - Lista de tareas (opcional filtro)
- `POST /Task` - Crear tarea
- `PUT /Task/{id}/status` - Actualizar estado de tarea

### Formatos de Datos
- **Usuario**: `{ id: number, name: string, email: string, createAt: string, tasks: Task[] }`
- **Tarea**: `{ id: number, title: string, status: string, taskPriority: string }`
- **Crear Usuario**: `{ name: string, email: string }`
- **Crear Tarea**: `{ title: string, description?: string, status: string, userId: number, taskPriority?: string }`

## Licencia

Este proyecto es para fines de prueba técnica. No tiene licencia específica.

## Autor

Andrés Londoño Carvajal

**Nota**: Asegúrate de que el backend esté corriendo y accesible antes de usar la aplicación.
