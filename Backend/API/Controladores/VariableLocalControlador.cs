using API.Modelos.VariableLocal;
using Dominio;
using Microsoft.AspNetCore.Mvc;
using Servicios.InterfacesServicios;

namespace API.Controladores;

[ApiController]
[Route("api/variableLocal")]
public class VariableLocalControlador(IVariableLocalServicio variableLocalServicio) : ControllerBase
{
    [HttpPost]
    public CrearVariableLocalRespuesta CrearVariable(CrearVariableSolicitud solicitud, ITipoDatoServicio tipoDatoServicio)
    {
        var tipoReferencia = tipoDatoServicio.ObtenerTipoDatoPoId(solicitud.TipoReferenciaId);
        var variableAGuardar = new VariableLocal(solicitud.Nombre!, tipoReferencia, null, solicitud.Valor!);
        variableAGuardar = variableLocalServicio.Agregar(variableAGuardar);
        return new CrearVariableLocalRespuesta(variableAGuardar);
    }

    [HttpGet]
    public List<ObtenerVariableLocalRespuesta> ObtenerTodas()
    {
        return variableLocalServicio.ObtenerTodasLasVariablesLocales()
            .Select(vl => new ObtenerVariableLocalRespuesta(vl))
            .ToList();
    }

    [HttpGet("{id}")]
    public ObtenerVariableLocalRespuesta ObtenerPorId(Guid id)
    {
        var variable = variableLocalServicio.ObtenerVariableLocalPorId(id);
        return new ObtenerVariableLocalRespuesta(variable);
    }
}
