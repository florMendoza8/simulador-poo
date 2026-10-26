using API.Modelos.Parametro;
using Dominio;
using Microsoft.AspNetCore.Mvc;
using Servicios.InterfacesServicios;

namespace API.Controladores;

[ApiController]
[Route("api/parametros")]
public class ParametroControlador(IParametroServicio parametroServicio, ITipoDatoServicio tipoDatoServicio)
{
    [HttpPost]
    public CrearParametroRespuesta Crear(CrearParametroSolicitud solicitud)
    {
        var tipoDato = tipoDatoServicio.ObtenerTipoDatoPoId(solicitud.TipoDatoId);
        var parametroAGuardar = new Parametro(solicitud.Nombre!, tipoDato);
        parametroAGuardar = parametroServicio.Agregar(parametroAGuardar);

        return new CrearParametroRespuesta(parametroAGuardar);
    }

    [HttpGet]
    public List<ObtenerParametroRespuesta> ObtenerTodos()
    {
        return parametroServicio.ObtenerTodosParametros()
            .Select(m => new ObtenerParametroRespuesta(m))
            .ToList();
    }

    [HttpGet("{parametroId}")]
    public ActionResult<ObtenerParametroRespuesta> ObtenerParametro([FromRoute] Guid parametroId)
    {
        var parametro = parametroServicio.ObtenerParametroPorId(parametroId);

        return new ObtenerParametroRespuesta(parametro);
    }
}
