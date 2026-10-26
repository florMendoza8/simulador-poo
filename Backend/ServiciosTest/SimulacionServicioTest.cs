using System.Diagnostics.Metrics;
using System.Text.Json.Nodes;
using AccesoADatos.InterfacesRepositorios;
using Dominio;
using Dominio.Excepciones;
using Moq;
using Reflection;
using Reflection.InterfacesReflection;
using Servicios;
using Servicios.InterfacesServicios;

namespace ServiciosTest;

[TestClass]
public class SimulacionServicioTest
{
    private SimulacionServicio? _simulacionServicio;
    private Mock<IMetodoServicio>? _metodoServicioMock;
    private Mock<IClaseServicio>? _claseServicioMock;
    private Mock<ICargadorDePlugins<ITransformador>>? _cargadorDePluginsMock;
    private Clase? _tipoReferencia;
    private Clase? _tipoObjeto;
    private Metodo? _metodoReferencia;
    private List<Metodo>? _listaMetodo;
    private Clase? _clase1;
    private Clase? _clase2;
    private Clase? _clase3;
    private Clase? _claseBase;
    private Metodo? _metodo1;
    private Metodo? _metodo2;
    private Metodo? _metodo3;
    private Metodo? _metodo4;
    private Metodo? _metodo5;

    [TestInitialize]
    public void Inicializar()
    {
        _metodoServicioMock = new Mock<IMetodoServicio>(MockBehavior.Strict);
        _claseServicioMock = new Mock<IClaseServicio>(MockBehavior.Strict);
        _cargadorDePluginsMock = new Mock<ICargadorDePlugins<ITransformador>>(MockBehavior.Strict);
        _simulacionServicio = new SimulacionServicio(
            _claseServicioMock.Object, _metodoServicioMock.Object, _cargadorDePluginsMock.Object);
        _claseBase = new Clase
        {
            Nombre = "ClaseBase",
            EsAbstract = false,
            EsSealed = false,
            ClaseBase = null,
            Atributos = [],
            MetodosInvocados = []
        };
        _clase1 = new Clase
        {
            Nombre = "Clase1",
            EsAbstract = false,
            EsSealed = false,
            ClaseBase = _claseBase,
            Atributos = [],
            MetodosInvocados = []
        };
        _clase2 = new Clase
        {
            Nombre = "Clase2",
            EsAbstract = false,
            EsSealed = false,
            ClaseBase = null,
            Atributos = [],
            MetodosInvocados = []
        };
        _clase3 = new Clase
        {
            Nombre = "Clase3",
            EsAbstract = false,
            EsSealed = false,
            ClaseBase = null,
            Atributos = [],
            MetodosInvocados = []
        };

        _metodo1 = new Metodo("Metodo1", new TipoDato(), false, false,
            Accesibilidad.Public, _clase1,
            new List<Parametro>()
            {
                new Parametro("obj2", new TipoDato("Clase2", null)),
            }, new List<VariableLocal>(), new List<Metodo>());

        _metodo2 = new Metodo("Metodo2", new TipoDato(), false, false,
            Accesibilidad.Public, _clase1,
            new List<Parametro>(), new List<VariableLocal>(),
            new List<Metodo>());

        _metodo3 = new Metodo("Metodo3", new TipoDato(), false, false,
            Accesibilidad.Public, _clase2,
            new List<Parametro>(), new List<VariableLocal>(),
            new List<Metodo>());

        _metodo4 = new Metodo("Metodo4", new TipoDato(), false, false,
            Accesibilidad.Public, _clase3,
            new List<Parametro>(), new List<VariableLocal>(),
            new List<Metodo>());

        _metodo5 = new Metodo("Metodo5", new TipoDato(), false, false,
            Accesibilidad.Public, _claseBase,
            new List<Parametro>()
            {
                new Parametro("obj2", new TipoDato("Clase2", null)),
            }, new List<VariableLocal>(), new List<Metodo>());

        _metodo1.MetodosInvocados.Add(_metodo2);
        _metodo1.MetodosInvocados.Add(_metodo3);
        _metodo1.MetodosInvocados.Add(_metodo5);
        _metodo3.MetodosInvocados.Add(_metodo4);

        _listaMetodo = new List<Metodo>()
        {
            _metodo1
        };
        _clase1.MetodosInvocados = _listaMetodo;
        _tipoReferencia = _clase1;
        _tipoObjeto = _clase1;
        _metodoReferencia = _listaMetodo[0];
    }

