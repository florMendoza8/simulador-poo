using System.ComponentModel.DataAnnotations;
using API.Modelos.Atributo;
using Dominio;
using Dominio.Excepciones;
using Microsoft.AspNetCore.Mvc;
using Servicios.InterfacesServicios;

namespace API.Controladores;

[ApiController]
[Route("api/atributos")]
public class AtributoControlador(IAtributoServicio atributoServicio) : ControllerBase
{
    [HttpPost]
    public CrearAtributoRespuesta CrearAtributo(CrearAtributoSolicitud solicitud, [FromServices] ITipoDatoServicio tipoDatoServicio)
    {
        var tipoReferencia = tipoDatoServicio.ObtenerTipoDatoPoId(solicitud.TipoReferenciaId);
        var atributoAGuardar = new Atributo(solicitud.Nombre, tipoReferencia, tipoReferencia, solicitud.Accesibilidad, solicitud.EsEstatico);
        atributoAGuardar = atributoServicio.CrearAtributo(atributoAGuardar);

        return new CrearAtributoRespuesta(atributoAGuardar);
    }

    [HttpPut("{atributoId}")]
    public void ModificarAtributo(Guid atributoId, ModificarAtributoSolicitud solicitud, [FromServices] ITipoDatoServicio tipoDatoServicio)
    {
        var atributo = atributoServicio.ObtenerAtributoPorId(atributoId);
        var tipoReferencia = tipoDatoServicio.ObtenerTipoDatoPoId(solicitud.TipoReferenciaId);

        atributo.Nombre = solicitud.Nombre;
        atributo.TipoReferencia = tipoReferencia;
        atributo.TipoInstancia = tipoReferencia;
        atributo.Accesibilidad = solicitud.Accesibilidad;
        atributo.EsEstatico = solicitud.EsEstatico;

        atributoServicio.ModificarAtributo(atributo);
    }

    [HttpDelete("{atributoId}")]
    public void EliminarAtributo(Guid atributoId)
    {
        var atributo = atributoServicio.ObtenerAtributoPorId(atributoId);
        atributoServicio.EliminarAtributo(atributo);
    }

    [HttpGet("{atributoId}")]
    public ObtenerAtributoRespuesta ObtenerAtributoPorId(Guid atributoId)
    {
        var atributo = atributoServicio.ObtenerAtributoPorId(atributoId);
        return new ObtenerAtributoRespuesta(atributo);
    }

    [HttpGet]
    public List<ObtenerAtributoRespuesta> ObtenerTodos()
    {
        return atributoServicio.ObtenerTodosLosAtributos()
            .Select(td => new ObtenerAtributoRespuesta(td))
            .ToList();
    }
}
