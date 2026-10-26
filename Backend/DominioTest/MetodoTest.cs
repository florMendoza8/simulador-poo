using Dominio;
namespace DominioTest;

[TestClass]
public class MetodoTest
{
    [TestMethod]
    public void CrearMetodo_ConConstructorVacio_DeberiaCrearloCorrectamente()
    {
        var metodo = new Metodo();

        Assert.IsNotNull(metodo);
    }

    [TestMethod]
    public void CrearMetodoConConstructorVacio_IdNoDeberiaSerNulo()
    {
        var metodo = new Metodo();

        Assert.IsNotNull(metodo.Id);
    }

    [TestMethod]
    public void AsignarIdAMetodo_DeberiaHacerloCorrectamente()
    {
        var metodo = new Metodo();
        var idMetodo = Guid.NewGuid();

        metodo.Id = idMetodo;

        Assert.AreEqual(idMetodo, metodo.Id);
    }

    [TestMethod]
    public void CrearMetodo_ConConstructorConParametro_DeberiaCrearloCorrectamente()
    {
        var nombreMetodo = "nombre";
        var tipoDatoRetorno = new TipoDato();
        var esAbstract = false;
        var esSealed = false;
        var accesibilidad = Accesibilidad.Public;
        var clase = new Clase();
        var parametros = new List<Parametro>();
        var variablesLocales = new List<VariableLocal>();
        var metodosInvocados = new List<Metodo>();

        var metodo = new Metodo(nombreMetodo, tipoDatoRetorno, esAbstract, esSealed, accesibilidad, clase, parametros,
            variablesLocales, metodosInvocados);

        Assert.IsNotNull(metodo);
    }

    [TestMethod]
    public void CrearMetodo_ConConstructorConParametroParaFirmaMetodo_DeberiaCrearloCorrectamente()
    {
        var nombreMetodo = "nombre";
        var tipoDatoRetorno = new TipoDato();
        var parametros = new List<Parametro>();

        var metodo = new Metodo(nombreMetodo, tipoDatoRetorno, parametros);

        Assert.IsNotNull(metodo);
    }

    [TestMethod]
    public void CrearMetodoVacio_DeberiaTenerListaParametrosVacia()
    {
        var metodo = new Metodo();

        Assert.IsNotNull(metodo.Parametros);
    }

    [TestMethod]
    public void CrearMetodoVacio_DeberiaTenerListaVariablesLocalesVacia()
    {
        var metodo = new Metodo();

        Assert.IsNotNull(metodo.VariablesLocales);
    }

    [TestMethod]
    public void CrearMetodoVacio_DeberiaTenerListaMetodosInvocadosVacia()
    {
        var metodo = new Metodo();

        Assert.IsNotNull(metodo.MetodosInvocados);
    }

    [TestMethod]
    public void AsignarNombreAMetodo_DeberiaHacerloCorrectamente()
    {
        var metodo = new Metodo();
        var nombre = "Nombre";
        metodo.Nombre = nombre;

        Assert.AreEqual(nombre, metodo.Nombre);
    }

    [TestMethod]
    public void AsignarTipoRetornoAMetodo_DeberiaHacerloCorrectamente()
    {
        var metodo = new Metodo();
        var tipoRetorno = new TipoDato();
        metodo.TipoRetorno = tipoRetorno;

        Assert.AreEqual(tipoRetorno, metodo.TipoRetorno);
    }

    [TestMethod]
    public void AsignarEsAbstractAMetodo_DeberiaHacerloCorrectamente()
    {
        var metodo = new Metodo();
        var esAbstract = true;
        metodo.EsAbstract = esAbstract;

        Assert.AreEqual(esAbstract, metodo.EsAbstract);
    }

    [TestMethod]
    public void AsignarEsSealedAMetodo_DeberiaHacerloCorrectamente()
    {
        var metodo = new Metodo();
        var esSealed = true;
        metodo.EsSealed = esSealed;

        Assert.AreEqual(esSealed, metodo.EsSealed);
    }

    [TestMethod]
    public void AsignarEsEstaticoAMetodo_DeberiaHacerloCorrectamente()
    {
        var metodo = new Metodo();
        var esEstatico = true;
        metodo.EsEstatico = esEstatico;

        Assert.AreEqual(esEstatico, metodo.EsEstatico);
    }

    [TestMethod]
    public void AsignarEsVirtualAMetodo_DeberiaHacerloCorrectamente()
    {
        var metodo = new Metodo();
        var esVirtual = true;
        metodo.EsVirtual = esVirtual;

        Assert.AreEqual(esVirtual, metodo.EsVirtual);
    }

    [TestMethod]
    public void AsignarEsOverrideAMetodo_DeberiaHacerloCorrectamente()
    {
        var metodo = new Metodo();
        var esOverride = true;
        metodo.EsOverride = esOverride;

        Assert.AreEqual(esOverride, metodo.EsOverride);
    }

    [TestMethod]
    public void AsignarAccesibilidadAMetodo_DeberiaHacerloCorrectamente()
    {
        var metodo = new Metodo();
        var accesibilidad = Accesibilidad.Private;
        metodo.Accesibilidad = accesibilidad;

        Assert.AreEqual(accesibilidad, metodo.Accesibilidad);
    }

    [TestMethod]
    public void AsignarClaseIdAMetodo_DeberiaHacerloCorrectamente()
    {
        var metodo = new Metodo();
        var clase = new Clase();
        metodo.Clase = clase;

        Assert.AreEqual(clase, metodo.Clase);
    }
}
