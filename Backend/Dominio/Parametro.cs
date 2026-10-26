namespace Dominio;

public class Parametro
{
    public Guid Id { get; set; } = Guid.NewGuid();
    private string? _nombre;
    private TipoDato? _tipoDato;

    public Parametro()
    {
    }

    public Parametro(string nombre, TipoDato tipoDato)
    {
        Nombre = nombre;
        TipoDato = tipoDato;
    }

    public string? Nombre { get => _nombre; set => _nombre = value; }

    public TipoDato? TipoDato { get => _tipoDato; set => _tipoDato = value; }
}
