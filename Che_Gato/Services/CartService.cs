using System.Collections.ObjectModel;
using System.Linq;
using Che_Gato.Models;


namespace Che_Gato.Services;

/// <summary>
/// Servicio TEMPORAL en memoria para que el Catálogo, el Carrito y el
/// Checkout puedan mostrarse conectados durante la demo visual.
///
/// NOTA PARA EL EQUIPO:
/// Esta clase NO es persistencia real. El compañero encargado de la lógica
/// local (Preferences / SecureStorage) y el de backend pueden reemplazar
/// su contenido interno (o inyectar un servicio real con la misma forma)
/// sin tener que tocar ninguna de las Vistas XAML.
/// </summary>
public static class CartService
{
    public static ObservableCollection<CartItem> Items { get; } = new();

    public static void AddProduct(Producto producto)
    {
        var existing = Items.FirstOrDefault(i => i.Producto.Id == producto.Id);

        if (existing is not null)
        {
            existing.Quantity++;

            // CartItem no implementa INotifyPropertyChanged, así que forzamos
            // el refresco visual del CollectionView quitando y reinsertando el item.
            var index = Items.IndexOf(existing);
            Items.RemoveAt(index);
            Items.Insert(index, existing);
        }
        else
        {
            Items.Add(new CartItem { Producto = producto, Quantity = 1 });
        }
    }

    public static void RemoveItem(CartItem item) => Items.Remove(item);

    public static void Clear() => Items.Clear();

    public static decimal Total => Items.Sum(i => i.Subtotal);
}
