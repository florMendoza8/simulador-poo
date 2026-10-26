using API.Filtros;
using API.Modelos.Simulacion;
using Dominio;
using Microsoft.AspNetCore.Mvc;
using Reflection;
using Reflection.InterfacesReflection;
using Servicios.InterfacesServicios;

namespace API.Controladores;

[ApiController]
[Route("api/simulaciones")]
public class SimulacionControlador(ISimulacionServicio simulacionServicio, IClaseServicio claseServicio,
    IMetodoServicio metodoServicio, ICargadorDePlugins<ITransformador> cargadorDePlugins) : ControllerBase
{
    [HttpPost]
    [AutorizacionFiltro("apiKey")]
    public CrearSimulacionRespuesta Crear(CrearSimulacionSolicitud solicitud)
    {
        var tipoReferencia = claseServicio.ObtenerClasePorId(solicitud.TipoReferenciaId);
        var tipoObjeto = claseServicio.ObtenerClasePorId(solicitud.TipoObjetoId);
        var metodoReferencia = metodoServicio.ObtenerMetodoPorId(solicitud.MetodoReferenciaId);

        var respuesta = simulacionServicio.EjecutarSimulacionTransformada(
            tipoReferencia, tipoObjeto, metodoReferencia, tipoObjeto.Nombre!, solicitud.Transformador);

        return new CrearSimulacionRespuesta(respuesta!);
    }

    [HttpGet("transformadores")]
    public List<string> ObtenerTransformadores()
    {
        return cargadorDePlugins.ObtenerImplementaciones();
    }
}
