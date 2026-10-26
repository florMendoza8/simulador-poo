using Dominio;

namespace Servicios.InterfacesServicios;

public interface IVariableLocalServicio
{
    VariableLocal Agregar(VariableLocal variable);

    VariableLocal ObtenerVariableLocalPorId(Guid variableLocalId);
    List<VariableLocal> ObtenerTodasLasVariablesLocales();
}
