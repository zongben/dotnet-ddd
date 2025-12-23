using System.Reflection;
using ApplicationLayer.Persistence;
using ApplicationLayer.Ports;
using InfraLayer.Entity;
using InfraLayer.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMediatR(
    (config) =>
    {
        config.RegisterServicesFromAssembly(Assembly.Load("ApplicationLayer"));
    }
);
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(opts =>
{
    opts.UseSqlite("Data Source=../db/app.db");
});

builder.Services.AddSingleton<ICryptService>(new CryptService());
builder.Services.AddScoped<IUserRepository, UserRepository>();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(builder => builder.AllowAnyOrigin());
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
