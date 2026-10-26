namespace Dominio.Excepciones;

public class OperacionNoPermitidaExcepcion : Exception
{
    public OperacionNoPermitidaExcepcion(string mensaje)
        : base(mensaje)
    {
    }
}
