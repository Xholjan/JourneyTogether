using Application.Common;
using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Application.Interfaces;
using Application.Journeys.Commands;
using Application.Notifications;
using FluentValidation;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Persistence.Repositories.Persistence.Repositories;
using Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Persistence;
using Persistence.Repositories;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Swagger
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "JourneyTogether API",
        Version = "v1"
    });
});

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowNuxt", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// SignalR
builder.Services.AddSignalR();
builder.Services.AddSingleton<IUserIdProvider, NameIdentifierUserIdProvider>();

// MediatR
builder.Services.AddMediatR(cfg =>
{
    var assembly = Assembly.Load("Application");
    cfg.RegisterServicesFromAssembly(assembly);
});

// Auth0 Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.Authority = "https://dev-0lb0kkcpz1t58aql.us.auth0.com/";
    options.Audience = "https://api.journeytogether.com";

    options.TokenValidationParameters = new TokenValidationParameters
    {
        NameClaimType = "sub",
        RoleClaimType = "https://journeytogether.com/roles"
    };

    options.MapInboundClaims = true;

    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];
            var path = context.HttpContext.Request.Path;

            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/notifications"))
            {
                context.Token = accessToken;
            }

            return Task.CompletedTask;
        },
        OnAuthenticationFailed = context =>
        {
            Console.WriteLine("Auth failed: " + context.Exception.Message);
            return Task.CompletedTask;
        }
    };
});

// Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IJourneyRepository, JourneyRepository>();
builder.Services.AddScoped<IShareRepository, ShareRepository>();
builder.Services.AddScoped<IFavouriteRepository, FavouriteRepository>();
builder.Services.AddScoped<IMonthlyDistanceRepository, MonthlyDistanceRepository>();
builder.Services.AddSingleton<IOnlineUserService, OnlineUserService>();
builder.Services.AddScoped<IEmailService, EmailService>();

// FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<CreateJourneyCommandValidator>();
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

// Authorization
builder.Services.AddAuthorization();

var app = builder.Build();
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var exceptionHandlerPathFeature =
            context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerPathFeature>();

        var exception = exceptionHandlerPathFeature?.Error;

        // CustomException handling
        if (exception is CustomException customException)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;

            var errors = new Dictionary<string, string[]>
            {
                { "Error", new[] { customException.Message } }
            };

            await context.Response.WriteAsJsonAsync(new { errors });
            return;
        }

        if (exception is GoneException goneException)
        {
            context.Response.StatusCode = StatusCodes.Status410Gone;
            var errors = new Dictionary<string, string[]>
            {
                { "Error", new[] { goneException.Message } }
            };

            await context.Response.WriteAsJsonAsync(new { errors });
            return;
        }

        // FluentValidation
        if (exception is ValidationException validationException)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;

            var errors = validationException.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.ErrorMessage).ToArray()
                );

            await context.Response.WriteAsJsonAsync(new { errors });
            return;
        }

        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
    });
});

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// CORS
app.UseCors("AllowNuxt");

// SignalR
app.MapHub<NotificationHub>("/notifications").RequireAuthorization();

// Auth
app.UseAuthentication();
app.UseAuthorization();

// Controllers
app.MapControllers();

app.Run();