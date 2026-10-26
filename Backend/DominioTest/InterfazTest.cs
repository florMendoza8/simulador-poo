using Dominio;
using Dominio.Excepciones;

namespace DominioTest;

[TestClass]
public class InterfazTest
{
    [TestMethod]
    public void CrearInterfaz_ConstructorVacio()
    {
        var interfaz = new Interfaz();
        Assert.IsNotNull(interfaz);
    }

    [TestMethod]
    public void CrearInterfaz_ConstructorConParametro()
    {
        var firmaMetodo = new List<Metodo>();
        var interfaz = new Interfaz("nombre", firmaMetodo);
        Assert.IsNotNull(interfaz);
    }

    [TestMethod]
    public void AsignarPropiedadesInterfaz()
    {
        var interfaz = new Interfaz();
        var firmaMetodos = new List<Metodo>();
        var id = Guid.NewGuid();

        interfaz.Id = id;
        interfaz.Nombre = "nombre";
        interfaz.FirmaMetodos = firmaMetodos;

        Assert.IsTrue(
            interfaz.Nombre == "nombre" &&
            interfaz.FirmaMetodos == firmaMetodos);
    }

    [TestMethod]
    public void CrearInterfazConPropiedades_DeberiaGenerarId()
    {
        var firmaMetodos = new List<Metodo>();
        var interfaz = new Interfaz("nombre", firmaMetodos);
        Assert.AreNotEqual(Guid.Empty, interfaz.Id);
    }

    [TestMethod]
    [ExpectedException(typeof(NoImplementadoExcepcion))]
    public void Agregar_EnInterfaz_LanzaExcepcionTest()
    {
        var interfaz = new Interfaz();
        var otro = new Interfaz();
        interfaz.Agregar(otro);
    }

    [TestMethod]
    [ExpectedException(typeof(NoImplementadoExcepcion))]
    public void Eliminar_EnInterfaz_LanzaExcepcionTest()
    {
        var interfaz = new Interfaz();
        var otro = new Interfaz();
        interfaz.Eliminar(otro);
    }

    [TestMethod]
    public void ObtenerElemento_CuandoIdCoincide_RetornaInstanciaTest()
    {
        var interfaz = new Interfaz();
        var resultado = interfaz.ObtenerElemento(interfaz.Id);
        Assert.AreEqual(interfaz, resultado);
    }

    [TestMethod]
    public void ObtenerElemento_CuandoIdNoCoincide_RetornaNullTest()
    {
        var interfaz = new Interfaz();
        var otroId = Guid.NewGuid();
        var resultado = interfaz.ObtenerElemento(otroId);
        Assert.IsNull(resultado);
    }

    [TestMethod]
    public void AsignarNamespaceAInterfaz_DeberiaMantenerReferencia()
    {
        var nameSpace = new Namespace("Servicios", null);
        var interfaz = new Interfaz();

        interfaz.Namespace = nameSpace;

        Assert.IsNotNull(interfaz.Namespace);
        Assert.AreEqual("Servicios", interfaz.Namespace?.Nombre);
    }

    [TestMethod]
    public void Interfaz_CuandoTieneNamespace_DeberiaCoincidirElId()
    {
        var nameSpace = new Namespace("Dominio", null);
        var interfaz = new Interfaz { Namespace = nameSpace };

        Assert.AreEqual(nameSpace.Id, interfaz.Namespace?.Id);
    }

    [TestMethod]
    public void Interfaz_CuandoNoTieneNamespace_DeberiaSerNull()
    {
        var interfaz = new Interfaz();

        Assert.IsNull(interfaz.Namespace);
    }
}
