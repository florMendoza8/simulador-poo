namespace API.Modelos.Atributo;
using Dominio;

public class ObtenerAtributoRespuesta
{
    public Guid Id { get; set; }
    public string? Nombre { get; set; }
    public TipoDato? TipoReferencia { get; set; }
    public TipoDato? TipoInstancia { get; set; }
    public string Accesibilidad { get; set; }
    public bool EsEstatico { get; set; }

    public ObtenerAtributoRespuesta(Atributo atributo)
    {
        Id = atributo.Id;
        Nombre = atributo.Nombre;
        TipoReferencia = atributo.TipoReferencia;
        TipoInstancia = atributo.TipoInstancia;
        Accesibilidad = atributo.Accesibilidad.ToString();
        EsEstatico = atributo.EsEstatico;
    }
}
