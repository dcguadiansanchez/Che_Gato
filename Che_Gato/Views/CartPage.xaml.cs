using Che_Gato.Services;
using Che_Gato.Models;

namespace Che_Gato.Views;

public partial class CartPage : ContentPage
{
    public CartPage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    // Exponemos la lista del carrito real para que la pantalla la vea
    public System.Collections.ObjectModel.ObservableCollection<CartItem> Items => CartService.Items;

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        ActualizarTotales();
        
        CartCollectionView.Opacity = 0;
        CartCollectionView.TranslationY = 30;

        await Task.WhenAll(
            CartCollectionView.FadeTo(1, 600, Easing.CubicOut),
            CartCollectionView.TranslateTo(0, 0, 600, Easing.CubicOut)
        );
    }

    private void ActualizarTotales()
    {
        decimal subtotal = CartService.Total;
        decimal envio = subtotal > 0 ? 25.00m : 0; // Si no hay nada, el envío es $0
        decimal total = subtotal + envio;

        SubtotalLabel.Text = $"${subtotal:F2}";
        EnvioLabel.Text = $"${envio:F2}";
        TotalLabel.Text = $"${total:F2}";
    }

    private void OnRemoveItemTapped(object sender, TappedEventArgs e)
    {
        var item = e.Parameter as CartItem;
        if (item != null)
        {
            CartService.RemoveItem(item);
            ActualizarTotales();
        }
    }
    private async void OnCheckoutClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        await button.ScaleTo(0.95, 100, Easing.CubicIn);
        await button.ScaleTo(1.0, 100, Easing.CubicOut);

        // 1. Primero verificamos si está vacío. Si lo está, cortamos todo aquí con el 'return'
        if (CartService.Items.Count == 0)
        {
            await DisplayAlert("Carrito vacío", "Aún no tienes platillos para pagar.", "OK");
            return;
        }

        // 2. Si pasamos el filtro anterior, significa que sí hay comida. Mostramos éxito:
        await DisplayAlert("¡Orden recibida!", "Tus alimentos están en preparación y el repartidor ha sido notificado.", "¡Genial!");
        
        // 3. Sumar 1 a los pedidos históricos del usuario (ahora sí es una compra real)
        int pedidosActuales = Preferences.Default.Get("userOrders", 0);
        Preferences.Default.Set("userOrders", pedidosActuales + 1);

        // 4. Vaciamos el carrito después de pagar y regresamos los montos a cero
        CartService.Clear();
        ActualizarTotales();
    }
}