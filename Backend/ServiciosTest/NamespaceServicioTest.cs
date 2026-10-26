using AccesoADatos.InterfacesRepositorios;
using Dominio;
using Dominio.Excepciones;
using Moq;
using Servicios;

namespace ServiciosTest;

[TestClass]
public class NamespaceServicioTest
{
    private Mock<INamespaceRepositorio>? _repositorioNamespace;
    private NamespaceServicio? _servicio;

    private Guid _existeId;
    private Namespace? _raizNs;
    private Namespace? _hijoNs;
    private Clase? _unaClase;
    private Interfaz? _unaInterfaz;

    [TestInitialize]
    public void Inicializar()
    {
        _repositorioNamespace = new Mock<INamespaceRepositorio>();
        _servicio = new NamespaceServicio(_repositorioNamespace.Object);

        _existeId = Guid.NewGuid();
        _raizNs = new Namespace("Padre", null) { Id = Guid.NewGuid() };
        _hijoNs = new Namespace("Hijo", _raizNs) { Id = _existeId };

        _unaClase = new Clase();
        _unaInterfaz = new Interfaz();

        _raizNs.Agregar(_hijoNs);

        _repositorioNamespace
            .Setup(r => r.Obtener(_hijoNs.Id))
            .Returns(_hijoNs);
        _repositorioNamespace
            .Setup(r => r.Obtener(_raizNs.Id))
            .Returns(_raizNs);

        _repositorioNamespace
            .Setup(r => r.Actualizar(_raizNs));

        _repositorioNamespace
            .Setup(r => r.ObtenerTodos())
            .Returns(new List<Namespace> { _raizNs! });
    }

