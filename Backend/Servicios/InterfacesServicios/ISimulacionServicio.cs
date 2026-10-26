using System.Text.Json.Nodes;
using Dominio;

namespace Servicios.InterfacesServicios;

public interface ISimulacionServicio
{
    public string? EjecutarSimulacionTransformada(Clase? tipoReferencia, Clase? tipoObjeto, Metodo metodoReferencia,
        string prefijo, string? nombreTransformador);
}
