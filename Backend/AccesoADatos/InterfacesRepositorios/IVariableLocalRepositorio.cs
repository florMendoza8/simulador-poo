using Dominio;

namespace AccesoADatos.InterfacesRepositorios;

public interface IVariableLocalRepositorio
{
    void Agregar(VariableLocal variables);
    VariableLocal ObtenerPorId(Guid? variableLocalId);
    List<VariableLocal> ObtenerTodos();
}
