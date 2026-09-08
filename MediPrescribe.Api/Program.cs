using MediPrescribe.Application;
using MediPrescribe.Application.Interfaces.Services;
using MediPrescribe.Domain;
using MediPrescribe.Domain.Enums;
using MediPrescribe.Infrastructure;
using MediPrescribe.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "MediPrescribe API", Version = "v1" });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "أدخل رمز الـ JWT في حقل القيمة: Bearer {token}"
    });

    options.AddSecurityRequirement(document =>
        new OpenApiSecurityRequirement
        {
            { new OpenApiSecuritySchemeReference("Bearer", document), new List<string>() }
        });
});

// Composition Root: Wire the Clean Architecture layers
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// Add JWT Authentication
var issuer = builder.Configuration["JwtSettings:Issuer"] ?? "MediPrescribe.Api";
var audience = builder.Configuration["JwtSettings:Audience"] ?? "MediPrescribe.Api";
var keyStr = builder.Configuration["JwtSettings:SigningKey"]
    ?? "this_is_a_very_long_secret_key_for_development_purposes";
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyStr));

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = key,
        ValidateIssuer = true,
        ValidIssuer = issuer,
        ValidateAudience = true,
        ValidAudience = audience,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        NameClaimType = System.Security.Claims.ClaimTypes.Name
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Apply pending migrations and seed a default admin user (Development only).
    await EnsureDatabaseReadyAsync(app.Services);
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

static async Task EnsureDatabaseReadyAsync(IServiceProvider services)
{
    using var scope = services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();
    var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();

    await dbContext.Database.MigrateAsync();

    // قراءة بيانات الـ Admin من Configuration (appsettings.json) مع إمكانية
    // التجاوز عبر متغيرات البيئة مثل Admin__Email و Admin__Password.
    var adminEmail = config["Admin:Email"] ?? "admin@mediprescribe.com";
    var adminPassword = config["Admin:Password"] ?? "Admin@12345";
    var adminFullName = config["Admin:FullName"] ?? "System Administrator";
    var adminPhone = config["Admin:Phone"];

    var admin = await dbContext.Users.FirstOrDefaultAsync(u => u.Role == UserRole.Admin);

    if (admin == null)
    {
        dbContext.Users.Add(new User
        {
            Username = "admin",
            FullName = adminFullName,
            Email = adminEmail,
            Phone = adminPhone,
            PasswordHash = passwordHasher.Hash(adminPassword),
            Role = UserRole.Admin,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        });
        await dbContext.SaveChangesAsync();
    }
    else
    {
        // Admin موجود: تحديث البيانات وكلمة المرور إن لزم دون إنشاء حساب جديد.
        admin.Email = adminEmail;
        admin.FullName = adminFullName;
        admin.Phone = adminPhone;
        admin.IsActive = true;
        admin.UpdatedAt = DateTime.UtcNow;

        if (!passwordHasher.Verify(adminPassword, admin.PasswordHash))
        {
            admin.PasswordHash = passwordHasher.Hash(adminPassword);
        }

        await dbContext.SaveChangesAsync();
    }
}
