using Microsoft.Extensions.Configuration;

using TaaghchehTask.Abstraction.Configs;
using TaaghchehTask.Abstraction.Services;
using TaaghchehTask.Service;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

// Add services to the container.
services.Configure<TaaghchehSettings>(builder.Configuration.GetSection("TaaghchehTask"));
services.AddHttpClient();
services.AddWTaaghchehervices();

services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
