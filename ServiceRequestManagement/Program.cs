using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using ServiceRequestManagement.BAL;
using ServiceRequestManagement.BAL.Interface;
using ServiceRequestManagement.DAL;
using ServiceRequestManagement.DAL.Interface;
using ServiceRequestManagement.Middleware;
using ServiceRequestManagement.Models;
using System.Diagnostics;
using Serilog;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddLogging(logging =>
{
    logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
    logging.AddConsole();
    logging.AddDebug();
}).AddSerilog();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowEndpointOrigin", builder =>
    {
        builder.AllowAnyOrigin()//.WithOrigins("https://specific-origin.com")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddMemoryCache();
builder.Services.AddSingleton<IDapperContext, DapperContext>();
builder.Services.AddSingleton<IBALogManagement, BALogManagement>();
builder.Services.AddScoped<IBAServiceRequest, BAServiceRequest>();

builder.Services.AddControllers(config => config.Filters.Add(new BadRequestResultFilter()))
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ServiceRequestManagement", Version = "v1" });
});

var app = builder.Build();


if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ServiceRequestManagement");
        c.RoutePrefix = "swagger";
        c.OAuthClientId("swagger-ui");
        c.OAuthAppName("Swagger UI");
        c.OAuthUsePkce();
    });
}
app.UseSerilogRequestLogging();
app.UseMiddleware<ExceptionMiddleware>();
//app.UseMiddleware<ConfigurationMiddleware>();

app.UseRouting();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.UseCors("AllowEndpointOrigin");

await app.RunAsync();
