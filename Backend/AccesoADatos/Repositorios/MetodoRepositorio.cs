using AccesoADatos.InterfacesRepositorios;
using Dominio;
using Microsoft.EntityFrameworkCore;

namespace AccesoADatos.Repositorios;

public class MetodoRepositorio : IMetodoRepositorio
{
    private readonly DbContext _dbContext;

    public MetodoRepositorio(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Agregar(Metodo metodo)
    {
        _dbContext.Set<Metodo>().Add(metodo);
        _dbContext.SaveChanges();
    }

    public bool Existe(string nombre)
    {
        return _dbContext.Set<Metodo>().Any(m => m.Nombre == nombre);
    }

    public Metodo Obtener(Guid id)
    {
        return _dbContext.Set<Metodo>()
            .Include(m => m.TipoRetorno)
            .Include(m => m.Parametros)
            .Include(m => m.VariablesLocales)
            .Include(m => m.MetodosInvocados)
            .FirstOrDefault(m => m.Id == id);
    }

    public List<Metodo> ObtenerTodos()
    {
        return _dbContext.Set<Metodo>()
            .Include(m => m.TipoRetorno)
            .Include(m => m.Parametros)
            .Include(m => m.VariablesLocales)
            .Include(m => m.MetodosInvocados)
            .ToList();
    }

    public List<Metodo> ObtenerMetodosInvocados(Guid metodoId)
    {
        var metodoConInvocados = _dbContext.Set<Metodo>()
            .Include(m => m.MetodosInvocados)
            .FirstOrDefault(m => m.Id == metodoId);

        return metodoConInvocados?.MetodosInvocados ?? new List<Metodo>();
    }

    public void Actualizar(Metodo metodo)
    {
        _dbContext.Set<Metodo>().Update(metodo);
        _dbContext.SaveChanges();
    }

    public void Eliminar(Metodo metodo)
    {
        var metodoAEliminar = _dbContext.Set<Metodo>().FirstOrDefault(m => m.Id == metodo.Id);
        if (metodoAEliminar != null)
        {
            _dbContext.Set<Metodo>().Remove(metodoAEliminar);
            _dbContext.SaveChanges();
        }
    }
}
