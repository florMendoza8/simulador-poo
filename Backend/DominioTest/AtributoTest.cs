using Dominio;

namespace DominioTest;

[TestClass]
public class AtributoTest
{
    [TestMethod]
    public void CrearAtributo_ConstructorVacio()
    {
        var atributo = new Atributo();
        Assert.IsNotNull(atributo);
    }

    [TestMethod]
    public void CrearAtributo_ConstructorConParametro()
    {
        var tipoDato = new TipoDato();
        var tipoInstancia = new TipoDato();
        var atributo = new Atributo("edad", tipoDato, tipoInstancia, accesibilidad: Accesibilidad.Private, false);
        Assert.IsNotNull(atributo);
    }

    [TestMethod]
    public void AsignarPropiedadesAtributo()
    {
        var atributo = new Atributo();
        var id = Guid.NewGuid();
        var tipoDato = new TipoDato("string", null);

        atributo.Id = id;
        atributo.Nombre = "nombre";
        atributo.TipoReferencia = tipoDato;
        atributo.Accesibilidad = Accesibilidad.Public;
        atributo.EsEstatico = false;

        Assert.IsTrue(
            atributo.Nombre == "nombre" &&
            atributo.TipoReferencia == tipoDato &&
            atributo.Accesibilidad == Accesibilidad.Public);
    }

    [TestMethod]
    public void CrearAtributo_ConPropiedades_DeberiaGenerarId()
    {
        var tipoDato = new TipoDato();
        var tipoInstancia = new TipoDato();
        var atributo = new Atributo("edad", tipoDato, tipoInstancia, accesibilidad: Accesibilidad.Private, false);
        Assert.AreNotEqual(Guid.Empty, atributo.Id);
    }
}
