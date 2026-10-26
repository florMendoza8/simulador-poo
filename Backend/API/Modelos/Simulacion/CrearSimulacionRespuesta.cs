using System.Text.Json.Nodes;

namespace API.Modelos.Simulacion;

public class CrearSimulacionRespuesta
{
    public string Resultado { get; set; }

    public CrearSimulacionRespuesta(string resultadoTransformado)
    {
        Resultado = resultadoTransformado;
    }
}
