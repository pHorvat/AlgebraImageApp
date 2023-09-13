// See https://aka.ms/new-console-template for more information

using System.Text;
using AlgebraImageApp.Patterns;
using AlgebraImageApp.Repositories;
using AlgebraImageApp.Services;
using AlgebraImageApp.Tools;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using MsSqlSimpleClient.SqlClient.Direct;
using MsSqlSimpleClient.SqlClient.Procedures;
using Prometheus;
using Prometheus.Client.AspNetCore;

//var metricServer = new KestrelMetricServer(port: 9090);

PhotoBuilder serBuilder = new PhotoBuilder();
BadClass badSer = new BadClass();
var ser = Serialization.SerializeObject(serBuilder);
var ser2 = Serialization.SerializeObject(badSer);
Console.WriteLine(Serialization.DeserializeObject<PhotoBuilder>(ser));
//Console.WriteLine(Serialization.DeserializeObject<BadClass>(ser2));


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<ISqlProcedureClient>(new SqlProcedureClient("Server=algebrainstaserv.database.windows.net;Database=AlgebraInstaDb;User Id=pHorva;Password=m354Hd9DtMWf27azAFq;"));
builder.Services.AddSingleton<ISqlDirectClient>(new SqlDirectClient("Server=algebrainstaserv.database.windows.net;Database=AlgebraInstaDb;User Id=pHorva;Password=m354Hd9DtMWf27azAFq;"));

builder.Services.AddSingleton<IUserRepository, SqlUserRepository>();
builder.Services.AddSingleton<IPhotosRepository, SqlPhotosRepository>();

builder.Services.AddSingleton<IUserService,UserService>();
builder.Services.AddSingleton<IPhotoModificationService,PhotoService>();
builder.Services.AddSingleton<IPhotoRetrievalService,PhotoService>();

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
app.UseSwagger();



if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUI();
}
app.UseAuthentication();
app.UseCors();

app.MapControllers();


app.UseRouting();

app.UseAuthorization();
app.UsePrometheusServer();
//app.UseMetricServer();
//app.UseHttpMetrics();
//app.MapMetrics();
/*
app.UseEndpoints(endpoints =>
{
    // ...
    endpoints.MapControllers();
    endpoints.MapMetrics();
});*/
//metricServer.Start();
app.Run();
