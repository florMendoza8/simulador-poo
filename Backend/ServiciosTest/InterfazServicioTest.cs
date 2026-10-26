using AccesoADatos.InterfacesRepositorios;
using Dominio;
using Dominio.Excepciones;
using Moq;
using Servicios;
using Servicios.InterfacesServicios;

namespace ServiciosTest;

[TestClass]
public class InterfazServicioTest
{
    private Mock<IInterfazRepositorio>? _interfazRepositorioMock;
    private InterfazServicio? _interfazServicio;
    private Mock<IMetodoServicio>? _metodoServicioMock;
    private Interfaz _interfaz = null;
    private Guid _interfazId;

    [TestInitialize]
    public void Inicializar()
    {
        _interfazRepositorioMock = new Mock<IInterfazRepositorio>(MockBehavior.Strict);
        _metodoServicioMock = new Mock<IMetodoServicio>(MockBehavior.Strict);
        _interfazServicio = new InterfazServicio(_interfazRepositorioMock.Object, _metodoServicioMock.Object);
        _interfazId = Guid.NewGuid();
        _interfaz = new Interfaz { Id = _interfazId, Nombre = "Nombre", FirmaMetodos = new List<Metodo>() };
    }

    [TestMethod]
    public void Agregar_CuandoPropiedadesSonValidas_DebeAgregarInterfaz()
    {
        _interfazRepositorioMock?.Setup(repo => repo.Agregar(It.IsAny<Interfaz>()));

        var resultado = _interfazServicio?.CrearInterfaz(_interfaz);

        _interfazRepositorioMock?.VerifyAll();
        Assert.AreEqual(_interfaz?.Id, resultado?.Id);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNulaExcepcion))]
    public void Agregar_CuandoInterfazEsNull_LanzaExcepcion()
    {
        _interfazServicio?.CrearInterfaz(null);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNulaExcepcion))]
    public void Agregar_CuandoNombreEsVacio_LanzaExcepcion()
    {
        _interfaz.Nombre = string.Empty;
        _interfazServicio?.CrearInterfaz(_interfaz);
    }

    [TestMethod]
    public void Eliminar_CuandoInterfazEsValida_DebeEliminarla()
    {
        _interfazRepositorioMock?.Setup(repo => repo.Eliminar(_interfaz));

        _interfazServicio?.EliminarInterfaz(_interfaz);

        _interfazRepositorioMock?.VerifyAll();
    }

    [TestMethod]
    [ExpectedException(typeof(OperacionNoPermitidaExcepcion))]
    public void Eliminar_CuandoInterfazNoExiste_LanzaExcepcion()
    {
        _interfazRepositorioMock?.Setup(repo => repo.Eliminar(_interfaz))
            .Throws(new OperacionNoPermitidaExcepcion("La interfaz no existe."));

        _interfazServicio?.EliminarInterfaz(_interfaz);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNulaExcepcion))]
    public void Eliminar_CuandoInterfazEsNull_LanzaExcepcion()
    {
        _interfazServicio?.EliminarInterfaz(null);
    }

    [TestMethod]
    public void Obtener_CuandoInterfazExiste_DebeDevolverla()
    {
        _interfazRepositorioMock?.Setup(repo => repo.Obtener(_interfazId))
            .Returns(_interfaz);

        var resultado = _interfazServicio?.ObtenerInterfazPorId(_interfazId);

        Assert.AreEqual(_interfaz.Id, resultado!.Id);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNoEncontradaExcepcion))]
    public void Obtener_CuandoInterfazNoExiste_DebeLanzarExcepcion()
    {
        _interfazRepositorioMock?.Setup(repo => repo.Obtener(_interfazId))
            .Returns(((Interfaz?)null)!);

        _interfazServicio?.ObtenerInterfazPorId(_interfazId);
    }

    [TestMethod]
    public void AgregarMetodoAFirmaMetodos_CuandoMetodoNoExiste_SeAgregaYActualiza()
    {
        var tipoDato = new TipoDato("int");
        var metodo = new Metodo("Nuevo", tipoDato, []);

        _interfazRepositorioMock?
            .Setup(r => r.Obtener(_interfaz.Id))
            .Returns(_interfaz);

        _metodoServicioMock?
            .Setup(s => s.ObtenerMetodoPorId(metodo.Id))
            .Returns(metodo);

        _interfazRepositorioMock?.Setup(r => r.Actualizar(_interfaz));

        _interfazServicio?.AgregarMetodoAFirmaMetodos(_interfazId, metodo.Id);

        Assert.AreEqual("Nuevo", _interfaz.FirmaMetodos?[0].Nombre);
        _interfazRepositorioMock?.VerifyAll();
    }

    [TestMethod]
    public void AgregarMetodoAFirmaMetodos_MetodoMismoNombreConDistintoParametro_SeAgregaYActualiza()
    {
        var tipoDato = new TipoDato("int");
        var metodoExistente = new Metodo("Procesar", tipoDato, []);
        _interfaz.FirmaMetodos?.Add(metodoExistente);

        _interfazRepositorioMock?
            .Setup(r => r.Obtener(_interfaz.Id))
            .Returns(_interfaz);

        _metodoServicioMock?
            .Setup(s => s.ObtenerMetodoPorId(metodoExistente.Id))
            .Returns(metodoExistente);

        _interfazRepositorioMock?.Setup(r => r.Actualizar(_interfaz));

        var metodoConParametro = new Metodo("Procesar", tipoDato, [new Parametro { TipoDato = tipoDato }]);

        _metodoServicioMock?
            .Setup(s => s.ObtenerMetodoPorId(metodoConParametro.Id))
            .Returns(metodoConParametro);

        _interfazServicio?.AgregarMetodoAFirmaMetodos(_interfazId, metodoConParametro.Id);

        Assert.AreEqual("Procesar", _interfaz.FirmaMetodos?[1].Nombre);
        _interfazRepositorioMock?.VerifyAll();
    }

    [TestMethod]
    public void AgregarMetodoAFirmaMetodos_CuandoFirmaMetodosEsNull_SeInicializaYAgrega()
    {
        _interfaz.FirmaMetodos = null;
        var tipoDato = new TipoDato("int");
        var metodo = new Metodo("Nuevo", tipoDato, []);

        _interfazRepositorioMock?
            .Setup(r => r.Obtener(_interfaz.Id))
            .Returns(_interfaz);

        _metodoServicioMock?
            .Setup(s => s.ObtenerMetodoPorId(metodo.Id))
            .Returns(metodo);

        _interfazRepositorioMock?.Setup(r => r.Actualizar(_interfaz));

        _interfazServicio?.AgregarMetodoAFirmaMetodos(_interfazId, metodo.Id);

        Assert.AreEqual("Nuevo", _interfaz.FirmaMetodos?[0].Nombre);
        _interfazRepositorioMock?.VerifyAll();
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadYaExisteExcepcion))]
    public void AgregarMetodoAFirmaMetodos_CuandoFirmaYaExiste_LanzaExcepcion()
    {
        var tipoDato = new TipoDato("int");
        var metodoExistente = new Metodo("Procesar", tipoDato, [new Parametro { TipoDato = tipoDato }]);
        _interfaz.FirmaMetodos?.Add(metodoExistente);

        _interfazRepositorioMock?
            .Setup(r => r.Obtener(_interfaz.Id))
            .Returns(_interfaz);

        _metodoServicioMock?
            .Setup(s => s.ObtenerMetodoPorId(metodoExistente.Id))
            .Returns(metodoExistente);

        _interfazRepositorioMock?.Setup(r => r.Actualizar(_interfaz));

        var metodoDuplicado = new Metodo("Procesar", tipoDato, [new Parametro { TipoDato = tipoDato }]);

        _metodoServicioMock?
            .Setup(s => s.ObtenerMetodoPorId(metodoDuplicado.Id))
            .Returns(metodoDuplicado);

        _interfazServicio?.AgregarMetodoAFirmaMetodos(_interfazId, metodoDuplicado.Id);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNulaExcepcion))]
    public void AgregarMetodoAFirmaMetodos_CuandoInterfazIdEsVacio_LanzaExcepcion()
    {
        var metodoId = Guid.NewGuid();

        _interfazServicio?.AgregarMetodoAFirmaMetodos(Guid.Empty, metodoId);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNulaExcepcion))]
    public void AgregarMetodoAFirmaMetodos_CuandoMetodoIdEsVacio_LanzaExcepcion()
    {
        var interfazId = Guid.NewGuid();

        _interfazServicio?.AgregarMetodoAFirmaMetodos(interfazId, Guid.Empty);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNulaExcepcion))]
    public void AgregarMetodoAFirmaMetodos_InterfazEsNull_LanzaExcepcion()
    {
        var tipoDato = new TipoDato("int");
        var metodo = new Metodo("Procesar", tipoDato, [new Parametro { TipoDato = tipoDato }]);

        _interfazRepositorioMock?
            .Setup(r => r.Obtener(_interfazId))
            .Returns(((Interfaz?)null)!);

        _metodoServicioMock?
            .Setup(s => s.ObtenerMetodoPorId(metodo.Id))
            .Returns(metodo);

        _interfazServicio?.AgregarMetodoAFirmaMetodos(_interfazId, metodo.Id);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNulaExcepcion))]
    public void AgregarMetodoAFirmaMetodos_MetodoEsNull_LanzaExcepcion()
    {
        var metodoId = Guid.NewGuid();

        _interfazRepositorioMock?
            .Setup(r => r.Obtener(_interfazId))
            .Returns(_interfaz);

        _metodoServicioMock?
            .Setup(s => s.ObtenerMetodoPorId(metodoId))
            .Returns(((Metodo?)null)!);

        _interfazServicio?.AgregarMetodoAFirmaMetodos(_interfazId, metodoId);
    }

    [TestMethod]
    public void ObtenerTodasLasInterfaces_CuandoHayDatos_DebeRetornarListaDeInterfaces()
    {
        var lista = new List<Interfaz>
        {
            new Interfaz { Id = Guid.NewGuid(), Nombre = "Interfaz 1", FirmaMetodos = new List<Metodo>() },
            new Interfaz { Id = Guid.NewGuid(), Nombre = "Interfaz 2", FirmaMetodos = new List<Metodo>() }
        };

        _interfazRepositorioMock?
            .Setup(repo => repo.ObtenerTodos())
            .Returns(lista);

        var resultado = _interfazServicio?.ObtenerTodasLasInterfaces();

        Assert.IsTrue(
            resultado?.Count == 2 &&
            resultado[0].Nombre == "Interfaz 1" &&
            resultado[1].Nombre == "Interfaz 2");
    }

    [TestMethod]
    public void ObtenerTodasLasInterfaces_CuandoNoHayDatos_DebeRetornarListaVacia()
    {
        var listaVacia = new List<Interfaz>();

        _interfazRepositorioMock?
            .Setup(repo => repo.ObtenerTodos())
            .Returns(listaVacia);

        var resultado = _interfazServicio?.ObtenerTodasLasInterfaces();

        Assert.AreEqual(0, resultado?.Count);
    }
}
