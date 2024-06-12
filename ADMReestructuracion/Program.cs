using ADMReestructuracion.Auth.BusinessLogic.Configuration;
using ADMReestructuracion.Auth.BusinessLogic.Mapping;
using ADMReestructuracion.Auth.DataAccess.Configuration;
using ADMReestructuracion.Auth.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Configurar DbContext
builder.Services.AddDbContext<AuthContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Configurar servicios personalizados
builder.Services.ConfigureAppService();
builder.Services.ConfigureMappingProfile();
builder.Services.ConfigureDataService();

// Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

// Configuración de Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ADMReestructuracion", Version = "v1" });
});

var app = builder.Build();

// Configurar middleware
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    // Configurar Swagger en desarrollo
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ADMReestructuracion V1");
        //c.RoutePrefix = string.Empty; // Para acceder a Swagger UI en la raíz (opcional)
    });
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();