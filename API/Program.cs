using API.Services;
using Prometheus;
using RabbitMQ.Client;
using System.Data;
using System.Data.SqlClient;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            // Add services to the container.
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<IConnectionFactory, ConnectionFactory>(provider => new ConnectionFactory { HostName = "rabbitQueue" });

            var stringConexao = configuration.GetValue<string>("ConnectionStringSQL");

            builder.Services.AddScoped<IDbConnection>((conexao) => new SqlConnection(stringConexao));

            builder.Services.AddSingleton<MetricsService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();

            // Adicione o middleware de m�tricas do Prometheus
            app.UseHttpMetrics(); // Middleware para m�tricas HTTP

            app.MapControllers();

            // Endpoint de m�tricas do Prometheus
            app.MapMetrics();

            app.UseMetricServer();

            app.Run();
        }
    }
}

public partial class Program { }