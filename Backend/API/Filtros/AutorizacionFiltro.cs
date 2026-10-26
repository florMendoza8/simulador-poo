using Dominio.Excepciones;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Servicios.InterfacesServicios;

namespace API.Filtros;

public class AutorizacionFiltro : Attribute, IAuthorizationFilter
{
    private readonly string _tipo;
    private const string ENCABEZADO_APIKEY = "X-API-KEY";
    private const string MENSAJE_ERROR = "API Key inválida o ausente";

    public AutorizacionFiltro(string tipo)
    {
        _tipo = tipo;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        try
        {
            if (_tipo == "apiKey")
            {
                var apiKey = context.HttpContext.Request.Headers[ENCABEZADO_APIKEY].ToString();
                var apiKeyServicio = ObtenerServicioApiKey(context);

                apiKeyServicio.EsClaveValida(apiKey);
            }
            else
            {
                throw new NoAutorizadoExcepcion(MENSAJE_ERROR);
            }
        }
        catch (NoAutorizadoExcepcion ex)
        {
            context.Result = new ObjectResult(new
            {
                mensaje = ex.Message
            })
            {
                StatusCode = 401
            };
        }
    }

    private IApiKeyServicio ObtenerServicioApiKey(AuthorizationFilterContext context)
    {
        return context.HttpContext.RequestServices.GetService(typeof(IApiKeyServicio)) as IApiKeyServicio
               ?? throw new InvalidOperationException("No se pudo obtener IApiKeyServicio.");
    }
}
