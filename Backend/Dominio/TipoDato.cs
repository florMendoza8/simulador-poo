using System;

namespace Dominio;

public class TipoDato
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string? Nombre { get; set; }
    public TipoDato? TipoPadre { get; set; }

    public TipoDato()
    {
    }

    public TipoDato(string nombre)
    {
        Nombre = nombre;
    }

    public TipoDato(string? nombre, TipoDato padre)
    {
        Nombre = nombre;
        TipoPadre = padre;
    }
}
