using AccesoADatos.InterfacesRepositorios;
using Dominio;
using Dominio.Excepciones;
using Servicios.InterfacesServicios;

namespace Servicios;

public class VariableLocalServicio : IVariableLocalServicio
{
    private readonly IVariableLocalRepositorio _repositorio;
    private readonly ITipoDatoServicio _tipoDatoServicio;

    public VariableLocalServicio(IVariableLocalRepositorio repositorio, ITipoDatoServicio tipoDato)
    {
        _repositorio = repositorio;
        _tipoDatoServicio = tipoDato;
    }

    public VariableLocal Agregar(VariableLocal variable)
    {
        if(variable == null)
        {
            throw new EntidadNulaExcepcion("La variable local no puede ser nula");
        }

        VerificacionNombre(variable.Nombre!);
        VerificacionTipoDato(variable.TipoReferencia);

        if(variable.TipoInstancia != null)
        {
            VerificacionTipoDato(variable.TipoInstancia);
            if(!_tipoDatoServicio.EsCompatible(variable.TipoReferencia.Id, variable.TipoInstancia.Id))
            {
                throw new OperacionNoPermitidaExcepcion(
                    $"No puedes asignar el tipo instancia {variable.TipoInstancia} a la referencia {variable.TipoReferencia}.");
            }
        }

        _repositorio.Agregar(variable);
        return variable;
    }

    public VariableLocal ObtenerVariableLocalPorId(Guid variableLocalId)
    {
        if (variableLocalId == Guid.Empty)
        {
            throw new EntidadNulaExcepcion("El id de la variableLocal no puede estar vacío.");
        }

        var variable = _repositorio.ObtenerPorId(variableLocalId);
        if (variable == null)
        {
            throw new EntidadNoEncontradaExcepcion($"No se encontro una variableLocal con id: {variableLocalId}");
        }

        return variable;
    }

    public List<VariableLocal> ObtenerTodasLasVariablesLocales()
    {
        return _repositorio.ObtenerTodos();
    }

    private void VerificacionNombre(string nombre)
    {
        if (string.IsNullOrEmpty(nombre))
        {
            throw new EntidadNulaExcepcion("El nombre no puede ser nulo ni vacío.");
        }
    }

    private void VerificacionTipoDato(TipoDato? variable)
    {
        if(variable == null)
        {
            throw new EntidadNulaExcepcion("El tipo de dato no puede ser nulo.");
        }

        if(variable.Id == Guid.Empty)
        {
            throw new EntidadNulaExcepcion("El tipo de dato no puede ser nulo.");
        }
    }
}
