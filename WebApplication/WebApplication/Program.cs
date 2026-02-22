global using Microsoft.AspNetCore.Builder;using Repository.Entities;
using Repository.Interfaces;
using Repository.Repositories;
using Service.Dto;
using Service.Interface;
using Service.Services;
using Service;

var builder = Microsoft.AspNetCore.Builder.WebApplication.CreateBuilder(args);

// Repositories
builder.Services.AddScoped<IRepository<User>, UserRepository>();
builder.Services.AddScoped<IRepository<Project>, ProjectRepository>();
builder.Services.AddScoped<IRepository<TaskItem>, TaskItemRepository>();
builder.Services.AddScoped<IRepository<SubTask>, SubTaskRepository>();
builder.Services.AddScoped<IRepository<History>, HistoryRepository>();
// Add services to the container.

builder.Services.AddScoped<IService<UserDto>, UserService>();
builder.Services.AddScoped<IService<ProjectDto>, ProjectService>();
builder.Services.AddScoped<IService<TaskItemDto>, TaskService>();
builder.Services.AddScoped<IService<SubTaskDto>, SubTaskService>();
builder.Services.AddScoped<IService<HistoryDto>, HistoryService>();
builder.Services.AddControllers();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MyMapper>());
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
