namespace Che_Gato.Views;

public partial class HomePage : ContentPage
{
    public HomePage()
    {
        InitializeComponent();
    }

    // Animación de entrada al abrir la app
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Verificamos si el usuario ya había iniciado sesión antes
        if (Preferences.Default.Get("isLoggedIn", false))
        {
            // Salta directo al catálogo sin mostrar el inicio
            await Shell.Current.GoToAsync("//catalog");
            return; 
        }
        
        // Si no ha iniciado sesión, hacemos la animación de la tarjeta normal
        MainContainer.Opacity = 0;
        MainContainer.TranslationY = 50;

        await Task.WhenAll(
            MainContainer.FadeTo(1, 800, Easing.CubicOut),
            MainContainer.TranslateTo(0, 0, 800, Easing.CubicOut)
        );
    }

    private async void OnComenzarClicked(object sender, EventArgs e)
    {
        // Efecto de rebote físico
        var button = (Button)sender;
        await button.ScaleTo(0.9, 100, Easing.CubicIn);
        await button.ScaleTo(1.0, 100, Easing.CubicOut);

        // Envía al usuario a la pantalla de Inicio de Sesión
        await Navigation.PushAsync(new LoginPage());
    }

    private async void OnTestConnectionClicked(object sender, EventArgs e)
{
    // Efecto de rebote físico
    var button = (Button)sender;
    await button.ScaleTo(0.95, 100, Easing.CubicIn);
    await button.ScaleTo(1.0, 100, Easing.CubicOut);

    // Llamamos a la API
    var apiService = new Services.ApiService();
    bool isConnected = await apiService.TestConnectionAsync();

    if (isConnected)
    {
        await DisplayAlert("¡Conectado! 🐾", "El servidor de Che Gato está respondiendo perfectamente. Todo listo.", "Miau");
    }
    else
    {
        await DisplayAlert("Error", "No se pudo contactar al servidor. Revisa que la otra ventana de VS Code siga corriendo.", "Entendido");
    }
}
}