using AccesoADatos.InterfacesRepositorios;
using Dominio;
using Microsoft.EntityFrameworkCore;

namespace AccesoADatos.Repositorios;

public class InterfazRepositorio : IInterfazRepositorio
{
    private readonly DbContext _dbContext;

    public InterfazRepositorio(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Agregar(Interfaz interfaz)
    {
        _dbContext.Set<Interfaz>().Add(interfaz);
        _dbContext.SaveChanges();
    }

    public void Eliminar(Interfaz interfaz)
    {
        var interfazAEliminar = _dbContext.Set<Interfaz>().FirstOrDefault(i => i.Id == interfaz.Id);
        if (interfazAEliminar != null)
        {
            _dbContext.Set<Interfaz>().Remove(interfazAEliminar);
            _dbContext.SaveChanges();
        }
    }

    public void Actualizar(Interfaz interfaz)
    {
        _dbContext.Set<Interfaz>().Update(interfaz);
        _dbContext.SaveChanges();
    }

    public Interfaz? Obtener(Guid id)
    {
        return _dbContext.Set<Interfaz>().Include(i => i.FirmaMetodos).FirstOrDefault(i => i.Id == id);
    }

    public List<Interfaz> ObtenerTodos()
    {
        return _dbContext.Set<Interfaz>().Include(i => i.FirmaMetodos).ToList();
    }
}
