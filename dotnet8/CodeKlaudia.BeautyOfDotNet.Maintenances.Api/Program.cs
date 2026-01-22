using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Stratio.Challenges.VehicleMaintenances.Api.Services;
using Stratio.Challenges.VehicleMaintenances.Database;
using Stratio.Challenges.VehicleMaintenances.Database.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<MaintenancesContext>(builder => 
    builder.UseInMemoryDatabase("ExampleDb"));
builder.Services.AddScoped<IMaintenanceService, MaintenanceService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
