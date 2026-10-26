namespace Dominio;
public class VariableLocal
{
    public Guid Id { get; set; } = Guid.NewGuid();
    private string? _nombre;
    private TipoDato? _tipoReferencia;
    private TipoDato? _tipoInstancia;
    private string? _valor;

    public VariableLocal(string nombre, TipoDato tipoReferencia, TipoDato? tipoInstancia, string valor)
    {
        Nombre = nombre;
        TipoReferencia = tipoReferencia;
        TipoInstancia = tipoInstancia;
        Valor = valor;
    }

    public VariableLocal()
    {
    }

    public string? Nombre { get => _nombre; set => _nombre = value; }

    public TipoDato TipoReferencia { get => _tipoReferencia; set => _tipoReferencia = value; }
    public string? Valor { get => _valor; set => _valor = value!; }
    public TipoDato? TipoInstancia { get => _tipoInstancia; set => _tipoInstancia = value; }
}
