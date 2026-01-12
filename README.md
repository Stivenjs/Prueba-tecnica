# Sistema de Gestión de Asegurados - SEGUROS ABC

Prueba técnica para Atlantic QI.

## Sobre el Proyecto

Este es un sistema full-stack que desarrollé para gestionar información de potenciales asegurados. El backend está hecho con .NET Core y el frontend con Angular. La idea es tener una aplicación completa donde se pueda registrar, editar, buscar y eliminar información de asegurados.

## Stack Tecnológico

**Backend:**
- .NET Core 10.0 con C#
- Entity Framework Core para el ORM
- PostgreSQL como base de datos

**Frontend:**
- Angular 21 con TypeScript
- Material Design para los componentes
- CSS para estilos personalizados

## Estructura del Proyecto

```
Prueba-Tecnica/
├── backend/
│   └── SegurosAPI/          # API .NET Core
│       ├── Controllers/     # Controladores de la API
│       ├── Models/          # Modelos de datos
│       ├── Data/            # DbContext y configuraciones
│       └── Migrations/      # Migraciones de base de datos
├── frontend/
│   └── seguros-app/         # Aplicación Angular
│       ├── src/
│       │   ├── app/         # Componentes y servicios
│       │   └── assets/      # Recursos estáticos
└── README.md
```

## Cómo ejecutar el proyecto

### Lo que necesitas tener instalado

- .NET SDK 8.0+
- Node.js 18.0+
- PostgreSQL 14+
- Git

### Configurar el Backend

Primero, entra a la carpeta del backend:
```bash
cd backend/SegurosAPI
```

Restaura las dependencias:
```bash
dotnet restore
```

Antes de correr la API, necesitas configurar la conexión a la base de datos. Más abajo explico cómo hacerlo con User Secrets (es la forma más segura para desarrollo).

Corre las migraciones para crear las tablas:
```bash
dotnet ef database update
```

Levanta la API:
```bash
dotnet run
```

Si todo salió bien, la API debería estar corriendo en `http://localhost:5231`

### Configurar el Frontend

En otra terminal, ve a la carpeta del frontend:
```bash
cd frontend/seguros-app
```

Instala las dependencias:
```bash
npm install
```

Y corre la aplicación:
```bash
ng serve
```

El frontend va a estar disponible en `http://localhost:4200`

## Configuración de la Base de Datos

Por temas de seguridad, no subí las credenciales de la base de datos al repositorio. Acá te explico cómo configurarlas en tu máquina local.

### La forma recomendada: User Secrets

Esta es la mejor manera de guardar credenciales en desarrollo porque los datos quedan fuera del proyecto y nunca se suben a Git.

```bash
cd backend/SegurosAPI
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5432;Database=InsuredsDB;Username=postgres;Password=tu_password"
```

Para verificar que quedó bien configurado:
```bash
dotnet user-secrets list
```

### Otras formas de configurarlo

Si prefieres usar variables de entorno del sistema:

En **Windows (PowerShell)**:
```powershell
$env:ConnectionStrings__DefaultConnection="Host=localhost;Port=5432;Database=InsuredsDB;Username=postgres;Password=tu_password"
dotnet run
```

En **Linux/Mac**:
```bash
export ConnectionStrings__DefaultConnection="Host=localhost;Port=5432;Database=InsuredsDB;Username=postgres;Password=tu_password"
dotnet run
```

Nota: en variables de entorno se usa `__` (doble guion bajo) en vez de `:` (dos puntos).

También puedes editar directamente el archivo `appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=InsuredsDB;Username=postgres;Password=tu_password"
  }
}
```

### Para Producción

Si vas a deployar esto en producción, nunca pongas las credenciales directamente en los archivos. Cada plataforma tiene su forma de manejar variables de entorno:

**Azure**: Configuration > Application settings > Agregar `ConnectionStrings__DefaultConnection`

**AWS Elastic Beanstalk**: Configuration > Software > Environment properties

**Docker**: Usar variables de entorno en el docker-compose o en el comando run

**Kubernetes**: Crear un Secret con la connection string

### Cómo funciona la configuración en .NET

