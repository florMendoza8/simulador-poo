namespace Dominio.Excepciones;

public class NoAutorizadoExcepcion : Exception
{
    public NoAutorizadoExcepcion(string mensaje)
        : base(mensaje)
    {
    }
}
