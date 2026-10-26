using API.Modelos.TipoDato;
using Dominio;
using Microsoft.AspNetCore.Mvc;
using Servicios.InterfacesServicios;

namespace API.Controladores;

[ApiController]
[Route("api/tipoDatos")]
public class TipoDatoControlador(ITipoDatoServicio tipoDatoServicio) : ControllerBase
{
    [HttpPost]
    public CrearTipoDatoRespuesta Crear(CrearTipoDatoSolicitud solicitud)
    {
        var tipoPadre = tipoDatoServicio.ObtenerTipoDatoPoId(solicitud.TipoPadreId);
        var tipoDatoAGuaradar = new TipoDato(solicitud.Nombre, tipoPadre);
        tipoDatoAGuaradar = tipoDatoServicio.CrearTipoDato(tipoDatoAGuaradar);

        return new CrearTipoDatoRespuesta(tipoDatoAGuaradar);
    }

    [HttpGet("{tipoDatoId}")]
    public ObtenerTipoDatoRespuesta Obtener(Guid tipoDatoId)
    {
        var tipoDato = tipoDatoServicio.ObtenerTipoDatoPoId(tipoDatoId);
        return new ObtenerTipoDatoRespuesta(tipoDato);
    }

    [HttpGet]
    public List<ObtenerTipoDatoRespuesta> ObtenerTodos()
    {
        return tipoDatoServicio.ObtenerTodosLosTipoDatos()
            .Select(td => new ObtenerTipoDatoRespuesta(td))
            .ToList();
    }
}
