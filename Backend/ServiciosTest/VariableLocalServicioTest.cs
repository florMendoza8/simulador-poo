using AccesoADatos.InterfacesRepositorios;
using Dominio;
using Dominio.Excepciones;
using Moq;
using Servicios;
using Servicios.InterfacesServicios;

namespace ServiciosTest;

[TestClass]
public class VariableLocalServicioTest
{
    private Mock<IVariableLocalRepositorio>? _repositorioVariable;
    private Mock<ITipoDatoServicio>? _tipoDatoServicio;
    private VariableLocalServicio? _servicio;

    [TestInitialize]
    public void Inicializar()
    {
        _tipoDatoServicio = new Mock<ITipoDatoServicio>();
        _repositorioVariable = new Mock<IVariableLocalRepositorio>();
        _servicio = new VariableLocalServicio(_repositorioVariable.Object, _tipoDatoServicio.Object);
    }

    [TestMethod]
    public void AgregarVariableTest()
    {
        var tipo = new TipoDato();
        var agregarVariable = new VariableLocal("nueva", tipo,tipo, "25");
        _tipoDatoServicio!
            .Setup(s => s.EsCompatible(tipo.Id, tipo.Id))
            .Returns(true);
        _repositorioVariable.Setup(r => r.Agregar(It.IsAny<VariableLocal>()));
        var variable = _servicio!.Agregar(agregarVariable);
        _repositorioVariable.VerifyAll();
        Assert.AreEqual(agregarVariable.Nombre, variable.Nombre);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNulaExcepcion))]
    public void NombreVariableNoVacioTest()
    {
        var variable = new VariableLocal(string.Empty, new TipoDato(),new TipoDato(), "25");
        _servicio!.Agregar(variable);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNulaExcepcion))]
    public void NombreVariableNuloTest()
    {
        var variable = new VariableLocal(null!, new TipoDato(), new TipoDato(), "25");
        _servicio!.Agregar(variable);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNulaExcepcion))]
    public void TipoDatoNoNulo()
    {
        var variable = new VariableLocal("nombre", null, new TipoDato(), "25");
        _servicio!.Agregar(variable);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNulaExcepcion))]
    public void Agregar_CuandoTipoDatoIdEsEmpty_LanzaExcepcion()
    {
        var variable = new VariableLocal("nombre", new TipoDato { Id = Guid.Empty }, new TipoDato(), "25");
        _servicio!.Agregar(variable);
    }

    [TestMethod]
    public void ObtenerVariableLocalPorId_Valida_DebeDevolverla()
    {
        var variable = new VariableLocal("nombre", new TipoDato(), new TipoDato(), "25");
        _repositorioVariable!
            .Setup(repo => repo.ObtenerPorId(variable.Id))
            .Returns(variable);

        var resultado = _servicio!.ObtenerVariableLocalPorId(variable.Id);

        Assert.AreEqual(variable.Nombre, resultado.Nombre);
        _repositorioVariable.Verify(r => r.ObtenerPorId(variable.Id), Times.Once);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNoEncontradaExcepcion))]
    public void ObtenerVariableLocalPorId_VariableNula_DebeLanzarExcepcion()
    {
        var id = Guid.NewGuid();
        _repositorioVariable!
            .Setup(repo => repo.ObtenerPorId(id))
            .Returns<VariableLocal>(null);

        _servicio!.ObtenerVariableLocalPorId(id);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNulaExcepcion))]
    public void ObtenerVariableLocalPorId_IdVacio_DebeLanzarExcepcion()
    {
        _servicio!.ObtenerVariableLocalPorId(Guid.Empty);
    }

    [TestMethod]
    public void ObtenerTodasLasVariablesLocales_CuandoHayDatos_DebeRetornarListaDeInterfaces()
    {
        var lista = new List<VariableLocal>
        {
            new VariableLocal { Id = Guid.NewGuid(), Nombre = "VariableLocal 1", TipoInstancia = new TipoDato(), TipoReferencia = new TipoDato(), Valor = "valor 1" },
            new VariableLocal { Id = Guid.NewGuid(), Nombre = "VariableLocal 2", TipoInstancia = new TipoDato(), TipoReferencia = new TipoDato(), Valor = "valor 2" }
        };

        _repositorioVariable?
            .Setup(repo => repo.ObtenerTodos())
            .Returns(lista);

        var resultado = _servicio?.ObtenerTodasLasVariablesLocales();

        Assert.IsTrue(
            resultado?.Count == 2 &&
            resultado[0].Nombre == "VariableLocal 1" &&
            resultado[1].Nombre == "VariableLocal 2");
    }

    [TestMethod]
    public void ObtenerTodasLasVariablesLocales_CuandoNoHayDatos_DebeRetornarListaVacia()
    {
        var listaVacia = new List<VariableLocal>();

        _repositorioVariable?
            .Setup(repo => repo.ObtenerTodos())
            .Returns(listaVacia);

        var resultado = _servicio?.ObtenerTodasLasVariablesLocales();

        Assert.AreEqual(0, resultado?.Count);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNulaExcepcion))]
    public void Agregar_TipoInstanciaVacioTest()
    {
        var referencia = new TipoDato();
        var variable = new VariableLocal("v", referencia, new TipoDato { Id = Guid.Empty }, "valor");
        _servicio!.Agregar(variable);
    }

    [TestMethod]
    [ExpectedException(typeof(OperacionNoPermitidaExcepcion))]
    public void Agregar_TiposNoCompatiblesTest()
    {
        var refId = Guid.NewGuid();
        var instId = Guid.NewGuid();
        var tipoRef = new TipoDato { Id = refId, Nombre = "A", TipoPadre = null };
        var tipoInst = new TipoDato { Id = instId, Nombre = "B", TipoPadre = null };

        var variable = new VariableLocal("v", tipoRef, tipoInst, "123");
        _tipoDatoServicio.Setup(s => s.EsCompatible(refId, instId)).Returns(false);
        _servicio?.Agregar(variable);
    }

    [TestMethod]
    public void Agregar_TipoCompatibleTest()
    {
        var refId = Guid.NewGuid();
        var instId = Guid.NewGuid();
        var tipoRef = new TipoDato { Id = refId, Nombre = "A", TipoPadre = null };
        var tipoInst = new TipoDato { Id = instId, Nombre = "B", TipoPadre = tipoRef };

        var variable = new VariableLocal("v", tipoRef, tipoInst, "123");
        _tipoDatoServicio?.Setup(s => s.EsCompatible(refId, instId)).Returns(true);
        _tipoDatoServicio?.Setup(s => s.ObtenerTipoDatoPoId(refId))
            .Returns(tipoRef);
        _tipoDatoServicio?.Setup(s => s.ObtenerTipoDatoPoId(instId))
            .Returns(tipoInst);
        _servicio?.Agregar(variable);
    }
}
