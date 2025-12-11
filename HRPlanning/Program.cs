using System;
using HRPlanning.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// �������� ������ ����������� �� appsettings.{Environment}.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// ��������� DbContext � ����������� Npgsql
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString)
           .EnableSensitiveDataLogging() // �������� ��� �������, ��� ��������� � ������
           .LogTo(Console.WriteLine, LogLevel.Information)
);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<HRPlanning.Repository.IEmployeeRepository, HRPlanning.Repository.EmployeeRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