    [TestMethod]
    public void EjecutarSimulacion_CuandoDatosValidos_DebeHacerloCorrectamente()
    {
        var resultado = _simulacionServicio!.EjecutarSimulacion(_tipoReferencia!, _tipoObjeto!,
            _metodoReferencia!, _tipoObjeto.Nombre!);
        Assert.IsNotNull(resultado);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNulaExcepcion))]
    public void EjecutarSimulacion_CuandoTipoReferenciaEsNull_LanzaExcepcion()
    {
        var tipoObjeto = new Clase { Nombre = "Clase1" };
        var metodo = new Metodo();

        _simulacionServicio!.EjecutarSimulacion(null!, tipoObjeto, metodo, "Clase1");
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNulaExcepcion))]
    public void EjecutarSimulacion_CuandoTipoObjetoEsNull_LanzaExcepcion()
    {
        var tipoReferencia = new Clase { Nombre = "Clase1" };
        var metodo = new Metodo();

        _simulacionServicio!.EjecutarSimulacion(tipoReferencia, null!, metodo, "Clase1");
    }

    [TestMethod]
    [ExpectedException(typeof(OperacionNoPermitidaExcepcion))]
    public void EjecutarSimulacion_CuandoTipoObjetoEsAbstracto_LanzaExcepcion()
    {
        var tipoObjeto = new Clase { Nombre = "Clase1", EsAbstract = true };
        var metodo = new Metodo();

        _simulacionServicio!.EjecutarSimulacion(tipoObjeto, tipoObjeto, metodo, "Clase1");
    }

    [TestMethod]
    public void EjecutarSimulacionTransformada_SinTransformador_DeberiaRetornarFormatoTexto()
    {
        var resultado = _simulacionServicio!.EjecutarSimulacionTransformada(
            _tipoReferencia!, _tipoObjeto!, _metodoReferencia!, "this", null);

        Assert.IsTrue(resultado.Contains("this.Metodo1"));
    }

    [TestMethod]
    public void EjecutarSimulacionTransformada_ConTransformadorValido_DeberiaAplicarlo()
    {
        var transformadorMock = new Mock<ITransformador>();
        transformadorMock.Setup(t => t.Transformar(It.IsAny<JsonObject>()))
            .Returns("<html>resultado</html>");
        transformadorMock.Setup(t => t.Nombre).Returns("HTML");

        _cargadorDePluginsMock!.Setup(c => c.ObtenerImplementaciones())
            .Returns(["HTML"]);
        _cargadorDePluginsMock.Setup(c => c.ObtenerImplementacion(0))
            .Returns(transformadorMock.Object);

        var resultado = _simulacionServicio!.EjecutarSimulacionTransformada(
            _tipoReferencia!, _tipoObjeto!, _metodoReferencia!, "this", "HTML");

        Assert.AreEqual("<html>resultado</html>", resultado);
    }

    [TestMethod]
    [ExpectedException(typeof(OperacionNoPermitidaExcepcion))]
    public void EjecutarSimulacionTransformada_TransformadorNoExiste_DeberiaLanzarExcepcion()
    {
        _cargadorDePluginsMock!.Setup(c => c.ObtenerImplementaciones()).Returns(["Otro"]);

        var simulacion = _simulacionServicio!.EjecutarSimulacionTransformada(
            _tipoReferencia!, _tipoObjeto!, _metodoReferencia!, "this", "Inexistente");
    }
}
