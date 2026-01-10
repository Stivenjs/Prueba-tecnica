# 🏥 Sistema de Gestión de Asegurados - SEGUROS ABC

Prueba técnica para Atlantic QI - Sistema de registro y gestión de información de potenciales asegurados.

## 📋 Descripción

Sistema full-stack que permite capturar, gestionar y consultar información de potenciales asegurados para SEGUROS ABC. Desarrollado con .NET Core para el backend y Angular para el frontend.

## 🛠️ Tecnologías Utilizadas

### Backend
- **.NET Core 10.0** - Framework para la API RESTful
- **Entity Framework Core** - ORM para manejo de base de datos
- **PostgreSQL** - Base de datos relacional
- **C#** - Lenguaje de programación

### Frontend
- **Angular 21** - Framework para SPA
- **TypeScript** - Lenguaje de programación
- **CSS** - Estilos

## 📁 Estructura del Proyecto

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

## 🚀 Instalación y Configuración

### Prerrequisitos
- .NET SDK 8.0 o superior
- Node.js 18.0 o superior
- PostgreSQL 14 o superior
- Git

### Backend

1. Navegar a la carpeta del backend:
```bash
cd backend/SegurosAPI
```

2. Restaurar dependencias:
```bash
dotnet restore
```

3. Configurar la cadena de conexión en `appsettings.json`

4. Ejecutar migraciones:
```bash
dotnet ef database update
```

5. Ejecutar la API:
```bash
dotnet run
```

La API estará disponible en `https://localhost:5001`

### Frontend

1. Navegar a la carpeta del frontend:
```bash
cd frontend/seguros-app
```

2. Instalar dependencias:
```bash
npm install
```

3. Ejecutar la aplicación:
```bash
ng serve
```

La aplicación estará disponible en `http://localhost:4200`

## 📡 Endpoints de la API

### Asegurados

- `GET /api/asegurados` - Obtener lista de asegurados (con paginación)
- `GET /api/asegurados/{id}` - Obtener un asegurado por ID
- `POST /api/asegurados` - Crear un nuevo asegurado
- `PUT /api/asegurados/{id}` - Actualizar un asegurado existente
- `DELETE /api/asegurados/{id}` - Eliminar un asegurado
- `GET /api/asegurados/buscar/{numeroIdentificacion}` - Buscar por número de identificación

## 📊 Modelo de Datos

### Asegurado
- **Número de identificación** (long, PK, requerido)
- **Primer nombre** (string, requerido)
- **Segundo nombre** (string, opcional)
- **Primer apellido** (string, requerido)
- **Segundo apellido** (string, requerido)
- **Teléfono de contacto** (string, requerido)
- **E-mail** (string, requerido)
- **Fecha de nacimiento** (DateTime, requerido)
- **Valor estimado de solicitud del seguro** (decimal, requerido)
- **Observaciones** (string, opcional)

## ✅ Funcionalidades Implementadas

### Backend
- [x] Modelo de datos completo
- [x] CRUD completo de asegurados
- [x] Validaciones en servidor
- [x] Paginación de resultados
- [x] Filtro por número de identificación
- [x] Entity Framework Core con migraciones
- [x] Arquitectura limpia y escalable

### Frontend
- [x] Formulario de registro de asegurados
- [x] Tabla de gestión con edición y eliminación
- [x] Búsqueda por número de identificación
- [x] Validaciones en cliente
- [x] Interfaz moderna y responsiva
- [x] Manejo de errores

## 🔄 Control de Versiones (GitFlow)

Este proyecto sigue el estándar GitFlow:

- `main` - Rama principal de producción
- `develop` - Rama de desarrollo
- `feature/*` - Ramas para nuevas funcionalidades
- `hotfix/*` - Ramas para correcciones urgentes

## 👨‍💻 Autor

Desarrollado como prueba técnica para Atlantic QI

## 📝 Notas

Este proyecto fue desarrollado siguiendo las mejores prácticas de desarrollo y arquitectura de software, con énfasis en código limpio, mantenible y escalable.
# Prueba-tecnica
