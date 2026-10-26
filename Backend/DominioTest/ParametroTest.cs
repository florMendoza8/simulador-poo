using Dominio;
namespace DominioTest;

[TestClass]
public class ParametroTest
{
    [TestMethod]
    public void CrearParametro_ConConstructorVacio_DeberiaCrearloVacio()
    {
        var parametro = new Parametro();

        Assert.IsNotNull(parametro);
    }

    [TestMethod]
    public void CrearParametro_ConConstructorConParametro_DeberiaCrearloCorrectamente()
    {
        var dato = new TipoDato();

        var parametro = new Parametro("nombre", dato);

        Assert.AreSame(parametro.TipoDato, dato);
        Assert.AreEqual(parametro.Nombre, "nombre");
    }

    [TestMethod]
    public void CrearParametroConParametros_DeberiaCrearloConGuidNoVacio()
    {
        var dato = new TipoDato();

        var parametro = new Parametro("nombre", dato);

        Assert.IsNotNull(parametro.Id);
    }

    [TestMethod]
    public void CrearParametroConConstructorVacio_IdNoDeberiaSerNulo()
    {
        var parametro = new Parametro();

        Assert.IsNotNull(parametro.Id);
    }

    [TestMethod]
    public void AsignarIdAParametro_DeberiaHacerloCorrectamente()
    {
        var parametro = new Parametro();
        var idParametro = Guid.NewGuid();

        parametro.Id = idParametro;

        Assert.AreEqual(idParametro, parametro.Id);
    }

    [TestMethod]
    public void AsignarNombreAParametro_DeberiaHacerloCorrectamente()
    {
        var parametro = new Parametro();
        var nombreParametro = "nombre";

        parametro.Nombre = nombreParametro;

        Assert.AreEqual(nombreParametro, parametro.Nombre);
    }

    [TestMethod]
    public void AsignarTipoDatoAParametro_DeberiaHacerloCorrectamente()
    {
        var parametro = new Parametro();
        var dato = new TipoDato();
        parametro.TipoDato = dato;

        Assert.AreEqual(dato, parametro.TipoDato);
    }
}
