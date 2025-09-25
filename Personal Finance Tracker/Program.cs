using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Personal_Finance_Tracker.Data;
using Personal_Finance_Tracker.Jobs;
using PersonalFinanceTracker.Core.Interfaces;
using PersonalFinanceTracker.Core.Services;
using PersonalFinanceTracker.Infrastructure.Repositories;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IBudgetService, BudgetService>();
builder.Services.AddScoped<IBudgetRepository, BudgetRepository>();
builder.Services.AddScoped<IReportService, ReportService>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("connection"), b => b.MigrationsAssembly("PersonalFinanceTracker.Infrastructure")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowUI",
        policy => policy.WithOrigins("http://localhost:5002") // Replace with UI URL
                        .AllowAnyHeader()
                        .AllowAnyMethod());
});

builder.Services.AddHostedService<BudgetMonthlyJob>();

var app = builder.Build();
app.UseCors("AllowUI");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
