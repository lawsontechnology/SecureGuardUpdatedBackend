using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using MailKit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Visitor_Management_System.Core.Application.Authentication;
using Visitor_Management_System.Core.Application.Implementation.Services;
using Visitor_Management_System.Core.Application.Interface.Repositories;
using Visitor_Management_System.Core.Application.Interface.Services;
using Visitor_Management_System.Infrastructure;
using Visitor_Management_System.Infrastructure.Context;
using Visitor_Management_System.Infrastructure.Email;
using Visitor_Management_System.Infrastructure.Excel;
using Visitor_Management_System.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});


builder.Services.AddDbContext<VisitorManagementSystem>(opt => opt.UseMySQL(builder.Configuration.GetConnectionString("VMSconnection")));
builder.Services.AddSingleton<IJWTAuthenticationManager, JWTAuthenticationManager>();
builder.Services.AddScoped<IAddressRepo, AddressRepositories>();
builder.Services.AddScoped<IAuditLogRepo, AuditLogRepositories>();
builder.Services.AddScoped<IProfileRepo, ProfilesRepositories>();
builder.Services.AddScoped<IRoleRepo, RoleRepositories>();
builder.Services.AddScoped<IUserRepo, UserRepositories>();
builder.Services.AddScoped<IVisitorRepo, VisitorRepositories>();
builder.Services.AddScoped<IVisitRepo, VisitRepositories>();
builder.Services.AddScoped<IAuditLogService, AuditLogService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IVisitorService, VisitorService>();
builder.Services.AddScoped<IExcelService, ExcelService>();
builder.Services.AddScoped<IMailServices, EmailSender>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IToken, TokenRepositories>();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "SecureGuard", Version = "v1" });
});

builder.Services.AddScoped<IUserService, UserService>();

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
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["JWTSettings:SecretKey"])),

            ValidateIssuer = false,
            ValidateAudience = false
        };

    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//
app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();
