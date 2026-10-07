namespace Che_Gato.Views;

public partial class RegisterPage : ContentPage
{
    public RegisterPage()
    {
        InitializeComponent();
    }

    // Animación de entrada al abrir la pantalla
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        
        MainForm.Opacity = 0;
        MainForm.TranslationY = 50;

        await Task.WhenAll(
            MainForm.FadeTo(1, 800, Easing.CubicOut),
            MainForm.TranslateTo(0, 0, 800, Easing.CubicOut)
        );
    }

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        await button.ScaleTo(0.9, 100, Easing.CubicIn);
        await button.ScaleTo(1.0, 100, Easing.CubicOut);

        string nombre = NameEntry.Text;
        string correo = RegEmailEntry.Text;
        string password = RegPasswordEntry.Text;

        if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(correo) || string.IsNullOrWhiteSpace(password))
        {
            await DisplayAlert("Aviso", "¡Miauch! Faltan datos por llenar.", "OK");
            return;
        }

        // Llamamos a la API real
        var apiService = new Services.ApiService();
        bool exito = await apiService.RegisterAsync(nombre, correo, password);

        if (exito)
        {
            await DisplayAlert("¡Éxito!", "Cuenta creada correctamente. ¡Bienvenido a la manada!", "OK");
            await Shell.Current.GoToAsync(".."); // Regresa al login
        }
        else
        {
            await DisplayAlert("Error", "No se pudo registrar. Tal vez el correo ya existe.", "Entendido");
        }
    }

    private async void OnBackToLoginTapped(object sender, EventArgs e)
    {
        // Pequeño rebote en el texto al tocarlo
        var label = (Label)sender;
        await label.ScaleTo(0.8, 100);
        await label.ScaleTo(1.0, 100);

        await Shell.Current.GoToAsync("..");
    }
}