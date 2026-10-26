using AccesoADatos.InterfacesRepositorios;
using Dominio;
using Dominio.Excepciones;
using Servicios.InterfacesServicios;

namespace Servicios;

public class ParametroServicio : IParametroServicio
{
    private readonly IParametroRepositorio _parametroRepositorio;

    public ParametroServicio(IParametroRepositorio parametroRepositorio)
    {
        _parametroRepositorio = parametroRepositorio;
    }

    public Parametro Agregar(Parametro parametro)
    {
        if (parametro == null)
        {
            throw new EntidadNulaExcepcion("Parametro no puede ser nulo");
        }

        if (string.IsNullOrWhiteSpace(parametro.Nombre))
        {
            throw new EntidadNulaExcepcion("Nombre no puede ser nuulo o vacío");
        }

        if (parametro.TipoDato == null)
        {
            throw new EntidadNulaExcepcion("TipoDato no puede ser nulo");
        }

        _parametroRepositorio.Agregar(parametro);

        return parametro;
    }

    public Parametro ObtenerParametroPorId(Guid parametroId)
    {
        if (parametroId == Guid.Empty)
        {
            throw new EntidadNulaExcepcion("El id del parametro no puede estar vacío.");
        }

        var parametro = _parametroRepositorio.Obtener(parametroId);
        if (parametro == null)
        {
            throw new EntidadNoEncontradaExcepcion($"No se encontro un parametro con id: {parametroId}");
        }

        return parametro;
    }

    public List<Parametro> ObtenerTodosParametros()
    {
        return _parametroRepositorio.ObtenerTodos();
    }
}
