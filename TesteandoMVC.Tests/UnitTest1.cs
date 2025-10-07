using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Net;
using System.Threading.Tasks;
using TesteandoMVC.Web.Services;
using Xunit;

namespace TesteandoMVC.Tests
{
    public class UnitTest1 : IClassFixture<WebApplicationFactory<TesteandoMVC.Web.Program>>
    {
        private readonly WebApplicationFactory<TesteandoMVC.Web.Program> _factory;

        public UnitTest1(WebApplicationFactory<TesteandoMVC.Web.Program> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("/")]
        [InlineData("/Home/Privacy")]
        public async Task Pages_Return_Status200(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Pagina_Home()
        {
            // Arrange
            // De alguna forma le hacemos creer al sistema que es otra hora
            // De alguna forma le decimos al codigo, que siempre que llame a la funcion
            // HoraEsPar, devuelva true

            // Codigo para hacer creer el cambio de hora
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/");

            // Assert
            // Chequeamos que el H1 tenga el contenido que queremos
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        
    }
}
