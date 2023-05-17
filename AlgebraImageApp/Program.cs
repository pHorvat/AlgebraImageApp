// See https://aka.ms/new-console-template for more information

using System.Text;
using AlgebraImageApp.Repositories;
using AlgebraImageApp.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using MsSqlSimpleClient.SqlClient.Procedures;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<ISqlProcedureClient>(new SqlProcedureClient("Server=algebrainstaserv.database.windows.net;Database=AlgebraInstaDb;User Id=pHorva;Password=m354Hd9DtMWf27azAFq;"));

builder.Services.AddSingleton<IUserRepository, SqlUserRepository>();

builder.Services.AddSingleton<IUserService,UserService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(
JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                "ZcK#K5KdDtq8Bx%%ByhKg9BhUrtw^M6aXrnUYwQEPWn9")),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    } );

builder.Services.AddCors(cors =>
{
    cors.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthentication();
app.UseSwagger();
app.UseSwaggerUI();
app.UseCors();

app.UseAuthorization();
app.MapControllers();
app.Run();