    [TestMethod]
    public void Crear_NombreValido_SinPadreTest()
    {
        var nuevoNs = new Namespace("Variable", null);
        _servicio?.Crear(nuevoNs);
        _repositorioNamespace?.Verify(r => r.Agregar(It.Is<Namespace>(n => n.Nombre == "Variable" && n.Padre == null)), Times.Once);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNulaExcepcion))]
    public void Crear_NombreVacio_SinPadreTest()
    {
        var nuevoNs = new Namespace(string.Empty, null);
        _servicio?.Crear(nuevoNs);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNulaExcepcion))]
    public void Crear_NombreNull_SinPadreTest()
    {
        _servicio?.Crear(null);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNoEncontradaExcepcion))]
    public void Crear_PadreNoExisteTest()
    {
        var padreId = Guid.NewGuid();
        var padreFake = new Namespace { Id = padreId, Nombre = "PadreInexistente", Padre = null };
        var noexiste = Guid.NewGuid();
        var nuevoNs = new Namespace { Id = noexiste, Nombre = "domingo", Padre = padreFake };
        _repositorioNamespace?.Setup(r => r.Obtener(padreId)).Returns((Namespace?)null);
        _servicio?.Crear(nuevoNs);
    }

    [TestMethod]
    [ExpectedException(typeof(OperacionNoPermitidaExcepcion))]
    public void Crear_PadreCircularTest()
    {
        var a = new Namespace("A", null) { Id = Guid.NewGuid() };
        var b = new Namespace("B", a) { Id = Guid.NewGuid() };
        a.Padre = b;
        _repositorioNamespace?.Setup(r => r.Obtener(a.Id)).Returns(a);
        _repositorioNamespace?.Setup(r => r.Obtener(b.Id)).Returns(b);
        _servicio?.Crear(a);
    }

    [TestMethod]
    [ExpectedException(typeof(OperacionNoPermitidaExcepcion))]
    public void Crear_AsignoMismoPadreASiMismoTest()
    {
        var mismoPadre = new Namespace("mismo", null) { Id = Guid.NewGuid() };
        mismoPadre.Padre = mismoPadre;
        _repositorioNamespace?.Setup(r => r.Obtener(mismoPadre.Id)).Returns(mismoPadre);
        _servicio?.Crear(mismoPadre);
    }

    [TestMethod]
    public void Crear_ConPadreValido_AsignaciónCorrecta()
    {
        var padreId = Guid.NewGuid();
        var padre = new Namespace("Padre", null) { Id = padreId };
        _repositorioNamespace?.Setup(r => r.Obtener(padreId)).Returns(padre);
        var hijo = new Namespace("Hijo", padre);

        _servicio?.Crear(hijo);
        Assert.AreEqual(padreId, hijo.Padre!.Id);
        _repositorioNamespace?.Verify(r => r.Agregar(hijo), Times.Once);
    }

    [TestMethod]
    [ExpectedException(typeof(OperacionNoPermitidaExcepcion))]
    public void Crear_NamespaceConNombreDuplicadoBajoMismoPadre_DeberiaLanzarOperacionNoPermitida()
    {
        var padreId = Guid.NewGuid();
        var padre = new Namespace("Padre", null) { Id = padreId };
        var hijoExistente = new Namespace("Hijo", padre) { Id = Guid.NewGuid() };
        padre.Agregar(hijoExistente);

        _repositorioNamespace?
            .Setup(r => r.Obtener(padreId))
            .Returns(padre);

        var nuevoHijo = new Namespace("Hijo", padre) { Id = Guid.NewGuid() };

        _servicio?.Crear(nuevoHijo);
    }

    [TestMethod]
    public void Renombrar_Correcto_CambiaNombreYLlamaActualizarTest()
    {
        _servicio?.Renombrar(_existeId, "Otro");
        Assert.AreEqual("Otro", _hijoNs?.Nombre);
        _repositorioNamespace?.Verify(r => r.Actualizar(_hijoNs), Times.Once);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNulaExcepcion))]
    public void Renombrar_CambiaNombreAVacioTest()
    {
        _servicio?.Renombrar(_existeId, string.Empty);
        _repositorioNamespace?.Verify(r => r.Actualizar(_hijoNs), Times.Once);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNoEncontradaExcepcion))]
    public void Renombrar_IdNoExisteTest()
    {
        var fake = Guid.NewGuid();
        _repositorioNamespace?.Setup(r => r.Obtener(fake)).Returns((Namespace?)null);
        _servicio?.Renombrar(fake, "lenovo");
    }

    [TestMethod]
    [ExpectedException(typeof(OperacionNoPermitidaExcepcion))]
    public void Renombrar_AliasADuplicarNombreBajoMismoPadre_DeberiaLanzarOperacionNoPermitida()
    {
        var padre = new Namespace("Padre", null) { Id = Guid.NewGuid() };
        var hijo1 = new Namespace("A", padre) { Id = Guid.NewGuid() };
        var hijo2 = new Namespace("B", padre) { Id = Guid.NewGuid() };
        padre.Agregar(hijo1);
        padre.Agregar(hijo2);

        _repositorioNamespace?
            .Setup(r => r.Obtener(hijo1.Id))
            .Returns(hijo1);
        _repositorioNamespace?
            .Setup(r => r.Obtener(hijo2.Id))
            .Returns(hijo2);
        _repositorioNamespace?
            .Setup(r => r.Obtener(padre.Id))
            .Returns(padre);

        _servicio?.Renombrar(hijo1.Id, "B");
    }

    [TestMethod]
    public void CambiarPadre_Valido_AsignaYActualizarTest()
    {
        var nuevoPadre = new Namespace("Otro", null) { Id = Guid.NewGuid() };
        _repositorioNamespace?.Setup(r => r.Obtener(nuevoPadre.Id)).Returns(nuevoPadre);

        _servicio?.CambiarPadre(_existeId, nuevoPadre.Id);

        Assert.AreSame(nuevoPadre, _hijoNs.Padre);
        _repositorioNamespace?.Verify(r => r.Actualizar(_hijoNs), Times.Once);
    }

    [TestMethod]
    public void CambiarPadre_AsignarNull_QuitarPadreTest()
    {
        _servicio?.CambiarPadre(_existeId, null);
        Assert.IsNull(_hijoNs.Padre);
        _repositorioNamespace?.Verify(r => r.Actualizar(_hijoNs), Times.Once);
    }

    [TestMethod]
    [ExpectedException(typeof(OperacionNoPermitidaExcepcion))]
    public void CambiarPadre_MismoIdTest()
    {
        _servicio?.CambiarPadre(_existeId, _existeId);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNoEncontradaExcepcion))]
    public void CambiarPadre_PadreNoExisteTest()
    {
        var fake = Guid.NewGuid();
        _repositorioNamespace?.Setup(r => r.Obtener(fake)).Returns((Namespace?)null);
        _servicio?.CambiarPadre(_existeId, fake);
    }

    [TestMethod]
    [ExpectedException(typeof(OperacionNoPermitidaExcepcion))]
    public void CambiarPadre_GeneraCicloTest()
    {
        var raiz = new Namespace("A", null) { Id = Guid.NewGuid() };
        var hijo = new Namespace("B", raiz) { Id = Guid.NewGuid() };
        raiz.Padre = hijo;

        _repositorioNamespace?.Setup(r => r.Obtener(raiz.Id)).Returns(raiz);
        _repositorioNamespace?.Setup(r => r.Obtener(hijo.Id)).Returns(hijo);
        _servicio?.CambiarPadre(hijo.Id, raiz.Id);
    }

    [TestMethod]
    [ExpectedException(typeof(OperacionNoPermitidaExcepcion))]
    public void CambiarPadre_AlPadreQueYaTieneHijoConMismoNombre_DeberiaLanzarOperacionNoPermitida()
    {
        var origen = new Namespace("Origen", null) { Id = Guid.NewGuid() };
        var mover = new Namespace("Mover", origen) { Id = Guid.NewGuid() };
        origen.Agregar(mover);

        var destino = new Namespace("Destino", null) { Id = Guid.NewGuid() };
        var clash = new Namespace("Mover", destino) { Id = Guid.NewGuid() };
        destino.Agregar(clash);

        _repositorioNamespace?
            .Setup(r => r.Obtener(mover.Id))
            .Returns(mover);
        _repositorioNamespace?
            .Setup(r => r.Obtener(origen.Id))
            .Returns(origen);
        _repositorioNamespace?
            .Setup(r => r.Obtener(destino.Id))
            .Returns(destino);

        _servicio?.CambiarPadre(mover.Id, destino.Id);
    }

    [TestMethod]
    public void Eliminar_NamespaceSinHijos()
    {
        _repositorioNamespace?.Setup(r => r.ObtenerTodos()).Returns(new List<Namespace> { _hijoNs! });
        _servicio?.Eliminar(_existeId);
        _repositorioNamespace?.Verify(r => r.Eliminar(_hijoNs), Times.Once);
    }

    [TestMethod]
    [ExpectedException(typeof(OperacionNoPermitidaExcepcion))]
    public void Eliminar_NamespaceConHijos()
    {
        _repositorioNamespace?.Setup(r => r.ObtenerTodos()).Returns(new List<Namespace?> { _hijoNs!, _raizNs! });
        _servicio?.Eliminar(_raizNs.Id);
        _repositorioNamespace?.Verify(r => r.Eliminar(It.IsAny<Namespace>()), Times.Never);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNoEncontradaExcepcion))]
    public void Eliminar_IdNoExisteTest()
    {
        var fakeId = Guid.NewGuid();
        _repositorioNamespace?.Setup(r => r.Obtener(fakeId)).Returns((Namespace?)null);
        _repositorioNamespace?.Verify(r => r.Eliminar(It.IsAny<Namespace>()), Times.Never);
        _servicio?.Eliminar(fakeId);
    }

    [TestMethod]
    public void ObtenerTodosLosNamespaceTest()
    {
        _repositorioNamespace?.Setup(repo => repo.ObtenerTodos()).Returns(new List<Namespace>());
        var resultado = _servicio?.ListarTodos();
        Assert.IsNotNull(resultado);
    }

    [TestMethod]
    public void Obtener_ExisteTest()
    {
        var resultado = _servicio?.Obtener(_existeId);
        Assert.AreEqual(_hijoNs?.Nombre, resultado?.Nombre);
        _repositorioNamespace?.Setup(r => r.Obtener(_existeId)).Returns(resultado);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNoEncontradaExcepcion))]
    public void Obtener_NoExisteTest()
    {
        var noExisteId = Guid.NewGuid();
        _repositorioNamespace?.Setup(r => r.Obtener(noExisteId)).Returns((Namespace?)null);
        _servicio?.Obtener(noExisteId);
        _repositorioNamespace?.Verify(r => r.Obtener(noExisteId), Times.Once);
    }

    [TestMethod]
    public void AgregarElemento_DeberiaActualizarRepositorioTest()
    {
        _servicio?.AgregarElemento(_raizNs.Id, _unaClase!);

        Assert.IsTrue(_raizNs.ObtenerElementos().Contains(_unaClase!));
        _repositorioNamespace?.Verify(r => r.Actualizar(_raizNs), Times.Once);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNoEncontradaExcepcion))]
    public void AgregarElemento_NamespaceNoEncontrado_DeberiaLanzarExcepcion()
    {
        var randomId = Guid.NewGuid();
        _repositorioNamespace?
            .Setup(r => r.Obtener(randomId))
            .Returns((Namespace?)null);

        _servicio?.AgregarElemento(randomId, _unaClase!);
        _repositorioNamespace?.Verify(r => r.Actualizar(It.IsAny<Namespace>()), Times.Never);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNulaExcepcion))]
    public void AgregarElemento_ElementoNull_DeberiaLanzarArgumentNullException()
    {
        _servicio?.AgregarElemento(_raizNs.Id, null!);
    }

    [TestMethod]
    [ExpectedException(typeof(OperacionNoPermitidaExcepcion))]
    public void AgregarElemento_ElementoDuplicado_DeberiaLanzarOperacionNoValida()
    {
        _raizNs?.Agregar(_unaClase!);
        _servicio?.AgregarElemento(_raizNs.Id, _unaClase!);
    }

    [TestMethod]
    public void AgregarElemento_DeberiaIncrementarConteoDeElementos()
    {
        var antes = _raizNs.ObtenerElementos().Count;
        _servicio?.AgregarElemento(_raizNs.Id, _unaClase!);
        var despues = _raizNs.ObtenerElementos().Count;
        Assert.AreEqual(antes + 1, despues);
    }

    [TestMethod]
    public void EliminarElemento_DeberiaLlamarActualizarYRemoverElemento()
    {
        _raizNs?.Agregar(_unaInterfaz!);
        _servicio?.EliminarElemento(_raizNs.Id, _unaInterfaz.Id);

        Assert.IsFalse(_raizNs.ObtenerElementos().Contains(_unaInterfaz!));
        _repositorioNamespace?.Verify(r => r.Actualizar(_raizNs), Times.Once);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNoEncontradaExcepcion))]
    public void EliminarElemento_NamespaceNoEncontrado_DeberiaLanzarExcepcion()
    {
        var randomId = Guid.NewGuid();
        _repositorioNamespace?
            .Setup(r => r.Obtener(randomId))
            .Returns((Namespace?)null);

        _servicio?.EliminarElemento(randomId, Guid.NewGuid());
        _repositorioNamespace?.Verify(r => r.Actualizar(It.IsAny<Namespace>()), Times.Never);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNoEncontradaExcepcion))]
    public void EliminarElemento_ElementoNoEncontrado_DeberiaLanzarExcepcion()
    {
        var noEncontrado = Guid.NewGuid();
        _servicio?.EliminarElemento(_raizNs.Id, noEncontrado);
        _repositorioNamespace?.Verify(r => r.Actualizar(It.IsAny<Namespace>()), Times.Never);
    }

    [TestMethod]
    public void ObtenerElementoEnArbol_CuandoExisteEnHijo_RetornaCorrecto()
    {
        var encontrado = _servicio?.ObtenerElementoEnArbol(_existeId);
        Assert.AreSame(_hijoNs, encontrado);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNoEncontradaExcepcion))]
    public void ObtenerElementoEnArbol_CuandoNoExiste_LanzaExcepcion()
    {
        _servicio?.ObtenerElementoEnArbol(Guid.NewGuid());
    }

    [TestMethod]
    public void AgregarClase_DeberiaAgregarYActualizar()
    {
        _servicio?.AgregarClase(_raizNs.Id, _unaClase!);
        Assert.IsTrue(_raizNs.ObtenerElementos().Contains(_unaClase!));
        _repositorioNamespace?.Verify(r => r.Actualizar(_raizNs), Times.Once);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNoEncontradaExcepcion))]
    public void AgregarClase_NamespaceNoEncontrado_DeberiaLanzarExcepcion()
    {
        var randomId = Guid.NewGuid();
        _repositorioNamespace?
            .Setup(r => r.Obtener(randomId))
            .Returns((Namespace?)null);

        _servicio?.AgregarClase(randomId, _unaClase!);

        _repositorioNamespace?.Verify(r => r.Actualizar(It.IsAny<Namespace>()), Times.Never);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNulaExcepcion))]
    public void AgregarClase_ClaseNull_DeberiaLanzarEntidadNula()
    {
        _servicio?.AgregarClase(_raizNs.Id, null!);
        _repositorioNamespace?.Verify(r => r.Actualizar(It.IsAny<Namespace>()), Times.Never);
    }

    [TestMethod]
    public void AgregarInterfaz_DeberiaAgregarYActualizar()
    {
        _servicio?.AgregarInterfaz(_raizNs.Id, _unaInterfaz!);

        Assert.IsTrue(_raizNs.ObtenerElementos().Contains(_unaInterfaz!));
        _repositorioNamespace?.Verify(r => r.Actualizar(_raizNs), Times.Once);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNulaExcepcion))]
    public void AgregarInterfazNula_DeberiaLanzarEntidadNula()
    {
        _servicio?.AgregarInterfaz(_raizNs.Id, null!);
        _repositorioNamespace?.Verify(r => r.Actualizar(It.IsAny<Namespace>()), Times.Never);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNoEncontradaExcepcion))]
    public void AgregarInterfaz_NamespaceNoEncontrado_DeberiaLanzarExcepcion()
    {
        var randomId = Guid.NewGuid();
        _repositorioNamespace?
            .Setup(r => r.Obtener(randomId))
            .Returns((Namespace?)null);

        _servicio?.AgregarInterfaz(randomId, _unaInterfaz!);

        _repositorioNamespace?.Verify(r => r.Actualizar(It.IsAny<Namespace>()), Times.Never);
    }

    [TestMethod]
    public void ObtenerHijos_DeberiaDevolverSoloElementosDirectos()
    {
        _raizNs?.Agregar(_unaClase!);
        _raizNs?.Agregar(_unaInterfaz!);

        _repositorioNamespace?
            .Setup(r => r.Obtener(_raizNs.Id))
            .Returns(_raizNs);

        var hijos = _servicio?.ObtenerHijos(_raizNs.Id).ToList();

        CollectionAssert.AreEquivalent(
            new List<Elemento> { _hijoNs!, _unaClase!, _unaInterfaz! },
            hijos);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNoEncontradaExcepcion))]
    public void ObtenerHijos_NamespaceNoExiste_DeberiaLanzarExcepcion()
    {
        var fakeId = Guid.NewGuid();
        _repositorioNamespace?
            .Setup(r => r.Obtener(fakeId))
            .Returns((Namespace?)null);

        _servicio?.ObtenerHijos(fakeId);
    }

    [TestMethod]
    public void ObtenerArbolCompleto_Vacio_RetornaColeccionVacia()
    {
        _repositorioNamespace?
            .Setup(r => r.ObtenerTodos())
            .Returns(new List<Namespace>());

        var arbol = _servicio?.ObtenerArbolCompleto().ToList();
        Assert.AreEqual(0, arbol.Count);
    }

    [TestMethod]
    public void ObtenerArbolCompleto_ConHijos_RetornaSoloTest()
    {
        var arbol = _servicio?.ObtenerArbolCompleto().ToList();

        Assert.AreEqual(1, arbol.Count);
        var root = arbol[0];
        Assert.AreSame(_raizNs, root);

        var hijosNs = root.ObtenerElementos().OfType<Namespace>().ToList();
        Assert.AreEqual(1, hijosNs.Count);
        Assert.AreSame(_hijoNs, hijosNs[0]);
    }
}
