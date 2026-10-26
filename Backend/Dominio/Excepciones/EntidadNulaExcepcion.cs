namespace Dominio.Excepciones;

public class EntidadNulaExcepcion : Exception
{
    public EntidadNulaExcepcion(string mensaje)
        : base(mensaje)
    {
    }
}
