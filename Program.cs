var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Fullstack Challenge API – Datos + Storage",
        Version = "v1",
        Description = "API REST creada como parte de un reto técnico para subir y consultar datos e imágenes en Azure Blob Storage.",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Luis Pablo Guerrero",
            Email = "lpguerrero08@hotmail.com",
            Url = new Uri("https://www.linkedin.com/in/fscluisguerrero")
        },
        License = new Microsoft.OpenApi.Models.OpenApiLicense
        {
            Name = "MIT License",
            Url = new Uri("https://opensource.org/licenses/MIT")
        }
    });

    options.TagActionsBy(api =>
    {
        var groupName = api.ActionDescriptor.RouteValues["controller"];
        return new[] { groupName };
    });

    options.DocInclusionPredicate((name, api) => true);
});

builder.Services.AddScoped<IAzureStorageService, AzureStorageService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.DocumentTitle = "Documentación de la API – Fullstack Challenge";
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Fullstack Challenge API v1");
        options.RoutePrefix = "swagger"; // URL final: /swagger
    });
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthorization();

app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
