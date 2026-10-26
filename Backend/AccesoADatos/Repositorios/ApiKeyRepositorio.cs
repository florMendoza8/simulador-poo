using AccesoADatos.InterfacesRepositorios;
using Dominio;
using Microsoft.EntityFrameworkCore;

namespace AccesoADatos.Repositorios;

public class ApiKeyRepositorio : IApiKeyRepositorio
{
    private readonly DbContext _dbContext;

    public ApiKeyRepositorio(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Agregar(ApiKey apiKey)
    {
        _dbContext.Set<ApiKey>().Add(apiKey);
        _dbContext.SaveChanges();
    }

    public bool Existe(Guid apiKeyId)
    {
        return _dbContext.Set<ApiKey>().Any(apiKey => apiKey.Id == apiKeyId);
    }
}
