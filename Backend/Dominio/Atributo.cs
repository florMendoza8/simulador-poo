namespace Dominio;

public class Atributo
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string? Nombre { get; set; }
    public TipoDato? TipoReferencia { get; set; }
    public TipoDato? TipoInstancia { get; set; }
    public Accesibilidad Accesibilidad { get; set; }
    public bool EsEstatico { get; set; }

    public Atributo()
    {
    }

    public Atributo(string? nombre, TipoDato tipoReferencia, TipoDato? tipoInstancia, Accesibilidad accesibilidad, bool estatico)
    {
        Nombre = nombre;
        TipoReferencia = tipoReferencia;
        TipoInstancia = tipoInstancia;
        Accesibilidad = accesibilidad;
        EsEstatico = estatico;
    }
}
