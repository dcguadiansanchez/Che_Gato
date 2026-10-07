namespace Che_Gato.Views;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
    }

    // 1. Animación de Entrada al abrir la pantalla
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        
        // Escondemos y bajamos la tarjeta al inicio
        MainForm.Opacity = 0;
        MainForm.TranslationY = 50;

        // La hacemos aparecer y subir suavemente (efecto Slide Up)
        await Task.WhenAll(
            MainForm.FadeTo(1, 800, Easing.CubicOut),
            MainForm.TranslateTo(0, 0, 800, Easing.CubicOut)
        );
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        await button.ScaleTo(0.9, 100, Easing.CubicIn);
        await button.ScaleTo(1.0, 100, Easing.CubicOut);

        string email = EmailEntry.Text;
        string password = PasswordEntry.Text;

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            await DisplayAlert("Aviso", "No dejes a tu gato con hambre. Llena tus datos.", "OK");
            return;
        }

        // Llamamos a la API real
        var apiService = new Services.ApiService();
        string nombreUsuario = await apiService.LoginAsync(email, password);

        if (!string.IsNullOrEmpty(nombreUsuario))
        {
            // ¡MAGIA! Guardamos los datos en la memoria del teléfono
            Preferences.Default.Set("isLoggedIn", true);
            Preferences.Default.Set("userName", nombreUsuario);
            Preferences.Default.Set("userEmail", email);

            // Vamos al catálogo
            await Shell.Current.GoToAsync("//catalog");
        }
        else
        {
            await DisplayAlert("Error", "Correo o contraseña incorrectos. Intenta de nuevo.", "OK");
        }
    }

    private async void OnGoToRegisterTapped(object sender, EventArgs e)
    {
        // Animamos el texto de registro ligeramente antes de ir allá
        var label = (Label)sender;
        await label.ScaleTo(0.8, 100);
        await label.ScaleTo(1.0, 100);

        await Shell.Current.GoToAsync(nameof(RegisterPage));
    }
}