.NET carga la configuración en este orden (el último sobrescribe al anterior):
1. `appsettings.json`
2. `appsettings.{Environment}.json`
3. User Secrets (solo Development)
4. Variables de entorno
5. Argumentos de línea de comandos

Por eso los User Secrets y las variables de entorno son seguros: sobrescriben cualquier cosa que esté en los archivos.

## API Endpoints

La API tiene estos endpoints para gestionar asegurados:

- `GET /api/insureds` - Lista todos los asegurados con paginación (pageNumber y pageSize como query params)
- `GET /api/insureds/{id}` - Trae un asegurado específico por su número de identificación
- `POST /api/insureds` - Crea un nuevo asegurado
- `PUT /api/insureds/{id}` - Actualiza un asegurado existente
- `DELETE /api/insureds/{id}` - Elimina un asegurado
- `GET /api/insureds/search/{identificationNumber}` - Busca asegurados (permite búsqueda parcial del número)

## Modelo de Datos

Un asegurado tiene estos campos:
- Número de identificación (obligatorio, es la primary key)
- Primer nombre (obligatorio)
- Segundo nombre (opcional)
- Primer apellido (obligatorio)
- Segundo apellido (obligatorio)
- Teléfono de contacto (obligatorio)
- Email (obligatorio y debe ser único)
- Fecha de nacimiento (obligatorio, debe ser mayor de 18 años)
- Valor estimado del seguro (obligatorio, debe ser mayor a 0)
- Observaciones (opcional)

## Lo que incluye el proyecto

### En el Backend:
- Arquitectura en capas separando Controllers, Services y Repositories
- CRUD completo con todas las operaciones
- Validaciones tanto de datos como de reglas de negocio
- Middleware para manejar errores de forma global
- Códigos HTTP correctos para cada tipo de respuesta (200, 201, 400, 404, 409, 500)
- Paginación en el listado
- Búsqueda que funciona con números parciales
- Migraciones de Entity Framework para versionar la BD
- Validaciones de email único y edad mínima
- DTOs para separar la lógica de la base de datos
- Logs de las operaciones importantes

### En el Frontend:
- Componentes standalone de Angular (sin módulos)
- Formularios reactivos con validaciones en tiempo real
- Tabla con Material Design
- Paginación y búsqueda que se actualiza mientras escribes
- Indicadores de carga
- Mensajes de error en español
- Confirmaciones antes de eliminar
- Validaciones de formulario del lado del cliente
- Diseño responsive
- Formato de moneda colombiana y fechas

## Decisiones de Arquitectura

Implementé el backend con una arquitectura en capas:
- **Repositories** para abstraer todo el acceso a datos
- **Services** donde va toda la lógica de negocio
- **Controllers** que solo manejan las peticiones HTTP
- **DTOs** para no exponer las entidades de la BD directamente
- **Excepciones personalizadas** (NotFoundException, ConflictException, ValidationException) para manejar cada tipo de error
- **Middleware global** que captura todas las excepciones y devuelve respuestas consistentes

En el frontend usé:
- **Componentes standalone** (sin NgModules)
- **Signals** para manejar el estado de forma reactiva
- **Services** para centralizar la comunicación con la API
- **Material Design** para tener una UI consistente

## Validaciones y Seguridad

Implementé validaciones tanto en el cliente como en el servidor:
- El número de identificación debe ser único
- El email debe ser único y tener formato válido
- Los nombres solo pueden tener letras
- El teléfono debe tener formato válido
- La edad mínima es 18 años
- El valor del seguro debe ser mayor a 0
- Todos los campos tienen límites de longitud

Para la seguridad:
- Las credenciales nunca se suben al repo
- Entity Framework previene inyección SQL
- Los errores no exponen información sensible del sistema
- CORS configurado para desarrollo
- Logs de todas las operaciones importantes

## Posibles Mejoras

Si tuviera más tiempo, agregaría:
- Autenticación y autorización con JWT
- Tests unitarios e de integración
- Caché para las consultas más frecuentes
- Rate limiting para prevenir abuse
- Audit trail más completo
- Soft delete en vez de borrado físico
- Exportar datos a Excel o PDF

---

Desarrollado como prueba técnica para Atlantic QI.