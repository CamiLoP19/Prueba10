using ApiTuEvento_.Helpers;
using ApiTuEvento_.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
var DefaultConnection = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ApiTuEventoDB;Integrated Security=True";
builder.Services.AddDbContext<ContextDB>(options => options.UseSqlServer(DefaultConnection));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
// Definimos el documento Swagger con OpenAPI 3.0.0
c.SwaggerDoc("v2", new OpenApiInfo
{
    Title = "API_Dinamita",
    Version = "v2",
    Description = "API de ejemplo con autenticación JWT"
});

    // Configurar JWT para Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Por favor ingrese el token JWT con el esquema Bearer",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});


builder.Services.AddScoped<JwtHelper>();

// Configurar JWT
var key = builder.Configuration["JwtSettings:Key"];
if (string.IsNullOrEmpty(key))
{
    throw new InvalidOperationException("JWT Key is missing from configuration.");
}
var keyBytes = Encoding.UTF8.GetBytes(key);
//Linea para permitir el uso de JWT
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]))
        };
    });

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v2/swagger.json", "API_Dinamita v2");
        c.RoutePrefix = "swagger";
    });
}

app.UseStaticFiles(); // <-- primero archivos estáticos
app.UseHttpsRedirection();

app.UseCors(permitir => permitir
    .AllowAnyOrigin() // o usa .SetIsOriginAllowed(...) si prefieres restringir
    .AllowAnyHeader()
    .AllowAnyMethod());

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = 500;
        context.Response.ContentType = "text/plain";
        var error = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>();
        if (error != null)
        {
            await context.Response.WriteAsync(error.Error.ToString());
        }
    });
});

app.Run();
