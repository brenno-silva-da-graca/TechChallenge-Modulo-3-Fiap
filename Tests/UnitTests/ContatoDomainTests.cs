using Domain.Repositories;

namespace Tests
{
    public class ContatoDomainTests
    {
        [Fact]
        public void TestEmailIsOk()
        {
            // Arrange
            string email = "comarroba@fiap.com";

            // Act
            bool result = ContatoRepositoryDomain.EmailIsOk(email);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void TestEmailIsNotOk()
        {
            // Arrange
            string email = "semarrobafiap.com";

            // Act
            bool result = ContatoRepositoryDomain.EmailIsOk(email);

            // Assert
            Assert.False(result);
        }
        [Fact]
        public void TestTelefoneIsOk()
        {
            //Arrange
            string telefone = "(11)9988-6543";

            //Act
            bool result = ContatoRepositoryDomain.TelefoneIsOk(telefone);

            //Assert
            Assert.True(result);
        }
        [Fact]
        public void TestTelefoneIsNotOk()
        {
            //Arrange
            string telefone = "11)A98643";

            //Act
            bool result = ContatoRepositoryDomain.TelefoneIsOk(telefone);

            //Assert
            Assert.False(result);
        }
    }
}