using NotificationService.Api.Extensions;
using NotificationService.DataAccess.Extentions;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddDatabaseConfig(builder.Configuration);

services.AddControllers();
services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

await app.InitializeDatabaseAsync();

app.Run();
