using System.Data;
using System.Linq;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationTests
{
    public class ApiApplication : WebApplicationFactory<Program>
    {
        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IDbConnection));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_e_sqlite3());
                SQLitePCL.Batteries.Init();

                var connection = new SqliteConnection("DataSource=:memory:");
                connection.Open();

                services.AddSingleton<IDbConnection>(sp => connection);

                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<IDbConnection>();

                    // Initialize the database
                    using (var command = db.CreateCommand())
                    {
                        command.CommandText = @"
                            CREATE TABLE DDD (
                                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                NumDDD INTEGER,
                                regiao TEXT
                            );

                            CREATE TABLE Contatos (
                                ID INTEGER PRIMARY KEY AUTOINCREMENT,
                                Nome TEXT,
                                Telefone TEXT,
                                Email TEXT,
                                DDDID INTEGER NOT NULL,
                                FOREIGN KEY (DDDID) REFERENCES DDD(Id)
                            );
                        ";
                        command.ExecuteNonQuery();
                    }
                }
            });

            return base.CreateHost(builder);
        }
    }
}
