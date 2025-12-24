using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMediatR(
    (config) =>
    {
        config.RegisterServicesFromAssembly(Assembly.Load("ApplicationLayer"));
    }
);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(opts =>
{
    opts.UseSqlite("Data Source=../db/app.db");
});

builder
    .Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
            ),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromMinutes(1),
        };
        options.Events = new JwtBearerEvents
        {
            OnChallenge = async context =>
            {
                context.HandleResponse();
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(ErrResponse.UnAuthorized());
                return;
            },
            OnAuthenticationFailed = async context =>
            {
                if (context.Exception is SecurityTokenExpiredException)
                {
                    context.Response.StatusCode = 401;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsJsonAsync(ErrResponse.TokenExpired());
                    return;
                }
            },
        };
        options.Events.OnForbidden = async context =>
        {
            context.Response.StatusCode = 403;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(ErrResponse.Forbidden());
            return;
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddSingleton<ICryptService>(new CryptService());
builder.Services.AddScoped<IUserRepository, UserRepository>();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(builder => builder.AllowAnyOrigin());
app.UseMiddleware<ExceptionMiddleware>();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseMiddleware<NotFoundMiddleware>();
app.Run();
