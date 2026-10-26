using Dominio;

namespace DominioTest;

[TestClass]
public class ApiKeyTest
{
    [TestMethod]
    public void CrearApiKey_ConConstructorVacio_DebeSerNoNulo()
    {
        var clave = new ApiKey();

        Assert.IsNotNull(clave);
    }

    [TestMethod]
    public void CrearApiKey_ConConstructorVacio_DebeSerGuid()
    {
        var clave = new ApiKey();

        Assert.AreNotEqual(Guid.Empty, clave.Id);
    }

    [TestMethod]
    public void CrearApiKey_ConConstructorNoVacio_DebeCrearloCorrectamente()
    {
        var guidEsperado = Guid.NewGuid();
        var clave = new ApiKey(guidEsperado);

        Assert.AreEqual(guidEsperado, clave.Id);
    }
}
