namespace Dominio.Excepciones;

public class EntidadYaExisteExcepcion : Exception
{
    public EntidadYaExisteExcepcion(string mensaje)
        : base(mensaje)
    {
    }
}
