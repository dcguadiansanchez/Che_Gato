namespace Che_Gato;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        // Instrucción para eliminar el borde nativo feo de iOS en todos los Entry
        Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("NoBorder", (handler, view) =>
        {
#if IOS
            handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
#endif
        });
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new AppShell());
    }
}