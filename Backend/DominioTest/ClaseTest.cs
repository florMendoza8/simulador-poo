using Dominio;
using Dominio.Excepciones;

namespace DominioTest;

[TestClass]
public class ClaseTest
{
    [TestMethod]
    public void CrearClase_ConstructorVacioTest()
    {
        var clase = new Clase();
        Assert.IsNotNull(clase);
    }

    [TestMethod]
    public void CrearClase_ConstructorConParametroTest()
    {
        var nombreClase = "celular";
        var esAbstract = false;
        var esSealed = false;
        var atributos = new List<Atributo>();
        var metodos = new List<Metodo>();
        var claseBase = new Clase();
        var claseBaseId = claseBase.Id;
        var interfacesImplementadas = new List<Interfaz>();

        var clase = new Clase(nombreClase, esAbstract, esSealed, atributos, metodos, claseBase, claseBaseId, interfacesImplementadas);
        Assert.IsNotNull(clase);
    }

    [TestMethod]
    public void ConstructorClaseVacio_IdNoDebeSerNuloTest()
    {
        var clase = new Clase();
        Assert.IsNotNull(clase.Id);
    }

    [TestMethod]
    public void AsignarIdAClaseCorrectamenteTest()
    {
        var clase = new Clase();
        var idClase = Guid.NewGuid();

        clase.Id = idClase;
        Assert.AreEqual(idClase, clase.Id);
    }

    [TestMethod]
    public void AsignacionDePropiedadesCorrectamenteTest()
    {
        var clase = new Clase();
        var atributos = new List<Atributo> { new Atributo() };
        var metodos = new List<Metodo> { new Metodo() };
        var clasebase = new Clase();
        var interfacesImplementadas = new List<Interfaz>();

        clase.Nombre = "Prueba";
        clase.EsAbstract = false;
        clase.EsSealed = true;
        clase.Atributos = atributos;
        clase.MetodosInvocados = metodos;
        clase.ClaseBase = clasebase;
        clase.InterfacesImplementadas = interfacesImplementadas;

        Assert.AreEqual("Prueba", clase.Nombre);
        Assert.IsFalse(clase.EsAbstract);
        Assert.IsTrue(clase.EsSealed);
        Assert.AreEqual(atributos, clase.Atributos);
        Assert.AreEqual(metodos, clase.MetodosInvocados);
        Assert.AreEqual(clasebase, clase.ClaseBase);
        Assert.AreEqual(interfacesImplementadas, clase.InterfacesImplementadas);
    }

    [TestMethod]
    [ExpectedException(typeof(NoImplementadoExcepcion))]
    public void Agregar_EnClase_LanzaExcepcionTest()
    {
        var clase = new Clase();
        var otro = new Clase();
        clase.Agregar(otro);
    }

    [TestMethod]
    [ExpectedException(typeof(NoImplementadoExcepcion))]
    public void Eliminar_EnClase_LanzaExcepcionTest()
    {
        var clase = new Clase();
        var otro = new Clase();
        clase.Eliminar(otro);
    }

    [TestMethod]
    public void ObtenerElemento_CuandoIdCoincide_RetornaInstanciaTest()
    {
        var clase = new Clase();
        var resultado = clase.ObtenerElemento(clase.Id);
        Assert.AreEqual(clase, resultado);
    }

    [TestMethod]
    public void ObtenerElemento_CuandoIdNoCoincide_RetornaNullTest()
    {
        var clase = new Clase();
        var otroId = Guid.NewGuid();
        var resultado = clase.ObtenerElemento(otroId);
        Assert.IsNull(resultado);
    }

    [TestMethod]
    public void AsignarNamespaceAClase_DeberiaMantenerReferencia()
    {
        var nameSpace = new Namespace("Simulador", null);
        var clase = new Clase();

        clase.Namespace = nameSpace;

        Assert.IsNotNull(clase.Namespace);
        Assert.AreEqual("Simulador", clase.Namespace?.Nombre);
    }

    [TestMethod]
    public void Clase_CuandoTieneNamespace_DeberiaCoincidirElId()
    {
        var nameSpace = new Namespace("Dominio", null);
        var clase = new Clase { Namespace = nameSpace };

        Assert.AreEqual(nameSpace.Id, clase.Namespace?.Id);
    }

    [TestMethod]
    public void Clase_CuandoNoTieneNamespace_DeberiaSerNull()
    {
        var clase = new Clase();

        Assert.IsNull(clase.Namespace);
    }
}
