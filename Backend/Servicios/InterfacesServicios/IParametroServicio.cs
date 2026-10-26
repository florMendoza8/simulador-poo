using Dominio;

namespace Servicios.InterfacesServicios;

public interface IParametroServicio
{
    Parametro Agregar(Parametro parametro);

    Parametro ObtenerParametroPorId(Guid parametroId);

    List<Parametro> ObtenerTodosParametros();
}
