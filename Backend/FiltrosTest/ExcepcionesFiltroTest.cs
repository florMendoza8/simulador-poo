using API.Filtros;
using Dominio.Excepciones;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FiltrosTest;

[TestClass]
public class ExcepcionesFiltroTest
{
    private ExcepcionesFiltro? _filtro;
    private ActionContext? _actionContext;
    private DefaultHttpContext? _httpContext;
    private Microsoft.AspNetCore.Routing.RouteData? _routeData;
    private Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor? _actionDescriptor;

    [TestInitialize]
    public void Inicializar()
    {
        _filtro = new ExcepcionesFiltro();
        _httpContext = new DefaultHttpContext();
        _routeData = new Microsoft.AspNetCore.Routing.RouteData();
        _actionDescriptor = new Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor();
        _actionContext = new ActionContext(_httpContext, _routeData, _actionDescriptor);
    }

    [TestMethod]
    public void OnException_CuandoExcepcionGenerica_RetornaResultadoConStatus500YMensajeEsperado()
    {
        var context = new ExceptionContext(_actionContext!, new List<IFilterMetadata>())
        {
            Exception = new Exception("Error inesperado")
        };

        _filtro!.OnException(context);

        Assert.IsTrue(context.Result is ObjectResult resultado &&
                      resultado.Value is RespuestaDto dto &&
                      dto.Mensaje == "Error Interno del Servidor");
    }

    [TestMethod]
    public void OnException_CuandoEntidadNulaExcepcion_Retorna400()
    {
        var context = new ExceptionContext(_actionContext!, new List<IFilterMetadata>())
        {
            Exception = new EntidadNulaExcepcion("El atributo no puede ser nulo o un espacio en blanco.")
        };

        _filtro!.OnException(context);

        Assert.IsTrue(
            context.Result is ObjectResult r &&
            r.Value is RespuestaDto dto &&
            dto.Mensaje == "El atributo no puede ser nulo o un espacio en blanco.");
    }

    [TestMethod]
    public void OnException_CuandoEntidadYaExisteExcepcion_Retorna409()
    {
        var context = new ExceptionContext(_actionContext!, new List<IFilterMetadata>())
        {
            Exception = new EntidadYaExisteExcepcion("Ya existe el tipo de dato con ese nombre")
        };

        _filtro!.OnException(context);

        Assert.IsTrue(
            context.Result is ObjectResult r &&
            r.Value is RespuestaDto dto &&
            dto.Mensaje == "Ya existe el tipo de dato con ese nombre");
    }

    [TestMethod]
    public void OnException_CuandoEntidadNoEncontradaExcepcion_Retorna404()
    {
        var context = new ExceptionContext(_actionContext!, new List<IFilterMetadata>())
        {
            Exception = new EntidadNoEncontradaExcepcion("No se encontró el atributo solicitado.")
        };

        _filtro!.OnException(context);

        Assert.IsTrue(
            context.Result is ObjectResult r &&
            r.Value is RespuestaDto dto &&
            dto.Mensaje == "No se encontró el atributo solicitado.");
    }

    [TestMethod]
    public void OnException_CuandoOperacionNoPermitida_Retorna403()
    {
        var context = new ExceptionContext(_actionContext!, new List<IFilterMetadata>())
        {
            Exception = new OperacionNoPermitidaExcepcion("No se puede modificar una clase sealed.")
        };

        _filtro!.OnException(context);

        Assert.IsTrue(
            context.Result is ObjectResult r &&
            r.Value is RespuestaDto dto &&
            dto.Mensaje == "No se puede modificar una clase sealed.");
    }

    [TestMethod]
    public void OnException_CuandoNoSeEstaAutorizado_Retorna401()
    {
        var context = new ExceptionContext(_actionContext!, new List<IFilterMetadata>())
        {
            Exception = new NoAutorizadoExcepcion("API Key invalida o ausente.")
        };

        _filtro!.OnException(context);

        Assert.IsTrue(
            context.Result is ObjectResult r &&
            r.Value is RespuestaDto dto &&
            dto.Mensaje == "API Key invalida o ausente.");
    }
}
