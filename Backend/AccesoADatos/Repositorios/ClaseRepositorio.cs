using AccesoADatos.InterfacesRepositorios;
using Dominio;
using Microsoft.EntityFrameworkCore;

namespace AccesoADatos.Repositorios;

public class ClaseRepositorio : IClaseRepositorio
{
    private readonly DbContext _contexto;

    public ClaseRepositorio(DbContext contexto)
    {
        _contexto = contexto;
    }

    public void Agregar(Clase clase)
    {
        _contexto.Set<Clase>().Add(clase);
        _contexto.SaveChanges();
    }

    public bool Existe(Clase? clase)
    {
        return _contexto.Set<Clase>().Any(c =>
            c.Id == clase.Id && c.Nombre == clase.Nombre && c.EsAbstract == clase.EsAbstract &&
            c.EsSealed == clase.EsSealed);
    }

    public Clase? ObtenerClase(Guid? id)
    {
        return _contexto.Set<Clase>()
            .Include(c => c.MetodosInvocados)
            .Include(c => c.Atributos)
            .Include(c => c.ClaseBase)
            .ThenInclude(cb => cb.MetodosInvocados)
            .FirstOrDefault(c => c.Id == id);
    }

    public List<Clase> ObtenerTodasLasClases()
    {
        return _contexto.Set<Clase>()
            .Include(c => c.MetodosInvocados)
            .Include(c => c.Atributos)
            .ToList();
    }

    public void EliminarClase(Clase? clase)
    {
        var aEliminar = _contexto.Set<Clase>().FirstOrDefault(a => a.Id == clase.Id);
        _contexto.Set<Clase>().Remove(aEliminar!);
        _contexto.SaveChanges();
    }

    public void Actualizar(Clase clase)
    {
        var set = _contexto.Set<Clase>();
        set.Update(clase);
        _contexto.SaveChanges();
    }
}
