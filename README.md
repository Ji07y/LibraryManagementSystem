# Proyecto Gestion Sistema de Libreria

Este repositorio contiene un sistema de gestión para una biblioteca llamado GestionSistemaLiBRERIA, desarrollado en ASP.NET Core. El proyecto se divide en dos aplicaciones principales:

## BookSearchService

BookSearchService es un servicio que permite buscar libros mediante una API RESTful.

### Configuración

- **URL Base del Servicio:** `http://localhost:5180`
- **Endpoints Disponibles:**
  - `/weatherforecast/`: Devuelve el pronóstico del tiempo en formato JSON.

### Tecnologías Utilizadas

- ASP.NET Core 8.0
- Entity Framework Core para la gestión de datos
- Docker para contenerización

### Instrucciones de Ejecución

1. Clona el repositorio.
2. Abre el proyecto en tu IDE preferido.
3. Ejecuta el servicio utilizando el comando `dotnet run` desde la raíz del proyecto.
4. Accede a los endpoints disponibles para probar la funcionalidad.

## LibraryManagementSystem

LibraryManagementSystem es la aplicación principal del sistema de gestión de la biblioteca.

### Características Principales

- Gestión de libros: Permite crear, listar y buscar libros en la biblioteca.
- Integración con BookSearchService: Utiliza BookSearchService para buscar libros externamente.
- Base de datos SQL Server: Utiliza una base de datos local para almacenar la información de los libros.

### Tecnologías Utilizadas

- ASP.NET Core 8.0
- Entity Framework Core para la gestión de datos
- SQL Server para la persistencia de datos

### Instrucciones de Ejecución

1. Clona el repositorio.
2. Abre el proyecto en tu IDE preferido.
3. Configura la cadena de conexión en `appsettings.json` para apuntar a tu instancia de SQL Server.
4. Ejecuta el proyecto usando `dotnet run` desde la raíz del proyecto.
5. Accede a `http://localhost:5133` para utilizar la aplicación.

### Configuración de Base de Datos

- Asegúrate de tener SQL Server instalado y configurado.
- Actualiza la cadena de conexión en `appsettings.json` bajo `ConnectionStrings`.


