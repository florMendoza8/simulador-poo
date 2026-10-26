using Dominio;

namespace DominioTest;

[TestClass]
public class NamespaceTest
{
    [TestMethod]
    public void CrearNamespace_ConstructorVacioTest()
    {
        var name_space = new Namespace();
        Assert.IsNotNull(name_space);
    }

    [TestMethod]
    public void CrearNamespace_ConstructorConParametroTest()
    {
        var padre = new Namespace("Root", null);
        var hijo = new Namespace("Child", padre);

        Assert.AreEqual("Child", hijo.Nombre);
        Assert.AreSame(padre, hijo.Padre);
    }

    [TestMethod]
    public void AsignarPropiedadesCorrectamenteTest()
    {
        var name_space = new Namespace();
        var id = Guid.NewGuid();

        name_space.Id = id;
        name_space.Nombre = "nombre";
        Assert.IsTrue(name_space.Nombre == "nombre");
    }

    [TestMethod]
    public void CrearNamespace_DeberiaGeneradaIdTest()
    {
        var name_space = new Namespace("nombre", null);
        Assert.AreNotEqual(Guid.Empty, name_space.Id);
    }

    [TestMethod]
    public void AgregarElemento_A_Namespace_DeberiaAgregarCorrectamenteTest()
    {
        var nameSpace = new Namespace("Padre", null);
        var clase = new Clase();

        nameSpace.Agregar(clase);

        Assert.IsTrue(nameSpace.ObtenerElementos().Contains(clase));
    }

    [TestMethod]
    public void EliminarElemento_De_Namespace_DeberiaEliminarCorrectamenteTest()
    {
        var nameSpace = new Namespace("Padre", null);
        var clase = new Clase();

        nameSpace.Agregar(clase);
        nameSpace.Eliminar(clase);

        Assert.IsFalse(nameSpace.ObtenerElementos().Contains(clase));
    }

    [TestMethod]
    public void ObtenerElemento_EnHijosDirectos_DeberiaRetornarElementoTest()
    {
        var clase = new Clase();
        var nameSpace = new Namespace("Padre", null);
        nameSpace.Agregar(clase);

        var resultado = nameSpace.ObtenerElemento(clase.Id);

        Assert.AreEqual(clase, resultado);
    }

    [TestMethod]
    public void ObtenerElemento_EnHijosAnidados_DeberiaRetornarElementoTest()
    {
        var clase = new Clase();
        var nameSpaceHijo = new Namespace("Hijo", null);
        nameSpaceHijo.Agregar(clase);

        var nameSpacePadre = new Namespace("Padre", null);
        nameSpacePadre.Agregar(nameSpaceHijo);

        var resultado = nameSpacePadre.ObtenerElemento(clase.Id);

        Assert.AreEqual(clase, resultado);
    }

    [TestMethod]
    public void ObtenerElemento_CuandoNoExiste_DeberiaRetornarNullTest()
    {
        var nameSpace = new Namespace("Padre", null);
        var resultado = nameSpace.ObtenerElemento(Guid.NewGuid());

        Assert.IsNull(resultado);
    }

    [TestMethod]
    public void ObtenerElemento_CuandoIdCoincideConNamespace_DeberiaRetornarEsteTest()
    {
        var nameSpace = new Namespace("Raiz", null);
        var resultado = nameSpace.ObtenerElemento(nameSpace.Id);

        Assert.AreEqual(nameSpace, resultado);
    }

    [TestMethod]
    public void ObtenerElemento_RecorreHijosPeroNoEncuentraCoincidencia_DeberiaRetornarNullTest()
    {
        var hijo1 = new Clase();
        var hijo2 = new Interfaz();

        var nameSpace = new Namespace("Raiz", null);
        nameSpace.Agregar(hijo1);
        nameSpace.Agregar(hijo2);

        var idInexistente = Guid.NewGuid();
        var resultado = nameSpace.ObtenerElemento(idInexistente);

        Assert.IsNull(resultado);
    }

    [TestMethod]
    public void Namespace_DeberiaAgregarClaseEnListaTest()
    {
        var nameSpace = new Namespace("Root", null);
        var clase = new Clase();
        nameSpace.Clases.Add(clase);

        Assert.IsTrue(nameSpace.Clases.Contains(clase));
    }

    [TestMethod]
    public void Namespace_DeberiaAgregarInterfazEnListaTest()
    {
        var nameSpace = new Namespace("Root", null);
        var interfaz = new Interfaz();
        nameSpace.Interfazes.Add(interfaz);

        Assert.IsTrue(nameSpace.Interfazes.Contains(interfaz));
    }

    [TestMethod]
    public void Namespace_DeberiaAgregarNamespaceHijoEnListaTest()
    {
        var padre = new Namespace("Root", null);
        var hijo = new Namespace("Child", padre);
        padre.NamespacesHijos.Add(hijo);

        Assert.IsTrue(padre.NamespacesHijos.Contains(hijo));
    }
}
