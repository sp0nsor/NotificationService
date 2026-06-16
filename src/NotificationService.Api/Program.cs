using NotificationService.Api.Extensions;
using NotificationService.DataAccess.Extentions;
using NotificationService.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddDatabaseConfig(builder.Configuration);
services.AddInfrastructure(builder.Configuration);

services.AddControllers();
services.AddSwaggerGen();

var app = builder.Build();

await app.InitializeDatabaseAsync();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapControllers();

app.Run();
