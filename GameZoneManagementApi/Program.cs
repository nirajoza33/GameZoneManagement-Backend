using GameZoneManagementApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<GamezoneDbContext>(op => 
                                op.UseSqlServer(builder.Configuration.GetConnectionString("GamezoneConString")));

builder.WebHost.ConfigureKestrel(op =>
{
    op.ListenAnyIP(5012);
    op.ListenAnyIP(7186, lp => lp.UseHttps());
});


// Configure file upload limits - Updated for larger files
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 50 * 1024 * 1024; // 50MB (increased from 10MB)
    options.ValueLengthLimit = int.MaxValue;
    options.ValueCountLimit = int.MaxValue;
    options.KeyLengthLimit = int.MaxValue;
});


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Otp sending and Storing functionality
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(op =>
{
    op.IOTimeout = TimeSpan.FromMinutes(5);
    //op.IdleTimeout = TimeSpan.FromMinutes(5);
    op.Cookie.HttpOnly = true;
    op.Cookie.IsEssential = true;
    op.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Important for security
    op.Cookie.SameSite = SameSiteMode.None;
    //op.Cookie.SameSite = SameSiteMode.Lax; // Change from None to Lax for better session handling
});

var smtpEmail = builder.Configuration["Smtp:Email"];

// Retrieve JWT key and decode if Base64-encoded
var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(op =>
    {
        op.RequireHttpsMetadata = false;
        op.SaveToken = true;
        op.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key), // Ensure key is set properly
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true, // Ensure token expiration is validated
            ValidIssuer = builder.Configuration["Jwt:Issuer"], // Matches appsettings.json
            ValidAudience = builder.Configuration["Jwt:Audience"], // Matches appsettings.json
        };
    });



builder.Services.AddAuthorization();    

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.WithOrigins("http://localhost:3000")
              .AllowCredentials()
              .AllowAnyHeader()
              .AllowAnyMethod());
});

var app = builder.Build();

app.UseCors("AllowAll");

app.UseSession();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


// Add static files support
app.UseStaticFiles();


app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();


app.Run();

