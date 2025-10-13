using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Net;
using System.Text;
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
            // mockService.Setup(x => x.ValidarUsuario("pepitro", "juansito")).Returns(true);
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
            // Alternativamente, si el método fuera POST
            // var response = await client.PostAsync("/", null);
            // O si el método fuera POST con parámetros
            // Para APIs que esperan JSON
            // string json = "{\"usuario\":\"don_correcto\",\"password\":\"iatusabes\"}";
            // StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            // HttpResponseMessage response = await client.PostAsync("/api/login", content);


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
        [InlineData(999999999)]
        public async Task NumeroAleatorio_CuandoElServicioDevuelveX_MuestraElNumeroDeLaSuerte(int num)
        {
            // Arrange
            var mockService = new Mock<ISimpleService>();
            mockService.Setup(x => x.NumeroAleatorio()).Returns(num);

            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddScoped<ISimpleService>(_ => mockService.Object);
                });
            }).CreateClient();

            // Act
            var response = await client.GetAsync("/Home/NumeroAleatorio");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains($"id=\"numero_aleatorio\">{num}</", content);
        }

        [Fact]
        public async Task Login_Exitoso_MuestraMensaje()
        {
            // Arrange
            var mockService = new Mock<ISimpleService>();
            mockService.Setup(x => x.ValidarUsuario(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            // EJEMPLO: Si el método recibiera parámetros, el mock se haría así:
            // mockService.Setup(x => x.ValidarUsuario("pepitro", "juansito")).Returns(true);
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
            var response = await client.PostAsync("/Home/ValidarLogin", null);
            // Alternativamente, si el método fuera POST
            // var response = await client.PostAsync("/", null);
            // O si el método fuera POST con parámetros
            // Para APIs que esperan JSON
            // string json = "{\"usuario\":\"don_correcto\",\"password\":\"iatusabes\"}";
            // StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            // HttpResponseMessage response = await client.PostAsync("/api/login", content);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("¡Bienvenido! Has iniciado sesión correctamente.", content);
        }

        [Theory]
        [InlineData("Es fin de semana", true)]
        [InlineData("No es fin de semana", false)]
        public async Task CuandoEsFinDeSemana_muestraMensajeCorrecto(string mensajeEsperado, bool esFinDeSemana)
        {
            // Arrange
            var mockService = new Mock<ISimpleService>();
            mockService.Setup(x => x.EsFinDeSemana()).Returns(esFinDeSemana);
            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddScoped<ISimpleService>(_ => mockService.Object);
                });
            }).CreateClient();

            // Act
            var response = await client.GetAsync("/Home/FinDeSemana");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains(mensajeEsperado, content);
        }

        [Fact]
        public async Task ObtenerSaludo_SeMuestraElSaludo_ConElNombreCorrecto()
        {
            // Arrange
            var mockService = new Mock<ISimpleService>();
            mockService.Setup(x => x.ObtenerSaludo(It.IsAny<string>())).Returns($"Hola, Carlos!");
            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddScoped<ISimpleService>(_ => mockService.Object);
                });
            }).CreateClient();

            // Act
            string json = "{\"nombre\":\"Carlos\"}";
            StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("/Home/ObtenerSaludo", httpContent);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("Hola, Carlos!", content);
        }

        [Theory]
        [InlineData(true, "Bienvenido, usuario premium")]
        [InlineData(false, "Acceso Denegado")]
        public async Task EsUsuarioPremium_DevuelveMensajeCorrecto_DependiendoElUsuario(bool esPremium, string mensaje)
        {
            // Arrange
            var mockService = new Mock<ISimpleService>();
            mockService.Setup(x => x.EsUsuarioPremium(It.IsAny<string>())).Returns(esPremium);
            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddScoped<ISimpleService>(_ => mockService.Object);
                });
            }).CreateClient();

            // Act
            string json = "{\"email\":\"a@gmail.com\"}";
            StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("/Home/ValidarPremium", httpContent);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains(mensaje, content);
        }

        [Theory]
        [InlineData(21, "Eres mayor de edad")]
        [InlineData(16, "Eres menor de edad")]
        [InlineData(18, "Eres mayor de edad")]
        public async Task CalcularEdad_DevuelveMensajeCorrecto_DependiendoLaEdad(int edad, string mensaje)
        {
            // Arrange
            var mockService = new Mock<ISimpleService>();
            mockService.Setup(x => x.CalcularEdad(It.IsAny<DateTime>())).Returns(edad);
            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddScoped<ISimpleService>(_ => mockService.Object);
                });
            }).CreateClient();

            // Act
            string json = "{\"fechaNacimiento\":\"2000-01-01\"}";
            StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("/Home/CalcularEdad", httpContent);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains(mensaje, content);
        }
    }
}
