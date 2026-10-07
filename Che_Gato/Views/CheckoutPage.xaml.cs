using Che_Gato.Services;

namespace Che_Gato.Views;

public partial class CheckoutPage : ContentPage
{
    private string _selectedPayment = "Tarjeta";

    public CheckoutPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        ItemCountLabel.Text = CartService.Items.Count.ToString();
        TotalLabel.Text = $"${CartService.Total:N2}";

        SelectPaymentVisual(CardTarjeta, CardEfectivo);
    }

    private void OnSelectCard(object sender, TappedEventArgs e)
    {
        _selectedPayment = "Tarjeta";
        SelectPaymentVisual(CardTarjeta, CardEfectivo);
    }

    private void OnSelectCash(object sender, TappedEventArgs e)
    {
        _selectedPayment = "Efectivo";
        SelectPaymentVisual(CardEfectivo, CardTarjeta);
    }

    // Selector "falso": solo cambia el estilo visual, no hay lógica de pago real.
    private void SelectPaymentVisual(Border selected, Border unselected)
    {
        selected.Stroke = (Color)Application.Current!.Resources["CatOrange"];
        selected.BackgroundColor = (Color)Application.Current!.Resources["CatOrangeSoft"];

        unselected.Stroke = (Color)Application.Current!.Resources["CatMist"];
        unselected.BackgroundColor = (Color)Application.Current!.Resources["CatWhisker"];
    }

    // Módulo 5: simula la confirmación del pedido.
    // El backend deberá reemplazar esto por la llamada real a la API de pedidos.
    private async void OnConfirmClicked(object sender, EventArgs e)
    {
        if (CartService.Items.Count == 0)
        {
            await DisplayAlert("Carrito vacío", "Agrega al menos una croqueta antes de pagar 🐾", "OK");
            return;
        }

        var total = CartService.Total;

        await DisplayAlert(
            "¡Pedido confirmado! 🐾",
            $"Gracias por tu compra de ${total:N2} pagando con {_selectedPayment}. Tu pedido llegará más rápido que un gato corriendo a su plato.",
            "Genial");

        CartService.Clear();
        await Shell.Current.GoToAsync("//home");
    }
}
