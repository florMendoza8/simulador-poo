using API.Modelos.Namespace;
using Dominio;
using Microsoft.AspNetCore.Mvc;
using Servicios.InterfacesServicios;

namespace API.Controladores;

[ApiController]
[Route("api/namespace")]
public class NamespaceControlador(INamespaceServicio namespaceServicio) : ControllerBase
{
    [HttpPost]
    public CrearNamespaceRespuesta Crear(CrearNamespaceSolicitud? solicitud)
    {
        var nameSpace = new Namespace(solicitud.Nombre, solicitud.Padre);
        var ns = namespaceServicio.Crear(nameSpace);
        return new CrearNamespaceRespuesta(ns);
    }

    [HttpPut("{namespaceId}/renombrar")]
    public IActionResult Renombrar(Guid namespaceId, [FromBody] string nuevoNombre)
    {
        namespaceServicio.Renombrar(namespaceId, nuevoNombre);
        return NoContent();
    }

    [HttpPut("{namespaceId}/cambiarPadre")]
    public IActionResult CambiarPadre(Guid namespaceId, [FromBody] Guid? nuevoPadreId)
    {
        namespaceServicio.CambiarPadre(namespaceId, nuevoPadreId);
        return NoContent();
    }

    [HttpDelete("{namespaceId}")]
    public IActionResult Eliminar(Guid namespaceId)
    {
        namespaceServicio.Eliminar(namespaceId);
        return NoContent();
    }

    [HttpGet]
    public List<ObtenerNamespaceRespuesta> Listar()
    {
        return namespaceServicio.ListarTodos().Select(lis => new ObtenerNamespaceRespuesta(lis)).ToList();
    }

    [HttpGet("{namespaceId}")]
    public ObtenerNamespaceRespuesta Obtener(Guid namespaceId)
    {
        var nameSaPe = namespaceServicio.Obtener(namespaceId);
        return new ObtenerNamespaceRespuesta(nameSaPe);
    }

    /*
    [HttpPost("{namespaceId}/elemento")]
    public IActionResult AgregarElemento(Guid namespaceId, [FromBody] AgregarElementoSolicitud solicitud)
    {
        namespaceServicio.AgregarElemento(namespaceId, solicitud.ToElemento());
        return NoContent();
    }*/

    [HttpDelete("{namespaceId}/elemento/{elementoId}")]
    public IActionResult EliminarElemento(Guid namespaceId, Guid elementoId)
    {
        namespaceServicio.EliminarElemento(namespaceId, elementoId);
        return NoContent();
    }

    [HttpGet("{namespaceId}/hijos")]
    public IActionResult ObtenerHijos(Guid namespaceId)
    {
        var hijos = namespaceServicio.ObtenerHijos(namespaceId);
        return Ok(hijos);
    }

    [HttpPost("{namespaceId:guid}/clase")]
    public IActionResult AgregarClase(Guid namespaceId, [FromBody] Clase? clase)
    {
        namespaceServicio.AgregarClase(namespaceId, clase);
        return NoContent();
    }

    [HttpPost("{namespaceId:guid}/interfaz")]
    public IActionResult AgregarInterfaz(Guid namespaceId, [FromBody] Interfaz interfaz)
    {
        namespaceServicio.AgregarInterfaz(namespaceId, interfaz);
        return NoContent();
    }
}
