using Timetables.Core.Settings;

var builder = WebApplication.CreateBuilder(args);

ServiceConfiguration.ConfigureServices(builder);
LoggingConfiguration.ConfigureLogging(builder.Logging);
AuthenticationConfiguration.ConfigureAuthentication(builder);
SwaggerConfiguration.ConfigureSwagger(builder.Services);
MassTransitConfiguration.ConfigureMassTransit(builder.Services);

var app = builder.Build();

// Middleware Setup
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
