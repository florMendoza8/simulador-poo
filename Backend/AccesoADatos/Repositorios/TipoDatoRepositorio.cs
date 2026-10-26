using AccesoADatos.InterfacesRepositorios;
using Dominio;
using Microsoft.EntityFrameworkCore;

namespace AccesoADatos.Repositorios;

public class TipoDatoRepositorio : ITipoDatoRepositorio
{
    private readonly DbContext _dbContext;

    public TipoDatoRepositorio(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Agregar(TipoDato tipoDato)
    {
        _dbContext.Set<TipoDato>().Add(tipoDato);
        _dbContext.SaveChanges();
    }

    public bool Existe(string? nombre)
    {
        return _dbContext.Set<TipoDato>().Any(td => td.Nombre == nombre);
    }

    public TipoDato Obtener(Guid id)
    {
        return _dbContext.Set<TipoDato>().FirstOrDefault(td => td.Id == id);
    }

    public List<TipoDato> ObtenerTodos()
    {
        return _dbContext.Set<TipoDato>().ToList();
    }

    public void EliminarTipoDato(TipoDato tipoDato)
    {
        var tipoAEliminar = _dbContext.Set<TipoDato>().FirstOrDefault(t => t.Id == tipoDato.Id);
        if(tipoAEliminar != null)
        {
            _dbContext.Set<TipoDato>().Remove(tipoAEliminar);
            _dbContext.SaveChanges();
        }
    }
}
