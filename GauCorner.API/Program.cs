using GauCorner.API.Middleware;
using GauCorner.Business.MapperProfiles;
using GauCorner.Business.Services.DonateServices;
using GauCorner.Business.Services.ProductServices;
using GauCorner.Business.Services.UserServices;
using GauCorner.Business.Services.UserTokenServices;
using GauCorner.Data.Entities;
using GauCorner.Data.Repositories.AttributeValueRepositories;
using GauCorner.Data.Repositories.CategoryRepositories;
using GauCorner.Data.Repositories.DonateRepositories;
using GauCorner.Data.Repositories.ProductAttachmentRepositories;
using GauCorner.Data.Repositories.ProductAttributeRepositories;
using GauCorner.Data.Repositories.ProductRepositories;
using GauCorner.Data.Repositories.ProductVariantRepositories;
using GauCorner.Data.Repositories.StreamConfigRepositories;
using GauCorner.Data.Repositories.StreamConfigTypeRepositories;
using GauCorner.Data.Repositories.UIConfigRepositories;
using GauCorner.Data.Repositories.UserRepositories;
using GauCorner.Data.Repositories.UserTokenRepositories;
using GauCorner.Data.Repositories.VariantAttributeValueRepo;
using GauCorner.Services.CategoryServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

DotNetEnv.Env.Load();
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var rawConnectionString = builder.Configuration.GetSection("Database:ConnectionString").Value;

if (rawConnectionString == null)
{
    throw new Exception("Connection string is not found");
}

var connectionString = rawConnectionString
    .Replace("${DB_SERVER}", Environment.GetEnvironmentVariable("DB_SERVER") ?? "")
    .Replace("${DB_USER}", Environment.GetEnvironmentVariable("DB_USER") ?? "")
    .Replace("${DB_PASSWORD}", Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "")
    .Replace("${DB_NAME}", Environment.GetEnvironmentVariable("DB_NAME") ?? "")
    .Replace("${DB_PORT}", Environment.GetEnvironmentVariable("DB_PORT") ?? "")
    .Replace("${HERE_MAP_API_KEY}", Environment.GetEnvironmentVariable("HERE_MAP_API_KEY") ?? "");

builder.Services.AddDbContext<GauCornerContext>(options =>
    options.UseSqlServer(connectionString));

//========================================== SWAGGER ==============================================
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "GauCorner.API",
        Description = "GauCorner"
    });

    // 🟢 Cấu hình Bearer Token
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. " +
                      "\n\nEnter your token in the text input below. " +
                      "\n\nExample: '12345abcde'",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });

    // 🟢 Cấu hình Cookie Authentication
    c.AddSecurityDefinition("cookieAuth", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.ApiKey,
        Name = "Cookie",
        In = ParameterLocation.Header,
        Description = "Nhập Cookie vào đây (VD: sessionId=xyz123)"
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
            new string[] {}
        },
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "cookieAuth"
                }
            },
            new string[] {}
        }
    });
});

//======================================= AUTHENTICATION ==========================================
builder.Services.AddAuthentication("GauCornerAuthentication")
    .AddScheme<AuthenticationSchemeOptions, AuthorizeMiddleware>("GauCornerAuthentication", null);

//========================================== MIDDLEWARE ===========================================
builder.Services.AddSingleton<GlobalExceptionMiddleware>();

//========================================== MAPPER ===============================================
builder.Services.AddAutoMapper(typeof(MapperProfileConfiguration).Assembly);

//========================================== REPOSITORY ===========================================
builder.Services.AddScoped<IUserRepositories, UserRepositories>();
builder.Services.AddScoped<IUserTokenRepositories, UserTokenRepositories>();
builder.Services.AddScoped<IStreamConfigTypeRepositories, StreamConfigTypeRepositories>();
builder.Services.AddScoped<IStreamConfigRepositories, StreamConfigRepositories>();
builder.Services.AddScoped<IDonateRepositories, DonateRepositories>();
builder.Services.AddScoped<IUIConfigRepositories, UIConfigRepositories>();
builder.Services.AddScoped<ICategoryRepositories, CategoryRepositories>();
builder.Services.AddScoped<IProductRepositories, ProductRepositories>();
builder.Services.AddScoped<IProductAttributeRepositories, ProductAttributeRepositories>();
builder.Services.AddScoped<IAttributeValueRepositories, AttributeValueRepositories>();
builder.Services.AddScoped<IProductVariantRepositories, ProductVariantRepositories>();
builder.Services.AddScoped<IVariantAttributeValueRepo, VariantAttributeValueRepo>();
builder.Services.AddScoped<IProductAttachmentRepositories, ProductAttachmentRepositories>();

//=========================================== SERVICE =============================================
builder.Services.AddScoped<IUserServices, UserServices>();
builder.Services.AddScoped<IDonateServices, DonateServices>();
builder.Services.AddScoped<IProductServices, ProductServices>();
builder.Services.AddScoped<ICategoryServices, CategoryServices>();
builder.Services.AddScoped<IUserTokenServices, UserTokenServices>();

//=========================================== CORS ================================================
var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>();
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowAllOrigin", policy =>
    {
        policy
            .WithOrigins(allowedOrigins!)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .WithExposedHeaders("New-Access-Token");
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowAllOrigin");

app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseAuthentication();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
