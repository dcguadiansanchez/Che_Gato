using System.Collections.ObjectModel;
using Che_Gato.Models;
using Che_Gato.Services; // Para que reconozca el CartService directamente

namespace Che_Gato.Views;

public partial class CatalogPage : ContentPage
{
    public ObservableCollection<Producto> Productos { get; set; } = new();
    private List<Producto> _allProductos = new();
    
    // El _cartCount viejo ya no está aquí, ahora la verdad viene de CartService
    private int _currentQty = 1;
    private Producto _selectedProducto;

    public CatalogPage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    // Se ejecuta al cargar la pantalla o regresar a ella
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await CargarMenuDesdeApi();
        ActualizarGloboCarrito(); // Sincroniza al abrir la pestaña
    }

    private async Task CargarMenuDesdeApi()
    {
        var apiService = new ApiService();
        _allProductos = await apiService.GetCatalogoAsync();

        Productos.Clear();
        foreach (var p in _allProductos)
        {
            Productos.Add(p);
        }
    }

    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        var keyword = e.NewTextValue?.ToLower() ?? "";
        Productos.Clear();
        var filtrados = _allProductos.Where(p => p.Nombre.ToLower().Contains(keyword));
        
        foreach (var p in filtrados)
        {
            Productos.Add(p);
        }
    }

    // 1. ABRE EL MODAL AL PRESIONAR "PEDIR"
    private async void OnOpenModalClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        await button.ScaleTo(0.85, 100, Easing.CubicIn);
        await button.ScaleTo(1.0, 100, Easing.CubicOut);

        // Obtenemos el producto exacto al que le dimos clic
        _selectedProducto = (Producto)button.CommandParameter;

        // Configuramos la ventana emergente con esos datos
        ModalTitle.Text = _selectedProducto.Nombre;
        ModalDesc.Text = _selectedProducto.Descripcion;
        RestaurantePicker.ItemsSource = _selectedProducto.Restaurantes;
        
        // Seleccionamos el primer restaurante por defecto si es que tiene
        if (_selectedProducto.Restaurantes.Any())
            RestaurantePicker.SelectedIndex = 0;

        _currentQty = 1;
        ActualizarBotonModal();

        // Mostramos el modal con una pequeña animación de desvanecimiento
        ModalOverlay.Opacity = 0;
        ModalOverlay.IsVisible = true;
        await ModalOverlay.FadeTo(1, 200);
    }

    // 2. BOTONES DE CANTIDAD (+ y -)
    private void OnMinusClicked(object sender, EventArgs e)
    {
        if (_currentQty > 1)
        {
            _currentQty--;
            ActualizarBotonModal();
        }
    }

    private void OnPlusClicked(object sender, EventArgs e)
    {
        if (_currentQty < 20) // Límite de 20 para no romper el carrito
        {
            _currentQty++;
            ActualizarBotonModal();
        }
    }

    private void ActualizarBotonModal()
    {
        ModalQty.Text = _currentQty.ToString();
        decimal total = _currentQty * _selectedProducto.Precio;
        ModalConfirmBtn.Text = $"Agregar por {total:C2}";
    }

    // 3. CERRAR EL MODAL
    private async void OnCloseModalTapped(object sender, EventArgs e)
    {
        await ModalOverlay.FadeTo(0, 200);
        ModalOverlay.IsVisible = false;
    }

    // 4. CONFIRMAR Y MANDAR AL CARRITO
    private async void OnConfirmAddClicked(object sender, EventArgs e)
    {
        var restauranteElegido = (Restaurante)RestaurantePicker.SelectedItem;
        if (restauranteElegido == null)
        {
            await DisplayAlert("Aviso", "Por favor, elige el restaurante de donde deseas pedir.", "OK");
            return;
        }

        // Efecto del botón
        await ModalConfirmBtn.ScaleTo(0.95, 100);
        await ModalConfirmBtn.ScaleTo(1.0, 100);

        // Guardamos el producto real en el servicio del carrito las veces que el usuario pidió
        for (int i = 0; i < _currentQty; i++)
        {
            CartService.AddProduct(_selectedProducto);
        }

        // Cerramos el modal
        await ModalOverlay.FadeTo(0, 200);
        ModalOverlay.IsVisible = false;

        // Sincronizamos el globo y hacemos la animación
        ActualizarGloboCarrito();
        await CartBadge.ScaleTo(1.5, 100);
        await CartBadge.ScaleTo(1.0, 100);

        await DisplayAlert("¡Listo!", $"Agregaste {_currentQty} {_selectedProducto.Nombre} de {restauranteElegido.Nombre} al carrito.", "Miau");
    }

    // 5. ACTUALIZAR EL GLOBO ROJO
    private void ActualizarGloboCarrito()
    {
        // Suma las cantidades reales que hay en el servicio
        int totalItems = CartService.Items.Sum(i => i.Quantity);
        CartBadge.Text = totalItems.ToString();
    }
}