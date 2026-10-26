using AccesoADatos;
using AccesoADatos.InterfacesRepositorios;
using AccesoADatos.Repositorios;
using API.Filtros;
using Microsoft.EntityFrameworkCore;
using Reflection;
using Reflection.InterfacesReflection;
using Servicios;
using Servicios.InterfacesServicios;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ExcepcionesFiltro>();
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var configuration = builder.Configuration;
var services = builder.Services;

var connectionString = configuration.GetConnectionString("AppDb");
if (string.IsNullOrEmpty(connectionString))
{
    throw new Exception("Missing connection string");
}

services.AddDbContext<DbContext, AppDbContext>(options => options.UseSqlServer(connectionString));

services.AddScoped<ITipoDatoRepositorio, TipoDatoRepositorio>();
services.AddScoped<ITipoDatoServicio, TipoDatoServicio>();
services.AddScoped<IAtributoRepositorio, AtributoRepositorio>();
services.AddScoped<IAtributoServicio, AtributoServicio>();
services.AddScoped<IVariableLocalRepositorio, VariableLocalRepositorio>();
services.AddScoped<IVariableLocalServicio, VariableLocalServicio>();
services.AddScoped<IParametroRepositorio, ParametroRepositorio>();
services.AddScoped<IParametroServicio, ParametroServicio>();
services.AddScoped<IMetodoRepositorio, MetodoRepositorio>();
services.AddScoped<IMetodoServicio, MetodoServicio>();
services.AddScoped<IClaseRepositorio, ClaseRepositorio>();
services.AddScoped<IClaseServicio, ClaseServicio>();
services.AddScoped<IInterfazRepositorio, InterfazRepositorio>();
services.AddScoped<IInterfazServicio, InterfazServicio>();
services.AddScoped<ISimulacionServicio, SimulacionServicio>();
services.AddScoped<ICargadorDePlugins<ITransformador>, CargadorDePlugins<ITransformador>>();
services.AddScoped<IApiKeyServicio, ApiKeyServicio>();
services.AddScoped<IApiKeyRepositorio, ApiKeyRepositorio>();

services.AddCors(options =>
{
    options.AddPolicy("PermitirAngular",
        policy => policy.WithOrigins("http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("PermitirAngular");

app.UseAuthorization();

app.MapControllers();

app.Run();
