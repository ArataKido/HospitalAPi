using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Timetables.Core.Context;
using Timetables.Core.Entity;
using Timetables.Services;
using Timetables.Utils.Mappers;
using Timetables.Validators;
using MassTransit;
// using Timetables.Core.Contracts;
using Timetables.Core;
using Contracts.Models;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddAutoMapper(typeof(TimeTableMappingProfile));
builder.Services.AddControllers();

builder.Services.AddDbContext<PostgresDbContext>();
builder.Services.AddScoped<ITimeTableService,TimeTableService>();
builder.Services.AddScoped<IValidator<TimeTable>, TimeTableValidator>();
builder.Services.AddScoped<MicroserviceClient>();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Debug);

builder.Services.AddMassTransit(config => {
    config.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ConfigureEndpoints(context, KebabCaseEndpointNameFormatter.Instance);
    });
    config.AddLogging();
    
    // config.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("timetables", false));
    config.AddRequestClient<DoctorRequest>(
                destinationAddress: new Uri("queue:doctor-request-queue"),
                timeout: RequestTimeout.After(s: 30));
});
builder.Services.AddMassTransitHostedService();

builder.Logging.ClearProviders();

builder.Logging.AddConsole();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "AuthCore API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            }, new List<string>()
        }
    });

});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var token = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            // ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
            RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
        };
        options.TokenValidationParameters = token;

        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine($"Authentication failed: {context.Exception.Message}");
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();

app.MapControllers();

app.Run();


