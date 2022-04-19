using System;
using ConsulExample;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

var env = builder.Environment.EnvironmentName;
var consulHost = builder.Configuration["CONSUL_HOST"];

builder.Configuration.AddConsul(env, opt =>
{
    opt.Address = consulHost;
    opt.AutoLoad = true;
    opt.WaitTime = TimeSpan.FromSeconds(1);
});

builder.Services.AddConsulClient(consulHost);
builder.Services.AddConsulService();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();