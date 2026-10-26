using System.Text;
using System.Text.Json.Nodes;

namespace Reflection.TransformadorHtml;

public class TransformadorHtmlServicio : ITransformador
{
    public string Nombre => "HTML";

    public string Transformar(JsonObject simulacion)
    {
        var sb = new StringBuilder();
        sb.Append("<html><body>");

        var llamadaInicial = simulacion["LlamadaInicial"]?.ToString() ?? "Llamada desconocida";

        sb.Append("<ul>");
        sb.Append($"<li>{llamadaInicial}");

        if (simulacion["MetodosEjecutados"] is JsonArray subLlamadas && subLlamadas.Count > 0)
        {
            sb.Append(ProcesarLlamadas(subLlamadas));
        }

        sb.Append("</li>");
        sb.Append("</ul>");

        sb.Append("</body></html>");
        return sb.ToString();
    }

    private string ProcesarLlamadas(JsonArray llamadas)
    {
        var sb = new StringBuilder();
        sb.Append("<ul>");

        foreach (var llamada in llamadas)
        {
            if (llamada is JsonValue)
            {
                sb.Append($"<li>{llamada.ToString()}</li>");
            }
            else if (llamada is JsonObject llamadaObj)
            {
                var texto = llamadaObj["LlamadaInicial"]?.ToString() ?? "(sin texto)";
                sb.Append($"<li>{texto}");

                if (llamadaObj["MetodosEjecutados"] is JsonArray subLlamadas && subLlamadas.Count > 0)
                {
                    sb.Append(ProcesarLlamadas(subLlamadas));
                }

                sb.Append("</li>");
            }
        }

        sb.Append("</ul>");
        return sb.ToString();
    }
}
