namespace Dominio.Excepciones;

public class NoImplementadoExcepcion : Exception
{
    public NoImplementadoExcepcion(string mensaje)
        : base(mensaje)
    {
    }
}
