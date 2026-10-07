using System.Text.Json;

namespace Che_Gato.Services;

public class ApiService
{
    private readonly HttpClient _httpClient;
    
    // Aquí ponemos EXACTAMENTE el puerto que te dio tu terminal
    private readonly string _baseUrl = "http://localhost:5160/"; 

    public ApiService()
    {
        var helper = new DevHttpsConnectionHelper();
        _httpClient = new HttpClient(helper.GetPlatformMessageHandler());
        // Establecemos un tiempo de espera corto para que no se quede colgado
        _httpClient.Timeout = TimeSpan.FromSeconds(10);
    }

    // Método para probar si el servidor responde
    public async Task<bool> TestConnectionAsync()
    {
        try
        {
            // Hacemos una petición rápida al controlador de clima que tienen por defecto
            HttpResponseMessage response = await _httpClient.GetAsync(_baseUrl + "WeatherForecast");
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error de conexión: {ex.Message}");
            return false;
        }
    }

    // Método para Registrar un usuario real
    public async Task<bool> RegisterAsync(string nombre, string correo, string password)
    {
        try
        {
            // Empaquetamos los datos como le gustan a la API
            var usuario = new { Nombre = nombre, Correo = correo, Password = password };
            string json = JsonSerializer.Serialize(usuario);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            // Enviamos el paquete a la ruta de registro
            var response = await _httpClient.PostAsync(_baseUrl + "api/auth/register", content);
            return response.IsSuccessStatusCode;
        }
        catch 
        { 
            return false; 
        }
    }

    // Método para Iniciar Sesión y obtener el nombre del usuario
    public async Task<string> LoginAsync(string correo, string password)
    {
        try
        {
            var loginData = new { Correo = correo, Password = password };
            string json = JsonSerializer.Serialize(loginData);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(_baseUrl + "api/auth/login", content);
            
            if (response.IsSuccessStatusCode)
            {
                // Si el login es correcto, extraemos el nombre del usuario de la respuesta
                var responseString = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(responseString);
                return doc.RootElement.GetProperty("nombre").GetString();
            }
            return null; // Si falla la contraseña, devuelve nulo
        }
        catch 
        { 
            return null; 
        }
    }
    public async Task<List<Models.Producto>> GetCatalogoAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync(_baseUrl + "api/menu/catalogo");
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                return JsonSerializer.Deserialize<List<Models.Producto>>(json, options) ?? new List<Models.Producto>();
            }
        }
        catch 
        { 
        }
        return new List<Models.Producto>();
    }
}