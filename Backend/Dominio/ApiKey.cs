namespace Dominio;

public class ApiKey
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public ApiKey()
    {
    }

    public ApiKey(Guid id)
    {
        Id = id;
    }
}
