using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using MongoDB.Driver;
using ProfilerBusiness;
using ProfilerCQRS.Commands;
using ProfilerWebAPI.Mongo;
using ProfilerModels;
using ProfilerModels.Abstractions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("ProfilerDB"));

builder.Services.AddSingleton<IMongoDBService, MongoDBService>();

builder.Services.AddScoped<IMessageBroker, MessageBroker>();

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssemblies(
        Assembly.GetExecutingAssembly(),
        typeof(CreateProfileCommand).Assembly));

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
