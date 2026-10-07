namespace Che_Gato.Models;

public class CartItem
{
    public Producto Producto { get; set; }
    public int Quantity { get; set; }

    // Le enseñamos al carrito cómo calcular el precio de (Cantidad x Precio)
    public decimal Subtotal => Producto != null ? Producto.Precio * Quantity : 0;
}