using Dominio;
namespace DominioTest;

[TestClass]
public class VariableLocalTest
{
    [TestMethod]
    public void CrearVariableLocalConstructorSinParametroTest()
    {
        var variableLocalVacio = new VariableLocal();
        Assert.IsNotNull(variableLocalVacio);
    }

    [TestMethod]
    public void CrearVariableLocalConstructorConParametroTest()
    {
        var dato = new TipoDato();
        var tipoInstancia = new TipoDato();
        var variableLocal = new VariableLocal("string", dato, tipoInstancia, "25");
        Assert.IsNotNull(variableLocal);
    }

    [TestMethod]
    public void AccederNombreDeVariableYTipoReferenciaLocalTest()
    {
        var dato = new TipoDato();
        var tipoInstancia = new TipoDato();
        var variableLocal = new VariableLocal("test", dato, tipoInstancia, "25");
        Assert.AreEqual("test", variableLocal.Nombre);
        Assert.AreEqual(dato, variableLocal.TipoReferencia);
    }

    [TestMethod]
    public void AccederIdDeVariableLocal()
    {
        var dato = new TipoDato();
        var tipoInstancia = new TipoDato();
        var variableLocal = new VariableLocal("test", dato, tipoInstancia, "25");
        Assert.AreNotEqual(Guid.Empty, variableLocal.Id);
    }

    [TestMethod]
    public void SetearIdDeVariableLocal()
    {
        var variableLocal = new VariableLocal();
        var nuevoId = Guid.NewGuid();
        variableLocal.Id = nuevoId;
        Assert.AreEqual(nuevoId, variableLocal.Id);
    }

    [TestMethod]
    public void ValorVariableNoNuloTest()
    {
        var variableLocal = new VariableLocal();
        variableLocal.Valor = "15";
        Assert.AreEqual("15", variableLocal.Valor);
    }

    [TestMethod]
    public void AccederTipoInstanciaTest()
    {
        var dato = new TipoDato();
        var tipoInstancia = new TipoDato();
        var variableLocal = new VariableLocal("test", dato, tipoInstancia, "25");
        Assert.AreEqual(tipoInstancia, variableLocal.TipoInstancia);
    }
}
