using CarService.CarService;
using CarService.CustomerService;
using CarService.Data;
using CarService.DTOs.CarDto;
using CarService.DTOs.CustomerDto;
using CarService.DTOs.EmployeeDto;
using CarService.DTOs.RepairDto;
using CarService.EmployeeService;
using CarService.Services;
using CarService.Services.EmployeeService;
using CarService.Utilities.Validators.Car;
using CarService.Utilities.Validators.Customer;
using CarService.Utilities.Validators.Employee;
using CarService.Utilities.Validators.Repair;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Sieve.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IValidator<CreateCustomerDto>, CustomerCreateValidator>();
builder.Services.AddScoped<IValidator<UpdateCustomerDto>, CustomerUpdateValidator>();
builder.Services.AddScoped<IValidator<CreateEmployeeDto>, EmployeeCreateValidator>();
builder.Services.AddScoped<IValidator<UpdateEmployeeDto>, EmployeeUpdateValidator>();

builder.Services.AddScoped<IValidator<CreateRepairDto>, RepairCreateValidator>();
builder.Services.AddScoped<IValidator<UpdateRepairDto>, RepairUpdateValidator>();

builder.Services.AddScoped<IValidator<CreateCarDto>, CarCreateValidator>();
builder.Services.AddScoped<IValidator<UpdateCarDto>, CarUpdateValidator>();

builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<ICarService, CarService.Services.CarService>();
builder.Services.AddScoped<IRepairService, RepairService>();
builder.Services.AddScoped<SieveProcessor>();


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.UseHttpsRedirection();

app.Run();