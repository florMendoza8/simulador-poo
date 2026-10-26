using System.Text.Json.Nodes;

namespace Reflection;

public interface ITransformador
{
    string Nombre { get; }
    string Transformar(JsonObject simulacion);
}
