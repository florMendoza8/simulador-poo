using API.Filtros;
using Dominio.Excepciones;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Moq;
using Servicios.InterfacesServicios;

namespace FiltrosTest;

[TestClass]
public class AutorizacionFiltroTests
{
    private const string HEADER = "X-API-KEY";
    private const string API_KEY_VALIDA = "9C0FF0B1-4ABD-45C6-8A4A-831748FB7A20";

    private AuthorizationFilterContext CrearContextoConHeaders(
        IDictionary<string, string>? headers = null,
        IApiKeyServicio? servicio = null)
    {
        var httpContext = new DefaultHttpContext();

        if (headers != null)
        {
            foreach (var h in headers)
            {
                httpContext.Request.Headers[h.Key] = h.Value;
            }
        }

        var serviceProvider = new Mock<IServiceProvider>();
        serviceProvider
            .Setup(s => s.GetService(typeof(IApiKeyServicio)))
            .Returns(servicio);
        httpContext.RequestServices = serviceProvider.Object;

        var actionContext = new ActionContext
        {
            HttpContext = httpContext,
            RouteData = new Microsoft.AspNetCore.Routing.RouteData(),
            ActionDescriptor = new Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor()
        };

        return new AuthorizationFilterContext(actionContext, new List<IFilterMetadata>());
    }

    [TestMethod]
    public void OnAuthorization_ConTipoApiKey_YClaveValida_NoLanzaExcepcion()
    {
        var mockServicio = new Mock<IApiKeyServicio>();
        mockServicio.Setup(s => s.EsClaveValida(API_KEY_VALIDA)).Returns(true);

        var filtro = new AutorizacionFiltro("apiKey");

        var context = CrearContextoConHeaders(
            new Dictionary<string, string> { { HEADER, API_KEY_VALIDA } },
            mockServicio.Object);

        filtro.OnAuthorization(context);
    }

    [TestMethod]
    public void OnAuthorization_ConTipoApiKey_YClaveInvalida_LanzaNoAutorizado()
    {
        var mockServicio = new Mock<IApiKeyServicio>();
        mockServicio
            .Setup(s => s.EsClaveValida(API_KEY_VALIDA))
            .Throws(new NoAutorizadoExcepcion("API Key inválida o ausente"));

        var filtro = new AutorizacionFiltro("apiKey");

        var context = CrearContextoConHeaders(
            new Dictionary<string, string> { { HEADER, API_KEY_VALIDA } },
            mockServicio.Object);

        filtro.OnAuthorization(context);
        Assert.IsInstanceOfType(context.Result, typeof(ObjectResult));
        Assert.AreEqual(401, (context.Result as ObjectResult)?.StatusCode);
    }

    [TestMethod]
    public void OnAuthorization_ConTipoDistintoApiKey_LanzaNoAutorizado()
    {
        var filtro = new AutorizacionFiltro("otroTipo");

        var context = CrearContextoConHeaders(
            new Dictionary<string, string> { { HEADER, API_KEY_VALIDA } },
            servicio: null);

        filtro.OnAuthorization(context);
        Assert.IsInstanceOfType(context.Result, typeof(ObjectResult));
        Assert.AreEqual(401, (context.Result as ObjectResult)?.StatusCode);
    }

    [TestMethod]
    public void OnAuthorization_TipoIncorrecto_IgualaApiKeyPeroNoEjecutaValidacion()
    {
        var filtro = new AutorizacionFiltro("otro");

        var context = CrearContextoConHeaders();

        filtro.OnAuthorization(context);
        Assert.IsInstanceOfType(context.Result, typeof(ObjectResult));
        Assert.AreEqual(401, (context.Result as ObjectResult)?.StatusCode);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void OnAuthorization_CuandoNoHayServicio_LanzaInvalidOperation()
    {
        var filtro = new AutorizacionFiltro("apiKey");

        var context = CrearContextoConHeaders(
            new Dictionary<string, string> { { HEADER, API_KEY_VALIDA } },
            servicio: null);

        filtro.OnAuthorization(context);
    }
}
