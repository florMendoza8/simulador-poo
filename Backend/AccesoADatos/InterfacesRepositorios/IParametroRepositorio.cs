using Dominio;

namespace AccesoADatos.InterfacesRepositorios;

public interface IParametroRepositorio
{
    void Agregar(Parametro parametro);

    bool Existe(string nombre, TipoDato tipoDato);

    Parametro Obtener(Guid id);

    List<Parametro> ObtenerTodos();
}
