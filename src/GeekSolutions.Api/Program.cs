using Microsoft.AspNetCore.Builder;
using GeekSolutions.Infrastructure;
using GeekSolutions.Application;
using FluentValidation;

//var builder = WebApplication.CreateBuilder(new WebApplicationOptions
//{
//    Args = args,
//    EnvironmentName = Environments.Development,
    
//});

//builder.Environment.EnvironmentName = Environments.Development;
//builder.WebHost.UseUrls("http://localhost:5055");

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowStaticWebsite", policy =>
    {
        policy.WithOrigins(
                "http://localhost:5500",
                "https://geeksolutions.com",
                "https://www.geeksolutions.com"
              )
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


// Registrar controladores y servicios
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Registrar la capa de infraestructura
builder.Services.AddSwaggerGen();
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "GeekSolutions API V1");
        c.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();

app.UseCors("AllowStaticWebsite");

app.UseAuthorization();
app.MapControllers();

app.Run();