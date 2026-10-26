using AccesoADatos.InterfacesRepositorios;
using Dominio;
using Microsoft.EntityFrameworkCore;

namespace AccesoADatos.Repositorios;

public class ParametroRepositorio : IParametroRepositorio
{
    private readonly DbContext _dbContext;

    public ParametroRepositorio(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Agregar(Parametro parametro)
    {
        _dbContext.Set<Parametro>().Add(parametro);
        _dbContext.SaveChanges();
    }

    public bool Existe(string nombre, TipoDato tipoDato)
    {
        return _dbContext.Set<Parametro>().Any(p => p.Nombre == nombre && p.TipoDato == tipoDato);
    }

    public Parametro Obtener(Guid id)
    {
        return _dbContext.Set<Parametro>()
            .Include(p => p.TipoDato)
            .FirstOrDefault(p => p.Id == id);
    }

    public List<Parametro> ObtenerTodos()
    {
        return _dbContext.Set<Parametro>()
            .Include(p => p.TipoDato)
            .ToList();
    }
}
