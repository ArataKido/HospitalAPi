using Accounts.Consumer;
using Accounts.Context;
using Accounts.Services;
using Accounts.Utils.Mappers;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "AuthCore API", Version = "v1" });
    // c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    // {
    //     Description = "JWT Authorization header using the Bearer scheme.",
    //     Name = "Authorization",
    //     In = ParameterLocation.Header,
    //     Type = SecuritySchemeType.Http,
    //     Scheme = "bearer"
    // });

    // c.AddSecurityRequirement(new OpenApiSecurityRequirement
    // {
    //     {
    //         new OpenApiSecurityScheme
    //         {
    //             Reference = new OpenApiReference
    //             {
    //                 Type = ReferenceType.SecurityScheme,
    //                 Id = "Bearer"
    //             }
    //         }, new List<string>()
    //     }
    // });

});

builder.Services.AddMassTransit(config => {
    config.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ConfigureEndpoints(context, KebabCaseEndpointNameFormatter.Instance);
        cfg.ReceiveEndpoint("doctor-request-queue", e =>
        {
            e.ConfigureConsumer<DoctorConsumer>(context);
            e.UseMessageRetry(r => r.Intervals(100, 200, 500, 1000, 2000, 5000));
            e.UseInMemoryOutbox();
        });
    });
    config.AddLogging();
    config.AddConsumer<DoctorConsumer>();
});
builder.Services.AddAutoMapper(typeof(AccountMappingProfile));

builder.Services.AddDbContext<PostgresDbContext>();
builder.Services.AddScoped<AccountService>();
builder.Services.AddScoped<AuthenticationService>();
builder.Services.AddScoped<PasswordService>();
builder.Services.AddScoped<DoctorConsumer>();

builder.Services.AddControllers();
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Debug);

// builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//     .AddJwtBearer(options =>
//     {
//         var token = new TokenValidationParameters
//         {
//             ValidateIssuer = true,
//             ValidateAudience = true,
//             // ValidateLifetime = true,
//             ValidateIssuerSigningKey = true,
//             ValidIssuer = builder.Configuration["Jwt:Issuer"],
//             ValidAudience = builder.Configuration["Jwt:Audience"],
//             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
//             RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
//         };
//         options.TokenValidationParameters = token;

//         options.Events = new JwtBearerEvents
//         {
//             OnAuthenticationFailed = context =>
//             {
//                 Console.WriteLine($"Authentication failed: {context.Exception.Message}");
//                 return Task.CompletedTask;
//             }
//         };
//     });

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


