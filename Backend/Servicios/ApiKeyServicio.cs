using System.Text.RegularExpressions;
using AccesoADatos.InterfacesRepositorios;
using Dominio.Excepciones;
using Servicios.InterfacesServicios;

namespace Servicios;

public class ApiKeyServicio : IApiKeyServicio
{
    private readonly IApiKeyRepositorio _apiKeyRepositorio;
    public ApiKeyServicio(IApiKeyRepositorio apiKeyRepositorio)
    {
        _apiKeyRepositorio = apiKeyRepositorio;
    }

    public bool EsClaveValida(string apiKey)
    {
        ValidarApiKeyNoVacia(apiKey);
        ValidarApiKeyFormatoValido(apiKey);

        return _apiKeyRepositorio.Existe(Guid.Parse(apiKey));
    }

    private static void ValidarApiKeyNoVacia(string apiKey)
    {
        if(string.IsNullOrEmpty(apiKey))
        {
            throw new NoAutorizadoExcepcion("API Key invalida o ausente");
        }
    }

    private static void ValidarApiKeyFormatoValido(string apiKey)
    {
        var regex = new Regex(@"^[A-Z0-9]{8}-[A-Z0-9]{4}-[A-Z0-9]{4}-[A-Z0-9]{4}-[A-Z0-9]{12}$");
        if(!regex.IsMatch(apiKey))
        {
            throw new NoAutorizadoExcepcion("API Key invalida o ausente");
        }
    }
}
