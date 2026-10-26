using API.Modelos.Clase;
using Dominio;
using Microsoft.AspNetCore.Mvc;
using Servicios.InterfacesServicios;

namespace API.Controladores;

[ApiController]
[Route("api/clases")]
public class ClaseControlador(IClaseServicio claseServicio) : ControllerBase
{
    [HttpPost]
    public CrearClaseRespuesta Crear(CrearClaseSolicitud? solicitud)
    {
        var nuevaClase = new Clase
        {
            Nombre = solicitud.Nombre,
            EsAbstract = solicitud.EsAbstract,
            EsSealed = solicitud.EsSealed,
            ClaseBaseId = solicitud.ClaseBaseId,
        };
        var claseGuardada = claseServicio.CrearClase(nuevaClase);
        return new CrearClaseRespuesta(claseGuardada);
    }

    [HttpGet("{claseId}")]
    public ObtenerClaseRespuesta Obtener(Guid claseId)
    {
        var clase = claseServicio.ObtenerClasePorId(claseId);
        return new ObtenerClaseRespuesta(clase);
    }

    [HttpGet]
    public List<ObtenerClaseRespuesta> ObtenerTodas()
    {
        return claseServicio.ObtenerTodasLasClases().Select(lis => new ObtenerClaseRespuesta(lis)).ToList();
    }

    [HttpDelete("{claseId}")]
    public void Eliminar(Guid claseId)
    {
        var clase = claseServicio.ObtenerClasePorId(claseId);
        claseServicio.EliminarClase(clase);
    }

    [HttpPut("{claseId}")]
    public IActionResult Modificar([FromRoute] Guid claseId, [FromBody] ModificarClaseSolicitud solicitud)
    {
        var clase = claseServicio.ObtenerClasePorId(claseId);

        clase.Nombre = solicitud.Nombre;
        clase.EsAbstract = solicitud.EsAbstract;
        clase.EsSealed = solicitud.EsSealed;
        clase.ClaseBaseId = solicitud.ClaseBaseId;

        claseServicio.ModificarClase(clase);
        return NoContent();
    }

    [HttpPost("{claseId}/metodo")]
    public AgregarMetodoRespuesta AgregarMetodo(Guid claseId, AgregarMetodoSolicitud solicitud)
    {
        claseServicio.AgregarMetodo(claseId, solicitud.MetodoId);
        return new AgregarMetodoRespuesta(claseId, solicitud.MetodoId);
    }

    [HttpPost("{claseId}/atributo")]
    public AgregarAtributoRespuesta AgregarAtributo(Guid claseId, AgregarAtributoSolicitud solicitud)
    {
        claseServicio.AgregarAtributo(claseId, solicitud.AtributoId);
        return new AgregarAtributoRespuesta(claseId, solicitud.AtributoId);
    }

    [HttpDelete("{claseId}/atributo/{atributoId}")]
    public IActionResult QuitarAtributo(Guid claseId, Guid atributoId)
    {
        claseServicio.QuitarAtributo(claseId, atributoId);
        return NoContent();
    }

    [HttpDelete("{claseId}/metodo/{metodoId}")]
    public IActionResult QuitarMetodo(Guid claseId, Guid metodoId)
    {
        claseServicio.QuitarMetodo(claseId, metodoId);
        return NoContent();
    }
}
