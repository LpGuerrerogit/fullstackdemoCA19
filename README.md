# FullstackDemoCA19

Aplicación fullstack desarrollada con **.NET 8 (Web API)** y **Angular 19** como parte de un reto técnico para validar habilidades en integración con Azure, desarrollo limpio y publicación profesional.

---

## 📌 Descripción del proyecto

Esta aplicación permite:

- Capturar y almacenar datos de usuario (nombre y edad) como archivos JSON en Azure Blob Storage.
- Descargar imágenes desde Azure Blob (público o privado).
- Obtener una URL temporal mediante **token SAS** para imágenes privadas.
- Mostrar la imagen en el frontend utilizando un pipe seguro.
- Integración directa con Azure Key Vault para manejo de secretos sin hardcodeo.

---

## 🧱 Estructura del proyecto

```
FullstackDemoCA19/
├── backend/ (Proyecto ASP.NET Core Web API)
│   ├── Controllers/
│   │   └── DatosController.cs
│   ├── Services/
│   │   ├── AzureStorageService.cs
│   │   └── IAzureStorageService.cs
│   ├── Models/
│   │   └── DatoUsuario.cs
│   ├── Program.cs
│   ├── wwwroot/ ← Se copian los archivos compilados del frontend Angular
│   └── TestAzureAngular19.csproj
│
├── demofstackca19/ (Angular 19)
│   ├── src/app/
│   │   ├── app.component.ts
│   │   ├── app.component.html
│   │   ├── app.component.scss
│   ├── angular.json
│   ├── tsconfig.json
│   └── package.json
```

---

## 🔐 Seguridad e integración con Azure

- Los secretos (como la cadena de conexión de Azure Blob Storage) se guardan en **Azure Key Vault**.
- El backend utiliza `DefaultAzureCredential` para autenticar la app y recuperar secretos de forma segura.
- Los contenedores `imgpub`, `imgpriv` y `datosusuario` se utilizan para almacenar imágenes y JSON.

---

## 🚀 Instrucciones para usar o personalizar

### Requisitos
- Node.js 18+
- Angular CLI 17+
- .NET SDK 8.0
- Cuenta en Azure

### Pasos

1. Clona el repositorio:

```bash
git clone https://github.com/<tu_usuario>/FullstackDemoCA19.git
cd FullstackDemoCA19
```

2. Restaura dependencias y compila Angular:

```bash
cd demofstackca19
npm install
```

3. Desde Visual Studio o CLI, publica el backend con:

```bash
dotnet publish -c Release
```

Esto ejecutará automáticamente el build de Angular y copiará los archivos generados a `wwwroot`.

4. Despliega el contenido a tu **Azure App Service** configurado con:
- Key Vault (con el secreto `StorageConnectionString`)
- Identidad administrada habilitada (para leer secretos)
- Variable de entorno `KeyVaultUrl`

---

## 🧪 Swagger personalizado

Durante el desarrollo, se configuró Swagger solo para entornos de desarrollo. Incluye:

- Título y descripción personalizados
- Contacto del autor
- Agrupación por controlador

Disponible en:

```
https://<tu_app>.azurewebsites.net/swagger
```

Solo visible si `app.Environment.IsDevelopment()` es verdadero.

---

## ✏️ Personalización del branding

La aplicación utiliza un tema personalizado con los siguientes colores:

| Elemento     | Color     |
|--------------|-----------|
| Fondo        | `#401a44` |
| Primario     | `#8249ad` |
| Secundario   | `#6de9b9` |
| Tipografía   | `#dec9b3` |

Los estilos están centralizados en `styles.scss` y `app.component.scss`.

---

## 🧩 Extras

- El frontend se sirve como SPA desde `wwwroot`, compilado automáticamente al hacer `dotnet build` o `publish`.
- Se eliminará `dist/` al hacer `dotnet clean` gracias a una tarea MSBuild personalizada (`CleanAngular`).
- Se puede usar en CI/CD sin necesidad de pasos adicionales de build de Angular.

---

## 📞 Contacto

Luis Pablo Guerrero Garza  
[LinkedIn](https://www.linkedin.com/in/fscluisguerrero)