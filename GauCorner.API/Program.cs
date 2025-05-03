using GauCorner.API.Middleware;
using GauCorner.Business.MapperProfiles;
using GauCorner.Business.Services.UserServices;
using GauCorner.Data.Entities;
using GauCorner.Data.Repositories.StreamConfigRepositories;
using GauCorner.Data.Repositories.StreamConfigTypeRepositories;
using GauCorner.Data.Repositories.UserRepositories;
using GauCorner.Data.Repositories.UserTokenRepositories;
using Microsoft.EntityFrameworkCore;

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

//========================================== MIDDLEWARE ===========================================
builder.Services.AddSingleton<GlobalExceptionMiddleware>();

//========================================== MAPPER ===============================================
builder.Services.AddAutoMapper(typeof(MapperProfileConfiguration).Assembly);

//========================================== REPOSITORY ===========================================
builder.Services.AddScoped<IUserRepositories, UserRepositories>();
builder.Services.AddScoped<IUserTokenRepositories, UserTokenRepositories>();
builder.Services.AddScoped<IStreamConfigTypeRepositories, StreamConfigTypeRepositories>();
builder.Services.AddScoped<IStreamConfigRepositories, StreamConfigRepositories>();

//=========================================== SERVICE =============================================
builder.Services.AddScoped<IUserServices, UserServices>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
