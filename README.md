# ObjectLab
### Simulador de programación orientada a objetos

ObjectLab es una aplicación web que permite modelar clases e interfaces,
definir sus miembros y simular secuencias de llamadas a métodos.

Su objetivo es explorar cómo se relacionan los elementos de un modelo
orientado a objetos y visualizar las invocaciones que se producen durante
una ejecución simulada.

Desarrollado por **Florencia Mendoza, Natalia Franco y Julieta Lopez** como proyecto académico de Diseño de Aplicaciones II
en la Universidad ORT Uruguay.

## Funcionalidades

- Gestión de clases, atributos, métodos, parámetros y variables locales.
- Modelado de herencia, clases abstractas y selladas, y modificadores
  de accesibilidad.
- Definición de interfaces y sus firmas de métodos.
- Organización de tipos mediante namespaces.
- Configuración de miembros estáticos y métodos virtuales y override.
- Simulación de llamadas a métodos con una salida jerárquica.
- Extensión de los formatos de salida mediante plugins cargados
  dinámicamente por reflection, incluyendo un transformador HTML.
- Protección del endpoint de simulación mediante API Key.
- Persistencia de los modelos en SQL Server.

## Tecnologías

- Frontend: Angular 19, TypeScript y Bootstrap.
- Backend: C# y ASP.NET Core sobre .NET 8.
- Persistencia: Entity Framework Core y SQL Server.
- Pruebas: MSTest y Moq.
- Automatización: workflows de GitHub Actions para compilación,
  pruebas y análisis de código.

## Arquitectura

La solución separa las responsabilidades en proyectos de dominio,
servicios, acceso a datos y API REST. El frontend Angular consume
los endpoints del backend.

El mecanismo de plugins permite incorporar transformadores de salida
desde ensamblados externos sin recompilar la aplicación principal.

## Contexto académico

El proyecto se desarrolló en dos etapas: una primera centrada en
el backend y la API REST, y una segunda que incorporó la interfaz web
y amplió el modelo y sus mecanismos de extensibilidad.