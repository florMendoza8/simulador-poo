using Dominio;
namespace DominioTest;

[TestClass]
public class TipoDatoTest
{
    [TestMethod]
    public void CrearTipoDato_ConstructorVacio()
    {
        var tipoDato = new TipoDato();
        Assert.IsNotNull(tipoDato);
    }

    [TestMethod]
    public void CrearTipoDato_ConstructorConParametro()
    {
        var tipoDato = new TipoDato("string", null);
        Assert.IsNotNull(tipoDato);
    }

    [TestMethod]
    public void AsignarPropiedadesTipoDato()
    {
        var tipoDato = new TipoDato();
        var id = Guid.NewGuid();
        tipoDato.Id = id;
        tipoDato.Nombre = "entero";
        tipoDato.TipoPadre = null;

        Assert.AreEqual(id, tipoDato.Id);
        Assert.AreEqual("entero", tipoDato.Nombre);
        Assert.IsNull(tipoDato.TipoPadre);
    }

    [TestMethod]
    public void CrearTipoDato_ConNombre_DeberiaGenerarId()
    {
        var tipoDato = new TipoDato("booleano", null);
        Assert.AreNotEqual(Guid.Empty, tipoDato.Id);
        Assert.IsNull(tipoDato.TipoPadre);
    }

    [TestMethod]
    public void AsignarTipoPadreCorrectamenteTest()
    {
        var padre = new TipoDato("Padre", null);
        var hijo = new TipoDato("Hijo", padre)
        {
            TipoPadre = padre
        };
        Assert.AreEqual(padre, hijo.TipoPadre);
    }
}
