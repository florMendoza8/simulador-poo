using System.ComponentModel.DataAnnotations;
using API.Filtros;
using API.Modelos.Interfaz;
using Dominio;
using Microsoft.AspNetCore.Mvc;
using Servicios.InterfacesServicios;

namespace API.Controladores;

[ApiController]
[Route("api/interfaces")]
public class InterfazControlador(IInterfazServicio interfazServicio) : ControllerBase
{
    [HttpPost]
    public CrearInterfazRespuesta Crear([FromBody, Required] CrearInterfazSolicitud solicitud)
    {
        var interfazAGuardar = new Interfaz(solicitud.Nombre, new List<Metodo>());
        interfazAGuardar = interfazServicio.CrearInterfaz(interfazAGuardar);

        return new CrearInterfazRespuesta(interfazAGuardar);
    }

    [HttpDelete("{interfazId}")]
    public IActionResult Eliminar(Guid interfazId)
    {
        var interfaz = interfazServicio.ObtenerInterfazPorId(interfazId);

        interfazServicio.EliminarInterfaz(interfaz);

        return NoContent();
    }

    [HttpGet("{interfazId}")]
    public ObtenerInterfazRespuesta Obtener(Guid interfazId)
    {
        var interfaz = interfazServicio.ObtenerInterfazPorId(interfazId);
        return new ObtenerInterfazRespuesta(interfaz);
    }

    [HttpGet]
    public List<ObtenerInterfazRespuesta> ObtenerTodas()
    {
        return interfazServicio.ObtenerTodasLasInterfaces()
            .Select(i => new ObtenerInterfazRespuesta(i))
            .ToList();
    }

    [HttpPost("{interfazId}/firmaMetodos")]
    public AgregarFirmaMetodoRespuesta AgregarFirmaMetodo([FromRoute] Guid interfazId, [FromBody, Required] AgregarFirmaMetodoSolicitud solicitud)
    {
        interfazServicio.AgregarMetodoAFirmaMetodos(interfazId, solicitud.FirmaMetodoId);

        return new AgregarFirmaMetodoRespuesta(interfazId, solicitud.FirmaMetodoId);
    }
}
