using AccesoADatos.InterfacesRepositorios;
using Dominio;
using Microsoft.EntityFrameworkCore;

namespace AccesoADatos.Repositorios;

public class AtributoRepositorio : IAtributoRepositorio
{
    private readonly DbContext _dbContext;

    public AtributoRepositorio(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Agregar(Atributo atributo)
    {
        _dbContext.Set<Atributo>().Add(atributo);
        _dbContext.SaveChanges();
    }

    public void Actualizar(Atributo atributo)
    {
        _dbContext.Set<Atributo>().Update(atributo);
        _dbContext.SaveChanges();
    }

    public void Eliminar(Atributo atributo)
    {
        var atributoAEliminar = _dbContext.Set<Atributo>().FirstOrDefault(a => a.Id == atributo.Id);
        if (atributoAEliminar != null)
        {
            _dbContext.Set<Atributo>().Remove(atributoAEliminar);
            _dbContext.SaveChanges();
        }
    }

    public bool Existe(string? nombre, TipoDato tipoDato, Accesibilidad accesibilidad)
    {
        return _dbContext.Set<Atributo>().Any(a => a.Nombre == nombre && a.TipoReferencia == tipoDato && a.Accesibilidad == accesibilidad);
    }

    public Atributo? Obtener(Guid id)
    {
        return _dbContext.Set<Atributo>().Include(a => a.TipoReferencia).FirstOrDefault(a => a.Id == id);
    }

    public List<Atributo> ObtenerTodos()
    {
        return _dbContext.Set<Atributo>().Include(a => a.TipoReferencia).ToList();
    }
}
