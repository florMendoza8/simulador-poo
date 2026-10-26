using AccesoADatos.InterfacesRepositorios;
using Dominio;
using Microsoft.EntityFrameworkCore;

namespace AccesoADatos.Repositorios;

public class VariableLocalRepositorio : IVariableLocalRepositorio
{
    private readonly DbContext _contexto;

    public VariableLocalRepositorio(DbContext contexto)
    {
        _contexto = contexto;
    }

    public void Agregar(VariableLocal variables)
    {
        _contexto.Set<VariableLocal>().Add(variables);
        _contexto.SaveChanges();
    }

    public bool ExisteVariable(string? nombre)
    {
        return _contexto.Set<VariableLocal>().Any(u => u.Nombre == nombre);
    }

    public VariableLocal? ObtenerPorId(Guid? variableLocalId)
    {
        return _contexto.Set<VariableLocal>().Include(v => v.TipoReferencia).FirstOrDefault(u => u.Id == variableLocalId);
    }

    public List<VariableLocal> ObtenerTodos()
    {
        return _contexto.Set<VariableLocal>()
            .Include(v => v.TipoReferencia)
            .ToList();
    }
}
