using Microsoft.EntityFrameworkCore;
using TodoApp.Core.Interfaces;
using TodoApp.Repositories;
using TodoApp.Repositories.Repositories;
using TodoApp.Repositories.UnitOfWorks;
using TodoApp.Services;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TodoApp.API.Middleware;
using Microsoft.Extensions.Configuration;
using TodoApp.Core.Settings;
using TodoApp.API.Helpers;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });
builder.Services.AddEndpointsApiExplorer();

// Swagger konfigürasyonunu güncelle
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "TodoApp.API", Version = "v1" });
    
    // JWT kimlik doğrulaması için Swagger UI konfigürasyonu
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
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

// DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositories
builder.Services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
builder.Services.AddScoped<TodoRepository>();
builder.Services.AddScoped<PlanRepository>();
builder.Services.AddScoped<NotificationRepository>();
builder.Services.AddScoped<TodoCollaboratorRepository>();
builder.Services.AddScoped<PlanCollaboratorRepository>();
builder.Services.AddScoped<CategoryRepository>();
builder.Services.AddScoped<UserRepository>();

// Services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITodoService, TodoService>();
builder.Services.AddScoped<IPlanService, PlanService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<ICollaborationService, CollaborationService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

// UnitOfWork
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// JWT ayarlarını yapılandırmadan al
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
builder.Services.Configure<JwtSettings>(jwtSettings);

// JwtHelper'ı servis olarak kaydet
builder.Services.AddScoped<JwtHelper>();

// Authentication ayarlarını güncelle
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = jwtSettings.GetValue<bool>("ValidateIssuer"),
        ValidateAudience = jwtSettings.GetValue<bool>("ValidateAudience"),
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.ASCII.GetBytes(jwtSettings.GetValue<string>("SecretKey")))
    };
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "TodoApp.API v1");
    });
}

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseHttpsRedirection();

// Authentication ve Authorization middleware'lerinin sırası önemli
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();