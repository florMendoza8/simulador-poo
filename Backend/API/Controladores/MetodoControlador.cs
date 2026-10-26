using System.ComponentModel.DataAnnotations;
using API.Filtros;
using API.Modelos.Metodo;
using Dominio;
using Microsoft.AspNetCore.Mvc;
using Servicios.InterfacesServicios;

namespace API.Controladores;

[ApiController]
[Route("api/metodos")]
public class MetodoControlador(IMetodoServicio metodoServicio,
    ITipoDatoServicio tipoDatoServicio) : ControllerBase
{
    [HttpPost]
    public CrearMetodoRespuesta Crear([FromBody, Required] CrearMetodoSolicitud solicitud)
    {
        var tipoDato = tipoDatoServicio.ObtenerTipoDatoPoId(solicitud.TipoRetornoId);

        var metodo = new Metodo(
            solicitud.Nombre!,
            tipoDato,
            solicitud.EsAbstracto,
            solicitud.EsSealed,
            solicitud.Accesibilidad,
            null,
            new List<Parametro>(),
            new List<VariableLocal>(),
            new List<Metodo>())
        {
            EsEstatico = solicitud.EsEstatico,
            EsVirtual = solicitud.EsVirtual,
            EsOverride = solicitud.EsOverride
        };

        var nuevoMetodo = metodoServicio.AgregarMetodo(metodo);
        return new CrearMetodoRespuesta(nuevoMetodo);
    }

    [HttpDelete("{metodoId}")]
    public IActionResult Eliminar(Guid metodoId)
    {
        var metodo = metodoServicio.ObtenerMetodoPorId(metodoId);

        metodoServicio.EliminarMetodo(metodo);

        return NoContent();
    }

    [HttpPost("{metodoId}/parametros")]
    public AgregarParametroMetodoRespuesta AgregarParametro([FromRoute] Guid metodoId, [FromBody, Required] AgregarParametroMetodoSolicitud solicitud)
    {
        metodoServicio.AgregarParametro(metodoId, solicitud.ParametroId);

        return new AgregarParametroMetodoRespuesta(metodoId, solicitud.ParametroId);
    }

    [HttpPost("{metodoId}/variablesLocales")]
    public AgregarVariableLocalMetodoRespuesta AgregarVariableLocal([FromRoute] Guid metodoId, [FromBody, Required] AgregarVariableLocalMetodoSolicitud solicitud)
    {
        metodoServicio.AgregarVariableLocal(metodoId, solicitud.VariableLocalId);

        return new AgregarVariableLocalMetodoRespuesta(metodoId, solicitud.VariableLocalId);
    }

    [HttpPost("{metodoId}/metodosInvocados")]
    public AgregarMetodoInvocadoMetodoRespuesta AgregarMetodoInvocado([FromRoute] Guid metodoId, [FromBody, Required] AgregarMetodoInvocadoMetodoSolicitud solicitud)
    {
        metodoServicio.AgregarMetodoInvocado(metodoId, solicitud.MetodoInvocadoId);

        return new AgregarMetodoInvocadoMetodoRespuesta(metodoId, solicitud.MetodoInvocadoId);
    }

    [HttpGet]
    public List<ObtenerMetodoRespuesta> ObtenerTodos()
    {
        return metodoServicio.ObtenerTodosMetodos()
            .Select(m => new ObtenerMetodoRespuesta(m))
            .ToList();
    }

    [HttpGet("{metodoId}")]
    public ActionResult<ObtenerMetodoRespuesta> ObtenerMetodo([FromRoute] Guid metodoId)
    {
        var metodo = metodoServicio.ObtenerMetodoPorId(metodoId);

        return new ObtenerMetodoRespuesta(metodo);
    }

    [HttpPost("firmas")]
    public CrearMetodoRespuesta CrearFirmaMetodo([FromBody, Required] CrearFirmaMetodoSolicitud solicitud)
    {
        var tipoDato = tipoDatoServicio.ObtenerTipoDatoPoId(solicitud.TipoRetornoId);

        var firmaMetodo = new Metodo(solicitud.Nombre!, tipoDato, new List<Parametro>());
        var nuevaFirmaMetodo = metodoServicio.AgregarFirmaMetodo(firmaMetodo);

        return new CrearMetodoRespuesta(nuevaFirmaMetodo);
    }

    [HttpGet("firmas")]
    public List<ObtenerFirmaMetodoRespuesta> ObtenerTodasFirmaMetodos()
    {
        return metodoServicio.ObtenerTodasFirmaMetodos()
            .Select(m => new ObtenerFirmaMetodoRespuesta(m))
            .ToList();
    }

    [HttpGet("{metodoId}/firma")]
    public ActionResult<ObtenerFirmaMetodoRespuesta> ObtenerFirmaMetodo([FromRoute] Guid metodoId)
    {
        var metodo = metodoServicio.ObtenerMetodoPorId(metodoId);

        return new ObtenerFirmaMetodoRespuesta(metodo);
    }
}
