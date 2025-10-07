using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Net;
using TesteandoMVC.Web.Services;
using Xunit;

namespace TesteandoMVC.Tests
{
    public class HomeControllerTests : IClassFixture<WebApplicationFactory<TesteandoMVC.Web.Program>>
    {
        private readonly WebApplicationFactory<TesteandoMVC.Web.Program> _factory;

        public HomeControllerTests(WebApplicationFactory<TesteandoMVC.Web.Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Index_CuandoElServicioDevuelveTrue_MuestraMensajeDeExito()
        {
            // Arrange
            var mockService = new Mock<ISimpleService>();
            mockService.Setup(x => x.HoraEsPar()).Returns(true);
            
            // EJEMPLO: Si el método recibiera parámetros, el mock se haría así:
            // mockService.Setup(x => x.ValidarUsuario("usuario", "password")).Returns(true);
            // mockService.Setup(x => x.ValidarUsuario(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            // mockService.Setup(x => x.ValidarUsuario(It.Is<string>(u => u == "don_correcto"), It.Is<string>(p => p == "iatusabes"))).Returns(true);

            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddScoped<ISimpleService>(_ => mockService.Object);
                });
            }).CreateClient();

            // Act
            var response = await client.GetAsync("/");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("¡El servicio funciona correctamente!", content);
        }

        [Fact]
        public async Task Index_CuandoElServicioDevuelveFalse_MuestraMensajeDeError()
        {
            // Arrange
            var mockService = new Mock<ISimpleService>();
            mockService.Setup(x => x.HoraEsPar()).Returns(false);

            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddScoped<ISimpleService>(_ => mockService.Object);
                });
            }).CreateClient();

            // Act
            var response = await client.GetAsync("/");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("¡El servicio no está funcionando!", content);
        }

        [Theory]
        [InlineData(987)]
        [InlineData(123)]
        public async Task NumeroAleatorio_Exito(int num)
        {
            //Arrange
            var mockService = new Mock<ISimpleService>();
            mockService.Setup(x => x.NumeroAleatorio()).Returns(num);

            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddScoped<ISimpleService>(_ => mockService.Object);
                });
            }).CreateClient();

            //Act
            HttpResponseMessage response = await client.GetAsync("/Home/NumeroAleatorio");

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains($"<h2>Tu número de la suerte es: {num}</h2>", content);
        }

        [Fact]
        public async Task Validar_Login()
        {
            //Arrange
            var mockService = new Mock<ISimpleService>();
            mockService.Setup(x => x.ValidarUsuario(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddScoped<ISimpleService>(_ => mockService.Object);
                });
            }).CreateClient();

            string json = "{\"usuario\":\"uno\",\"password\":\"dos\"}";
            StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            //Act
            HttpResponseMessage response = await client.PostAsync("/Home/ValidarLogin", httpContent);

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("¡Bienvenido! Has iniciado sesión correctamente.", content);
        }
    }
}
