namespace Dominio.Excepciones;

public class EntidadNoEncontradaExcepcion : Exception
{
    public EntidadNoEncontradaExcepcion(string mensaje)
        : base(mensaje)
    {
    }
}
