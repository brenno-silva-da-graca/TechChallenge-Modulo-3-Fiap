using Xunit;
using Dapper;
using System.Data;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace IntegrationTests
{
    public class ContatosControllerIntegrationTests : IClassFixture<ApiApplication>
    {
        private readonly HttpClient _client;
        private readonly ApiApplication _factory;

        public ContatosControllerIntegrationTests(ApiApplication factory)
        {
            _client = factory.CreateClient();
            _factory = factory;
        }

        [Fact]
        public async Task GetContato_ReturnsSuccess()
        {
            using (var scope = _factory.Services.CreateScope())
            {
                var dbConnection = scope.ServiceProvider.GetRequiredService<IDbConnection>();

                // Arrange
                await SeedDatabase(dbConnection);

                // Act
                var response = await _client.GetAsync("/api/Contatos/Listar");

                // Assert
                response.EnsureSuccessStatusCode();
                Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());

                await CleanupDatabase(dbConnection);
            }
        }

        private async Task SeedDatabase(IDbConnection dbConnection)
        {
            var commandText = @"
                INSERT INTO DDD (NumDDD, regiao) VALUES (11, 'São Paulo'); 
                INSERT INTO Contatos (Nome, Telefone, Email, DDDID) VALUES ('Teste', '11123456789', 'teste@example.com', (SELECT Id FROM DDD WHERE NumDDD = 11));";

            await dbConnection.ExecuteAsync(commandText);
        }

        private async Task CleanupDatabase(IDbConnection dbConnection)
        {
            var commandText = @"DELETE FROM Contatos; DELETE FROM DDD;";
            await dbConnection.ExecuteAsync(commandText);
        }

        // Adicione mais testes conforme necessário
    }
}
