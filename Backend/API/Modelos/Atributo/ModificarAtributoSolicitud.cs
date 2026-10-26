using System.Text.Json.Serialization;

namespace API.Modelos.Atributo;
using Dominio;

public class ModificarAtributoSolicitud
{
    public string? Nombre { get; set; }
    public Guid TipoReferenciaId { get; set; }
    public Guid TipoInstanciaId { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Accesibilidad Accesibilidad { get; set; }
    public bool EsEstatico { get; set; }
}
