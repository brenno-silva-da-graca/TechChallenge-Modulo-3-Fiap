using System.Data;
using System.Data.SqlClient;
using Application.Interfaces;
using Infrastructure.Repositories;
using TechChallenge_Contatos.Repository;
using InfrastructureWebApi.MessageConsumers;

var builder = WebApplication.CreateBuilder(args);

var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IContatoCadastro, ContatoRepository>();
builder.Services.AddScoped<IDDDCadastro, DDDRepository>();
builder.Services.AddScoped<IContatoConsumer, ContatoConsumer>();

var stringConexao = configuration.GetValue<string>("ConnectionStringSQL");

builder.Services.AddTransient<IDbConnection>((conexao) => new SqlConnection(stringConexao));

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

app.Logger.LogWarning("Started Infra");
