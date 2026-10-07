using Che_Gato.Views;

namespace Che_Gato;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // Rutas "globales" que NO forman parte de la barra de pestañas.
        // Se navegan con Shell.Current.GoToAsync(nameof(Pagina)).
        Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
        Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
        Routing.RegisterRoute(nameof(CheckoutPage), typeof(CheckoutPage));
    }
}
