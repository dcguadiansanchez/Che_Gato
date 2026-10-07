namespace Che_Gato.Services;

public class DevHttpsConnectionHelper
{
    public HttpClientHandler GetPlatformMessageHandler()
    {
        var handler = new HttpClientHandler();
#if IOS
        // Le dice a iOS que confíe en nuestro servidor local de prueba
        handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
        {
            return cert?.Issuer.Equals("CN=localhost") == true || errors == System.Net.Security.SslPolicyErrors.None;
        };
#endif
        return handler;
    }
}