using Application.Interfaces;
using System.Data;
using System.Data.SqlClient;
using TechChallenge_Contatos.Repository;
using WorkerMessageConsumer.Workers;

namespace WorkerMessageConsumer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            var stringConexao = configuration.GetValue<string>("ConnectionStringSQL");

            var builder = Host.CreateApplicationBuilder(args);
            builder.Services.AddHostedService<WorkerPostContato>();
            builder.Services.AddHostedService<WorkerPatchContato>();
            builder.Services.AddHostedService<WorkerDeleteContato>();

            builder.Services.AddTransient<IDbConnection>((conexao) => new SqlConnection(stringConexao));
            builder.Services.AddScoped<IContatoCadastro, ContatoRepository>();

            var host = builder.Build();
            Console.WriteLine("Starting all workers");
            host.Run();
        }
    }
}