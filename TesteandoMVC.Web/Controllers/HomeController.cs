using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TesteandoMVC.Web.Models;
using TesteandoMVC.Web.Services;

namespace TesteandoMVC.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ISimpleService _simpleService;
    private readonly ITestDoublesService _testDoublesService;

    public HomeController(
        ILogger<HomeController> logger, 
        ISimpleService simpleService,
        ITestDoublesService testDoublesService)
    {
        _logger = logger;
        _simpleService = simpleService;
        _testDoublesService = testDoublesService;
    }
    public IActionResult Premium() 
    {
        return View();
    }

    [HttpPost]
    public IActionResult ValidarPremium(string email)
    {
        ViewBag.EsPremium = _simpleService.EsUsuarioPremium(email);
        return View("Premium");
    }

    public IActionResult Saludo() 
    {
        return View();
    }

    [HttpPost]
    public IActionResult ObtenerSaludo(string nombre) 
    {
        ViewBag.Saludo = _simpleService.ObtenerSaludo(nombre);
        return View("Saludo");
    }

    [HttpGet]
    public IActionResult FinDeSemana() 
    {
        ViewBag.EsFinDeSemana = _simpleService.EsFinDeSemana();
        return View();
    }

    [HttpGet]
    public IActionResult Edad() 
    {
        return View();
    }

    [HttpPost]
    public IActionResult CalcularEdad(DateTime anioNacimiento) 
    {
        ViewBag.Edad = _simpleService.CalcularEdad(anioNacimiento);
        return View("Edad");
    }

    public IActionResult Index()
    {
        ViewBag.ShowMessage = _simpleService.HoraEsPar();
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult NumeroAleatorio()
    {
        ViewBag.NumeroAleatorio = _simpleService.NumeroAleatorio();
        return View();
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult ValidarLogin(string usuario, string password)
    {
        bool esValido = _simpleService.ValidarUsuario(usuario, password);
        
        ViewBag.EsValido = esValido;
        ViewBag.Mensaje = esValido ? 
            "¡Bienvenido! Has iniciado sesión correctamente." : 
            "Usuario o contraseña incorrectos. Intenta de nuevo.";
        
        return View("Login");
    }

    /// <summary>
    /// Demo simple de Test Doubles usando un solo servicio
    /// </summary>
    public IActionResult TestDoubles(string input = "demo")
    {
        var resultado = new
        {
            // DUMMY: logger se pasa pero no se usa
            TextoProcesado = _testDoublesService.ProcesarTexto(_logger, input),
            
            // STUB: valores predefinidos
            DescuentoVIP = _testDoublesService.CalcularDescuento("VIP"),
            DescuentoRegular = _testDoublesService.CalcularDescuento("REGULAR"),
            
            // FAKE: operaciones en memoria
            DatoGuardado = GuardarYObtenerDato(input),
            
            // MOCK & SPY: estos se verifican en tests
            ValidacionExitosa = _testDoublesService.ValidarYProcesar(input)
        };

        // MOCK: este método se verifica en tests
        _testDoublesService.EnviarNotificacion($"Demo ejecutado con input: {input}");

        ViewBag.Resultado = resultado;
        ViewBag.Input = input;
        return View();
    }

    private string GuardarYObtenerDato(string input)
    {
        var key = "ultimo_input";
        _testDoublesService.GuardarDato(key, input);
        return _testDoublesService.ObtenerDato(key);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
