namespace Che_Gato.Views;

public partial class ProfilePage : ContentPage
{
    public ProfilePage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        
        ProfileName.Text = Preferences.Default.Get("userName", "Usuario Gatuno");
        ProfileEmail.Text = Preferences.Default.Get("userEmail", "correo@ejemplo.com");
        
        AvatarImage.Source = Preferences.Default.Get("userAvatar", "https://cdn-icons-png.flaticon.com/512/3069/3069170.png");

        OrdersLabel.Text = Preferences.Default.Get("userOrders", 0).ToString();
        FavsLabel.Text = Preferences.Default.Get("userFavs", 0).ToString();

        ProfileContainer.Opacity = 0;
        ProfileContainer.TranslationY = 30;

        await Task.WhenAll(
            ProfileContainer.FadeTo(1, 600, Easing.CubicOut),
            ProfileContainer.TranslateTo(0, 0, 600, Easing.CubicOut)
        );
    }

    private async void OnChangeAvatarTapped(object sender, EventArgs e)
    {
        string action = await DisplayActionSheet("Elige tu avatar", "Cancelar", null, "Koala 🐨", "Rana 🐸", "Tigre 🐯", "Cangrejo 🦀");
        string newUrl = "";

        if (action == "Koala 🐨") newUrl = "https://cdn-icons-png.flaticon.com/512/3069/3069172.png";
        else if (action == "Rana 🐸") newUrl = "https://cdn-icons-png.flaticon.com/512/3069/3069170.png";
        else if (action == "Tigre 🐯") newUrl = "https://cdn-icons-png.flaticon.com/512/3069/3069173.png";
        else if (action == "Cangrejo 🦀") newUrl = "https://cdn-icons-png.flaticon.com/512/3069/3069168.png";

        if (!string.IsNullOrEmpty(newUrl))
        {
            AvatarImage.Source = newUrl;
            Preferences.Default.Set("userAvatar", newUrl); 
        }
    }

    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        await button.ScaleTo(0.9, 100);
        await button.ScaleTo(1.0, 100);

        bool answer = await DisplayAlert("Cerrar Sesión", "¿Seguro que quieres abandonar la manada?", "Sí, salir", "Cancelar");
        
        if (answer)
        {
            Preferences.Default.Clear(); 
            await Shell.Current.GoToAsync("//home");
        }
    }
}