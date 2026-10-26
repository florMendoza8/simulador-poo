using AccesoADatos.Repositorios;
using Dominio;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace AccesoADatosTest;

[TestClass]
public class ApiKeyRepositorioTest
{
    private Guid _id;
    private IQueryable<ApiKey>? _data;
    private Mock<DbSet<ApiKey>>? _mockSet;
    private Mock<DbContext>? _apiKeyManagerContextMock;
    private ApiKeyRepositorio? _apiKeyAccesoADatos;
    private ApiKey _apiKey = null;

    [TestInitialize]
    public void Inicializar()
    {
        _id = Guid.NewGuid();
        _apiKey = new ApiKey { Id = _id };
        _data = new List<ApiKey>
        {
            _apiKey
        }.AsQueryable();
        _mockSet = new Mock<DbSet<ApiKey>>();
        _mockSet.As<IQueryable<ApiKey>>().Setup(a => a.Provider).Returns(_data.Provider);
        _mockSet.As<IQueryable<ApiKey>>().Setup(a => a.Expression).Returns(_data.Expression);
        _mockSet.As<IQueryable<ApiKey>>().Setup(a => a.ElementType).Returns(_data.ElementType);
        _mockSet.As<IQueryable<ApiKey>>().Setup(a => a.GetEnumerator()).Returns(() => _data.GetEnumerator());
        _apiKeyManagerContextMock = new Mock<DbContext>();
        _apiKeyAccesoADatos = new ApiKeyRepositorio(_apiKeyManagerContextMock.Object);
        _apiKeyManagerContextMock.Setup(x => x.Set<ApiKey>()).Returns(_mockSet.Object);
    }

    [TestMethod]
    public void AgregarApiKeyTest()
    {
        _apiKeyManagerContextMock.Setup(x => x.SaveChanges()).Returns(1);

        _apiKeyAccesoADatos.Agregar(_apiKey);

        _apiKeyManagerContextMock.Verify(blogSystemContext => blogSystemContext.SaveChanges(), Times.Once);
    }

    [TestMethod]
    public void ExisteApiKeyTest()
    {
        var resultado = _apiKeyAccesoADatos.Existe(_apiKey.Id);

        Assert.IsTrue(resultado);
    }
}
