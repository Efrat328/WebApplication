global using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using DataContext;
using Repository.Entities;
using Repository.Repositories;
using Repository.Interfaces;
using Service.Dto;
using Service.Interface;
using Service.Services;
using Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;


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
builder.Services.AddDbContext<TaskManagerContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IContext, TaskManagerContext>();
builder.Services.AddScoped<HistoryService>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,        // ← בדוק שהטוקן מהשרת שלנו
            ValidateAudience = true,      // ← בדוק שהטוקן מיועד למשתמשים שלנו
            ValidateLifetime = true,      // ← בדוק שהטוקן לא פג תוקף
            ValidateIssuerSigningKey = true, // ← בדוק שהחתימה תקינה
            ValidIssuer = builder.Configuration["Jwt:Issuer"],      // ← קורא מה-appsettings
            ValidAudience = builder.Configuration["Jwt:Audience"],  // ← קורא מה-appsettings
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])) // ← המפתח הסודי
        };
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MyMapper>());
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowFrontend");
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
