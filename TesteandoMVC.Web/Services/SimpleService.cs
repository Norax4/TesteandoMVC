namespace TesteandoMVC.Web.Services
{
    public interface ISimpleService
    {
        bool HoraEsPar();
        int NumeroAleatorio();
        bool ValidarUsuario(string usuario, string password);

        string ObtenerSaludo(string nombre);
        bool EsFinDeSemana();
        int CalcularEdad(DateTime fechaNacimiento);
        bool EsUsuarioPremium(string email);
    }

    public class SimpleService : ISimpleService
    {
        private readonly Random _random = new Random();

        public bool EsUsuarioPremium(string email) 
        {
            return true; // Simplificado para el ejemplo
        }

        public int CalcularEdad(DateTime fechaNacimiento) 
        {
            return 18;
        }

        public bool EsFinDeSemana() 
        {
            return true; // Simplificado para el ejemplo
        }

        public string ObtenerSaludo(string nombre)
        {
            return $"Hola, {nombre}!";
        }

        public bool HoraEsPar()
        {
            // Si la hora es un numero par, devuelve true, sino devuelve false
            return DateTime.Now.Hour % 2 == 0;
        }

        public int NumeroAleatorio()
        {
            // Devuelve un número aleatorio entre 1 y 100
            return _random.Next(1, 101);
        }

        public bool ValidarUsuario(string usuario, string password)
        {
            // Valida usuario y contraseña específicos
            return usuario == "don_correcto" && password == "iatusabes";
        }
    }
}
