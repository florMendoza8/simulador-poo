using Dominio.Excepciones;
using Reflection;

namespace ReflectionTest;

[TestClass]
public class CargadorDePluginsTest
{
    private CargadorDePlugins<ITransformador>? _cargador;

    [TestInitialize]
    public void Inicializar()
    {
        _cargador = new CargadorDePlugins<ITransformador>();
    }

    [TestMethod]
    public void ObtenerImplementaciones_DeberiaDevolverNombreTransformadorHtml()
    {
        var nombres = _cargador!.ObtenerImplementaciones();

        Assert.IsTrue(nombres.Contains("TransformadorHtmlServicio"));
    }

    [TestMethod]
    public void ObtenerImplementaciones_DeberiaDevolverListaNoVacia()
    {
        var nombres = _cargador!.ObtenerImplementaciones();

        Assert.IsTrue(nombres.Count > 0);
    }

    [TestMethod]
    public void ObtenerImplementacion_DeberiaCrearInstanciaDeTransformadorHtml()
    {
        _cargador!.ObtenerImplementaciones();
        var instancia = _cargador.ObtenerImplementacion(0);

        Assert.IsInstanceOfType(instancia, typeof(ITransformador));
    }

    [TestMethod]
    [ExpectedException(typeof(OperacionNoPermitidaExcepcion))]
    public void ObtenerImplementacion_DeberiaLanzarExcepcionSiIndiceInvalido()
    {
        _cargador!.ObtenerImplementaciones();
        var instancia = _cargador!.ObtenerImplementacion(99);
    }

    [TestMethod]
    public void ObtenerImplementacion_DeberiaCrearloCorrectamente()
    {
        _cargador!.ObtenerImplementaciones();
        var instancia = _cargador.ObtenerImplementacion(0);

        Assert.IsNotNull(instancia);
        Assert.IsInstanceOfType(instancia, typeof(ITransformador));
    }

    [TestMethod]
    public void Constructor_DeberiaCrearInstanciaCorrectamente()
    {
        var ruta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins");

        var cargador = new CargadorDePlugins<ITransformador>();

        Assert.IsNotNull(cargador);
    }
}
