namespace Che_Gato.Models;

public class Producto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public decimal Precio { get; set; }
    public string Descripcion { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public List<Restaurante> Restaurantes { get; set; } = new(); 
}
