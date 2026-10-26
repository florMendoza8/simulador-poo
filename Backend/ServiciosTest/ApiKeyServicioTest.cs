using AccesoADatos.InterfacesRepositorios;
using Dominio.Excepciones;
using Moq;
using Servicios;

namespace ServiciosTest;

[TestClass]
public class ApiKeyServicioTest
{
    private Mock<IApiKeyRepositorio>? _apiKeyRepositorioMock;
    private ApiKeyServicio? _apiKeyServicio;
    private string? _id;

    [TestInitialize]
    public void Inicializar()
    {
        _apiKeyRepositorioMock = new Mock<IApiKeyRepositorio>(MockBehavior.Strict);
        _apiKeyServicio = new ApiKeyServicio(_apiKeyRepositorioMock.Object);

        _id = Guid.NewGuid().ToString();
    }

    [TestMethod]
    [ExpectedException(typeof(NoAutorizadoExcepcion))]
    public void EsClaveValida_ApiKey_Vacia_DebeLanzarExcepcion()
    {
        var claveInvalida = string.Empty;

        _apiKeyServicio.EsClaveValida(claveInvalida);
    }

    [TestMethod]
    public void EsClaveValida_ApiKeyExiste_DebeDevolverTrue()
    {
        var claveValida = _id!.ToString().ToUpper();
        var apiKey = Guid.Parse(claveValida);
        _apiKeyRepositorioMock!
            .Setup(r => r.Existe(apiKey))
            .Returns(true);

        var resultado = _apiKeyServicio!.EsClaveValida(claveValida);

        Assert.IsTrue(resultado);
        _apiKeyRepositorioMock.Verify(r => r.Existe(apiKey), Times.Once);
    }

    [TestMethod]
    [ExpectedException(typeof(NoAutorizadoExcepcion))]
    public void EsClaveValida_ApiKeyConFormatoIncorrecto_LanzaExcepcion()
    {
        var claveInvalida = "123123123Hola!";

        _apiKeyServicio.EsClaveValida(claveInvalida);
    }
}
