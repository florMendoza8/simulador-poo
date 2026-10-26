using Dominio;

namespace AccesoADatos.InterfacesRepositorios;

public interface IApiKeyRepositorio
{
    void Agregar(ApiKey apiKey);

    bool Existe(Guid apiKeyId);
}
