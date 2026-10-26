using System.Text.Json.Serialization;

namespace API.Modelos.Metodo;
using Dominio;
public class CrearMetodoSolicitud
{
    public string? Nombre { get; set; }
    public Guid TipoRetornoId { get; set; }
    public bool EsAbstracto { get; set; }
    public bool EsSealed { get; set; }
    public bool EsEstatico { get; set; }
    public bool EsVirtual { get; set; }
    public bool EsOverride { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Accesibilidad Accesibilidad { get; set; }
}
