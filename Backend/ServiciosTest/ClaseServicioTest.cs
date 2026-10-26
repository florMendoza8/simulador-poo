using System.Reflection;
using AccesoADatos.InterfacesRepositorios;
using Dominio;
using Dominio.Excepciones;
using Moq;
using Servicios;
using Servicios.InterfacesServicios;

namespace ServiciosTest;

[TestClass]
public class ClaseServicioTest
{
    private Mock<IClaseRepositorio>? _repositorioClase;
    private ClaseServicio? _servicio;
    private Mock<IAtributoServicio>? _atributoServicio;
    private Mock<IMetodoServicio>? _metodoServicio;
    private Mock<ITipoDatoServicio>? _tipoDatoServicio;
    private Clase? _base;
    private Clase? _derivada;
    private Atributo? _atributo;

    [TestInitialize]
    public void Inicializar()
    {
        _repositorioClase = new Mock<IClaseRepositorio>();
        _atributoServicio = new Mock<IAtributoServicio>();
        _metodoServicio = new Mock<IMetodoServicio>();
        _tipoDatoServicio = new Mock<ITipoDatoServicio>();
        _servicio = new ClaseServicio(_repositorioClase.Object, _atributoServicio.Object, _metodoServicio.Object, _tipoDatoServicio.Object);

        _base = new Clase { Id = Guid.NewGuid(), Nombre = "Primera" };
        _derivada = new Clase { Id = Guid.NewGuid(), Nombre = "Segunda", ClaseBase = _base , ClaseBaseId = _base.Id};

        _base.MetodosInvocados = new List<Metodo>();
        _derivada.MetodosInvocados = new List<Metodo>();
        _base.Atributos = new List<Atributo>();
        _derivada.Atributos = new List<Atributo>();

        _repositorioClase
            .Setup(repo => repo.ObtenerClase(It.Is<Guid>(id => id == _base.Id)))
            .Returns(_base);

        _repositorioClase
            .Setup(repo => repo.ObtenerClase(It.Is<Guid>(id => id == _derivada.Id)))
            .Returns(_derivada);

        _tipoDatoServicio
            .Setup(s => s.ObtenerTipoDatoPoId(It.IsAny<Guid>()))
            .Returns<Guid>(id => new TipoDato { Id = id, Nombre = "TipoGenerico" });

         _atributo = new Atributo { Id = Guid.NewGuid(), Nombre = "X" };
         _atributoServicio.Setup(repo => repo.ObtenerAtributoPorId(_atributo.Id)).Returns(_atributo);
    }

    [TestMethod]
    public void CrearClaseDatosValidosTest()
    {
        var claseBase = new Clase();
        var nuevaClase = new Clase("Variable", false, false, new List<Atributo>(), new List<Metodo>(), claseBase, claseBase.Id, new List<Interfaz>());
        _repositorioClase?.Setup(repo => repo.Existe(It.IsAny<Clase>())).Returns(false);
        _repositorioClase?.Setup(r => r.ObtenerClase(claseBase.Id)).Returns(claseBase);
        _servicio?.CrearClase(nuevaClase);
        _repositorioClase?.Verify(repo => repo.Agregar(It.IsAny<Clase>()), Times.Once);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNulaExcepcion))]
    public void CrearClaseNoNullTest()
    {
        _servicio?.CrearClase(null);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNulaExcepcion))]
    public void CrearClaseNombreNoVacioTest()
    {
        var nueva = new Clase { Nombre = " " };
        _servicio?.CrearClase(nueva);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadYaExisteExcepcion))]
    public void CrearClaseYaExisteTest()
    {
        var nueva = new Clase { Nombre = "Variable" };
        _repositorioClase?.Setup(repo => repo.Existe(It.IsAny<Clase>())).Returns(true);
        _servicio?.CrearClase(nueva);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNoEncontradaExcepcion))]
    public void CrearClase_ClaseBaseNoExisteTest()
    {
        var baseClase = new Clase { Nombre = "PadreInexistente" };
        var clase = new Clase { Id = Guid.NewGuid(), Nombre = "Sorpresa", ClaseBase = baseClase };
        _repositorioClase?.Setup(r => r.ObtenerClase(clase.ClaseBase.Id)).Returns((Clase)null!);
        _tipoDatoServicio?.Setup(s => s.ObtenerTodosLosTipoDatos()).Returns(new List<TipoDato>());
        _servicio?.CrearClase(clase);

        _repositorioClase?.Verify(r => r.Agregar(It.IsAny<Clase>()), Times.Never);
        _tipoDatoServicio?.Verify(s => s.CrearTipoDato(It.IsAny<TipoDato>()), Times.Never);
    }

    [TestMethod]
    public void CrearClase_ConClaseBaseExistente_RegistrarTipoConPadreCorrectoTest()
    {
        var baseClase = new Clase
        {
            Id = Guid.NewGuid(),
            Nombre = "Base",
            Atributos = [],
            MetodosInvocados = []
        };
        var clase = new Clase
        {
            Id = Guid.NewGuid(), Nombre = "Sorpresa", ClaseBase = baseClase, ClaseBaseId = baseClase.Id, Atributos = [],
            MetodosInvocados = []
        };
        _repositorioClase?.Setup(r => r.ObtenerClase(clase.Id)).Returns(clase);
        _repositorioClase?.Setup(r => r.ObtenerClase(baseClase.Id)).Returns(baseClase);

        var tipoPadre = new TipoDato("Base", null) { Id = Guid.NewGuid() };
        _tipoDatoServicio?.Setup(s => s.CrearTipoDato(
            It.Is<TipoDato>(t =>
                t.Nombre == "Sorpresa" &&
                t.TipoPadre == tipoPadre)));

        _servicio?.CrearClase(clase);

        _repositorioClase?.Verify(r => r.Agregar(clase), Times.Once);
        _repositorioClase?.Verify(r => r.ObtenerClase(baseClase.Id), Times.AtLeastOnce);
        _tipoDatoServicio?.Verify(s => s.CrearTipoDato(It.IsAny<TipoDato>()), Times.Once);
    }

    [TestMethod]
    public void CrearClase_SinClaseBase_TipoPadreNullTest()
    {
        var clase = new Clase
        {
            Nombre = "MiClase",
            ClaseBaseId = null,
            ClaseBase = null,
            Atributos = new List<Atributo>(),
            MetodosInvocados = new List<Metodo>()
        };
        _repositorioClase?.Setup(r => r.Agregar(clase));
        _tipoDatoServicio?.Setup(s => s.CrearTipoDato(It.Is<TipoDato>(t =>
            t.Nombre == "MiClase" &&
            t.TipoPadre == null)));

        _servicio?.CrearClase(clase);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNulaExcepcion))]
    public void CrearClase_HerenciaCircularLanzaExcepcionTest()
    {
        var clase = new Clase { Id = Guid.NewGuid() };
        clase.ClaseBase = clase;
        _repositorioClase?.Setup(r => r.ObtenerClase(clase.Id)).Returns(clase);
        _servicio?.CrearClase(clase);
    }

    [TestMethod]
    public void CrearClase_HerenciaUnNivelTest()
    {
        var claseBase = new Clase { Id = Guid.NewGuid(), Nombre = "prueba", ClaseBase = null };
        var derivado = new Clase { Id = Guid.NewGuid(), Nombre = "prueba", ClaseBase = claseBase, ClaseBaseId = claseBase.Id };
        _repositorioClase?.Setup(r => r.ObtenerClase(derivado.Id)).Returns(derivado);
        _repositorioClase?.Setup(r => r.ObtenerClase(claseBase.Id)).Returns(claseBase);
        _servicio?.CrearClase(derivado);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNulaExcepcion))]
    public void CrearClase_ClaseNoImplementaMetodosAbstractosTest()
    {
        var clase_uno = new Clase
        {
            Id = Guid.NewGuid(), EsAbstract = true, MetodosInvocados = new List<Metodo>
            {
                new Metodo { Nombre = "M", EsAbstract = true, Parametros = new List<Parametro>() }
            }
        };
        var clase_dos = new Clase { Id = Guid.NewGuid(), EsAbstract = false, ClaseBase = clase_uno, MetodosInvocados = new List<Metodo>() };
        _repositorioClase?.Setup(repo => repo.ObtenerClase(clase_dos.Id)).Returns(clase_dos);
        _repositorioClase?.Setup(repo => repo.ObtenerClase(clase_uno.Id)).Returns(clase_uno);
        _servicio?.CrearClase(clase_dos);
    }

    [TestMethod]
    public void CrearClase_ClaseImplementaMetodosAbstractosTest()
    {
        var clase_uno = new Clase
        {
            Id = Guid.NewGuid(), Nombre = "nueva", EsAbstract = true, MetodosInvocados = new List<Metodo>
            {
                new Metodo { Nombre = "M", EsAbstract = true, Parametros = new List<Parametro>() }
            }
        };
        var implementoMetodos = new Metodo { Nombre = "M", EsAbstract = false, Parametros = new List<Parametro>() };
        var clase_dos = new Clase { Id = Guid.NewGuid(), Nombre = "nuevados", EsAbstract = false, ClaseBase = clase_uno, ClaseBaseId = clase_uno.Id, MetodosInvocados = new List<Metodo> { implementoMetodos } };
        _repositorioClase?.Setup(repo => repo.ObtenerClase(clase_dos.Id)).Returns(clase_dos);
        _repositorioClase?.Setup(repo => repo.ObtenerClase(clase_uno.Id)).Returns(clase_uno);

        _tipoDatoServicio
            .Setup(s => s.ObtenerTipoDatoPoId(It.Is<Guid>(g => g == clase_uno.Id)))
            .Returns(new TipoDato { Id = clase_uno.Id, Nombre = "TipoDeNueva" });

        _servicio?.CrearClase(clase_dos);
        _repositorioClase?.Verify(repo => repo.Agregar(clase_dos), Times.Once);
    }

    [TestMethod]
    public void CrearClase_ClaseAbstractaNoImplementaAbstractosTest()
    {
        var clase_uno = new Clase
        {
            Id = Guid.NewGuid(), Nombre = "nueva", EsAbstract = true, MetodosInvocados = new List<Metodo>
            {
                new Metodo { Nombre = "M", EsAbstract = true, Parametros = new List<Parametro>() }
            }
        };
        var clase_dos = new Clase { Id = Guid.NewGuid(), Nombre = "nuevados", EsAbstract = true, ClaseBase = clase_uno, ClaseBaseId = clase_uno.Id, MetodosInvocados = new List<Metodo>() };
        _repositorioClase?.Setup(repo => repo.ObtenerClase(clase_dos.Id)).Returns(clase_dos);
        _repositorioClase?.Setup(repo => repo.ObtenerClase(clase_uno.Id)).Returns(clase_uno);

        _tipoDatoServicio
            .Setup(s => s.ObtenerTipoDatoPoId(It.Is<Guid>(g => g == clase_uno.Id)))
            .Returns(new TipoDato { Id = clase_uno.Id, Nombre = "TipoDeNueva" });

        _servicio?.CrearClase(clase_dos);
        _repositorioClase?.Verify(repo => repo.Agregar(clase_dos), Times.Once);
    }

    [TestMethod]
    public void CrearClase_AtributoNoNullTest()
    {
        var clase = new Clase { Nombre = "MiClase", ClaseBase = _base, ClaseBaseId = _base.Id, Atributos = new List<Atributo>(), MetodosInvocados = new List<Metodo> { new Metodo() } };
        _servicio?.CrearClase(clase);
    }

    [TestMethod]
    public void CrearClase_MetodoNoNullTest()
    {
        var clase = new Clase { Nombre = "MiClase", ClaseBase = _base, ClaseBaseId = _base.Id, Atributos = new List<Atributo>(), MetodosInvocados = new List<Metodo> { new Metodo() } };
        _servicio?.CrearClase(clase);
    }

    [TestMethod]
    public void ModificarClaseDatosValidosTest()
    {
        var claseBase = new Clase();
        var claseExistente = new Clase { Id = Guid.NewGuid(), Nombre = "Existente", ClaseBase = claseBase };

        _repositorioClase?.Setup(repo => repo.ObtenerClase(claseExistente.Id)).Returns(claseExistente);
       _repositorioClase?.Setup(r => r.ObtenerClase(claseBase.Id)).Returns(claseBase);
        _servicio?.ModificarClase(claseExistente);
        _repositorioClase?.Verify(repo => repo.Actualizar(It.IsAny<Clase>()), Times.Once);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNoEncontradaExcepcion))]
    public void ModificarClaseNoExisteTest()
    {
        var clase = new Clase { Id = Guid.NewGuid() };
        _repositorioClase?.Setup(repo => repo.ObtenerClase(clase.Id)).Returns((Clase)null!);
        _servicio?.ModificarClase(clase);
    }

    [TestMethod]
    [ExpectedException(typeof(OperacionNoPermitidaExcepcion))]
    public void CrearClase_HeredaDeClaseSealed_LanzaExcepcion()
    {
        var claseBase = new Clase("Base", false, true, [], [], null, null, []);
        var nuevaClase = new Clase("Derivada", false, false, [], [], claseBase, null, []);

        _repositorioClase?.Setup(r => r.ObtenerClase(claseBase.Id)).Returns(claseBase);
        _repositorioClase?.Setup(r => r.Existe(It.IsAny<Clase>())).Returns(false);

        _servicio?.CrearClase(nuevaClase);
    }

    [TestMethod]
    public void EliminarClaseCorrectoTest()
    {
        var clase = new Clase { Id = Guid.NewGuid() };
        _repositorioClase?.Setup(repo => repo.ObtenerClase(clase.Id)).Returns(clase);
        _repositorioClase?.Setup(repo => repo.ObtenerTodasLasClases()).Returns(new List<Clase?> { clase });
        _servicio?.EliminarClase(clase);
        _repositorioClase?.Verify(repo => repo.EliminarClase(It.IsAny<Clase>()), Times.Once);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNoEncontradaExcepcion))]
    public void EliminarClaseNoExisteTest()
    {
        var clase = new Clase { Id = Guid.NewGuid() };
        _repositorioClase?.Setup(repo => repo.ObtenerClase(clase.Id)).Returns((Clase)null!);
        _servicio?.EliminarClase(clase);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNoEncontradaExcepcion))]
    public void EliminarClaseConClaseDerivadaTest()
    {
        var idPadre = Guid.NewGuid();
        var clasePadre = new Clase { Id = idPadre, ClaseBase = null };
        var claseHija = new Clase { Id = Guid.NewGuid(), ClaseBase = clasePadre };
        _repositorioClase?.Setup(r => r.ObtenerTodasLasClases()).Returns(new List<Clase?>() { clasePadre, claseHija });
        _servicio?.EliminarClase(clasePadre);
    }

    [TestMethod]
    public void EliminarClaseSinClaseDerivadaTest()
    {
        var clase = new Clase { Id = Guid.NewGuid(), ClaseBase = null };
        _repositorioClase?.Setup(r => r.ObtenerClase(clase.Id)).Returns(clase );
        _repositorioClase?.Setup(r => r.ObtenerTodasLasClases()).Returns(new List<Clase?>() { clase });
        _servicio?.EliminarClase(clase);
        _repositorioClase?.Verify(r => r.EliminarClase(clase));
    }

    [TestMethod]
    public void ObtenerClaseQueExisteTest()
    {
        var clase = new Clase { Id = Guid.NewGuid() };
        _repositorioClase?.Setup(repo => repo.ObtenerClase(clase.Id)).Returns(clase);
        var resultado = _servicio?.ObtenerClasePorId(clase.Id);
        Assert.IsNotNull(resultado);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNoEncontradaExcepcion))]
    public void ObtenerClaseQueNoExisteTest()
    {
        var clase = new Clase { Id = Guid.NewGuid() };
        _repositorioClase?.Setup(repo => repo.ObtenerClase(clase.Id)).Returns((Clase)null!);
        _servicio?.ObtenerClasePorId(clase.Id);
    }

    [TestMethod]
    public void ObtenerTodasLasClasesTest()
    {
        _repositorioClase?.Setup(repo => repo.ObtenerTodasLasClases()).Returns(new List<Clase>());
        var resultado = _servicio?.ObtenerTodasLasClases();
        Assert.IsNotNull(resultado);
    }

    [TestMethod]
    public void AgregarAtributoTest()
    {
        var claseExistente = new Clase { Id = Guid.NewGuid(), Atributos = new List<Atributo>() };
        var atributoNuevo = new Atributo { Id = Guid.NewGuid(), Nombre = "Edad" };
        _repositorioClase?.Setup(repo => repo.ObtenerClase(claseExistente.Id)).Returns(claseExistente);
        _atributoServicio?.Setup(repo => repo.ObtenerAtributoPorId(atributoNuevo.Id)).Returns(atributoNuevo);
        _servicio?.AgregarAtributo(claseExistente.Id, atributoNuevo.Id);

        Assert.AreEqual("Edad", claseExistente.Atributos.First().Nombre);
        _repositorioClase?.Verify(r => r.Actualizar(It.IsAny<Clase>()), Times.Once);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNoEncontradaExcepcion))]
    public void AgregarAtributoClaseNoExisteTest()
    {
        var claseExistente = new Clase { Id = Guid.NewGuid(), Atributos = new List<Atributo>() };
        var atributoNuevo = new Atributo { Id = Guid.NewGuid(), Nombre = "Edad" };
        _repositorioClase?.Setup(repo => repo.ObtenerClase(claseExistente.Id)).Returns((Clase)null!);
        _servicio?.AgregarAtributo(claseExistente.Id, atributoNuevo.Id);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNoEncontradaExcepcion))]
    public void AgregarAtributoNoExisteTest()
    {
        var claseExistente = new Clase { Id = Guid.NewGuid(), Atributos = new List<Atributo>() };
        var atributoNuevo = new Atributo { Id = Guid.NewGuid(), Nombre = "Edad" };
        _repositorioClase?.Setup(repo => repo.ObtenerClase(claseExistente.Id)).Returns(claseExistente);
        _atributoServicio?.Setup(repo => repo.ObtenerAtributoPorId(atributoNuevo.Id)).Returns((Atributo)null!);
        _servicio?.AgregarAtributo(claseExistente.Id, atributoNuevo.Id);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadYaExisteExcepcion))]
    public void AgregarAtributoDuplicadoTest()
    {
        var atributoNuevo = new Atributo { Id = Guid.NewGuid(), Nombre = "Edad" };
        var clase = new Clase
        {
            Id = Guid.NewGuid(),
            Atributos = new List<Atributo>
            {
                new Atributo { Nombre = "Edad" }
            }
        };
        _repositorioClase?.Setup(repo => repo.ObtenerClase(clase.Id)).Returns(clase);
        _atributoServicio?.Setup(repo => repo.ObtenerAtributoPorId(atributoNuevo.Id)).Returns(atributoNuevo);
        _servicio?.AgregarAtributo(clase.Id, atributoNuevo.Id);
    }

    [TestMethod]
    public void AgregarMetodoAClaseTest()
    {
        var claseExistente = new Clase { Id = Guid.NewGuid(), MetodosInvocados = new List<Metodo>() };
        var metodo = new Metodo { Id = Guid.NewGuid(), Nombre = "CalcularEdad", Parametros = new List<Parametro>() };
        _repositorioClase?.Setup(repo => repo.ObtenerClase(claseExistente.Id)).Returns(claseExistente);
        _metodoServicio?.Setup(repo => repo.ObtenerMetodoPorId(metodo.Id)).Returns(metodo);

        _servicio?.AgregarMetodo(claseExistente.Id, metodo.Id);
        Assert.AreEqual("CalcularEdad", claseExistente.MetodosInvocados.First().Nombre);
        _repositorioClase?.Verify(r => r.Actualizar(It.IsAny<Clase>()), Times.Once);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNoEncontradaExcepcion))]
    public void AgregarMetodoClaseNoExisteTest()
    {
        var claseExistente = new Clase { Id = Guid.NewGuid(), MetodosInvocados = new List<Metodo>() };
        var metodo = new Metodo { Id = Guid.NewGuid(), Nombre = "Calcular", Parametros = new List<Parametro>() };
        _repositorioClase?.Setup(repo => repo.ObtenerClase(claseExistente.Id)).Returns((Clase)null!);
        _servicio?.AgregarMetodo(claseExistente.Id, metodo.Id);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNoEncontradaExcepcion))]
    public void AgregarMetodoNoExisteTest()
    {
        var claseExistente = new Clase { Id = Guid.NewGuid(), MetodosInvocados = new List<Metodo>() };
        var metodo = new Metodo { Id = Guid.NewGuid(), Nombre = "Calcular", Parametros = new List<Parametro>() };
        _repositorioClase?.Setup(repo => repo.ObtenerClase(claseExistente.Id)).Returns(claseExistente);
        _metodoServicio?.Setup(repo => repo.ObtenerMetodoPorId(metodo.Id)).Returns((Metodo)null!);

        _servicio?.AgregarMetodo(claseExistente.Id, metodo.Id);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadYaExisteExcepcion))]
    public void AgregarMetodoDuplicadoEnMiMismaClaseTest()
    {
        var idMetodo = Guid.NewGuid();
        var tipoDato = new TipoDato("int");
        var parametro = new List<Parametro> { new Parametro { TipoDato = tipoDato } };

        var metodoExistente = new Metodo
        {
            Id = idMetodo,
            Nombre = "CalcularEdad",
            Parametros = parametro
        };

        var metodoDuplicado = new Metodo
        {
            Id = idMetodo,
            Nombre = "CalcularEdad",
            Parametros = parametro
        };

        var claseExistente = new Clase { Id = Guid.NewGuid(), MetodosInvocados = new List<Metodo> { metodoExistente } };
        _repositorioClase?.Setup(repo => repo.ObtenerClase(claseExistente.Id)).Returns(claseExistente);
        _metodoServicio?.Setup(repo => repo.ObtenerMetodoPorId(metodoDuplicado.Id)).Returns(metodoDuplicado);

        _servicio?.AgregarMetodo(claseExistente.Id, metodoDuplicado.Id);
    }

    [TestMethod]
    public void AgregarMetodoAbtsracto_ClaseSeVuelveAbstractaTest()
    {
        var clase = new Clase
        {
            Id = Guid.NewGuid(),
            Nombre = "ClaseConcreta",
            EsAbstract = false,
            MetodosInvocados = []
        };

        var metodoAbstracto = new Metodo
        {
            Id = Guid.NewGuid(),
            Nombre = "MetodoAbstracto",
            EsAbstract = true
        };
        _repositorioClase?.Setup(repo => repo.ObtenerClase(clase.Id)).Returns(clase);
        _metodoServicio?.Setup(repo => repo.ObtenerMetodoPorId(metodoAbstracto.Id)).Returns(metodoAbstracto);

        _servicio?.AgregarMetodo(clase.Id, metodoAbstracto.Id);

        Assert.IsTrue(clase.EsAbstract, "La clase debería haberse marcado como abstracta.");
    }

    [TestMethod]
    public void AgregarAtributo_NoExisteHerenciaTest()
    {
        _servicio.AgregarAtributo(_derivada.Id, _atributo.Id);
        Assert.IsTrue(_derivada.Atributos.Contains(_atributo));
        _repositorioClase?.Verify(repo => repo.Actualizar(_derivada), Times.Once);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadYaExisteExcepcion))]
    public void AgregarAtributo_ExisteHerenciaTest()
    {
        _base?.Atributos.Add(_atributo!);

        _servicio?.AgregarAtributo(_derivada.Id, _atributo.Id);
    }

    [TestMethod]
    public void AgregarMetodoOverrideTest()
    {
        var metodoBase = new Metodo { Id = Guid.NewGuid(), Nombre = "Funcion", EsVirtual = true };
        _base?.MetodosInvocados.Add(metodoBase);

        var metodoOverride = new Metodo
        {
            Id = Guid.NewGuid(),
            Nombre = "Funcion",
            EsOverride = true,
            EsAbstract = false,
            Parametros = new List<Parametro>(),
            TipoRetorno = new TipoDato("void", null)
        };

        metodoBase.Parametros = new List<Parametro>();
        metodoBase.TipoRetorno = metodoOverride.TipoRetorno;

        _metodoServicio?.Setup(m => m.ObtenerMetodoPorId(metodoOverride.Id)).Returns(metodoOverride);

        _servicio?.AgregarMetodo(_derivada!.Id, metodoOverride.Id);

        Assert.IsTrue(_derivada!.MetodosInvocados.Contains(metodoOverride));
    }

    [TestMethod]
    public void AgregarMetodo_NuevoMetodo_NoLanzaYLoAgrega()
    {
        var metodo = new Metodo { Id = Guid.NewGuid(), Nombre = "NuevaFunc", EsAbstract = false };

        _metodoServicio?.Setup(r => r.ObtenerMetodoPorId(metodo.Id)).Returns(metodo);

        Assert.IsFalse(_base?.MetodosInvocados.Any());
        Assert.IsFalse(_derivada?.MetodosInvocados.Any());

        _servicio?.AgregarMetodo(_derivada.Id, metodo.Id);

        Assert.IsTrue(_derivada.MetodosInvocados.Contains(metodo));
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadYaExisteExcepcion))]
    public void AgregarMetodoDuplicadoEnMismaClaseTest()
    {
        var metodo = new Metodo { Id = Guid.NewGuid(), Nombre = "Funcion", EsAbstract = false };
        _derivada?.MetodosInvocados.Add(metodo);
       _metodoServicio?.Setup(repo => repo.ObtenerMetodoPorId(metodo.Id)).Returns(metodo);
        _servicio?.AgregarMetodo(_derivada.Id, metodo.Id);
    }

    [TestMethod]
    [ExpectedException(typeof(OperacionNoPermitidaExcepcion))]
    public void AgregarMetodoAbstractoEnClaseNoAbstractaTest()
    {
        var otrometodo = new Metodo { Id = Guid.NewGuid(), Nombre = "Valor", EsAbstract = true };

       _metodoServicio?.Setup(repo => repo.ObtenerMetodoPorId(otrometodo.Id)).Returns(otrometodo);
        _servicio?.AgregarMetodo(_derivada.Id, otrometodo.Id);
    }

    [TestMethod]
    public void CrearClase_DeberiaCrearTipoDatoConNombreDeLaClaseTest()
    {
        var nuevaClase = new Clase("Variable", false, false, new List<Atributo>(), new List<Metodo>(), _base, _base.Id, new List<Interfaz>());
        _tipoDatoServicio?.Setup(repo => repo.CrearTipoDato(It.IsAny<TipoDato>()));
        _servicio?.CrearClase(nuevaClase);
        _repositorioClase?.Verify(r => r.Agregar(nuevaClase), Times.Once);
        _tipoDatoServicio?.Verify(s => s.CrearTipoDato(It.Is<TipoDato>(td => td.Nombre == "Variable")), Times.Once);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNulaExcepcion))]
    public void CrearClase_NoDeberiaCrearTipoDatoConNombreNULLTest()
    {
        var clase = new Clase { Nombre = null };

        _servicio?.CrearClase(clase);
        _tipoDatoServicio?.Verify(s => s.CrearTipoDato(It.Is<TipoDato>(td => td.Nombre == "Mascota")), Times.Never);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNoEncontradaExcepcion))]
    public void CrearClase_PropagaExcepcionSiFallaTipoDatoTest()
    {
        var clase = new Clase { Nombre = "Animal" };
        _tipoDatoServicio?
            .Setup(s => s.CrearTipoDato(It.IsAny<TipoDato>()))
            .Throws(new EntidadNoEncontradaExcepcion("falló al agregar tipo de dato"));
        _servicio?.CrearClase(clase);
    }

    [TestMethod]
    public void AgregarMetodo_AsignaClaseAlMetodo_CorrectamenteTest()
    {
        var metodo = new Metodo
        {
            Id = Guid.NewGuid(),
            Nombre = "MiMetodo",
            EsAbstract = false,
            Parametros = new List<Parametro>()
        };

        _metodoServicio?
            .Setup(s => s.ObtenerMetodoPorId(metodo.Id))
            .Returns(metodo);

        _metodoServicio?
            .Setup(s => s.ActualizarMetodo(It.Is<Metodo>(m => m.Clase == _derivada)))
            .Verifiable();

        _repositorioClase?
            .Setup(r => r.Actualizar(_derivada!))
            .Verifiable();

        _servicio?.AgregarMetodo(_derivada!.Id, metodo.Id);

        _metodoServicio?.Verify(s => s.ActualizarMetodo(It.Is<Metodo>(m => m.Clase == _derivada)), Times.Once);
        Assert.AreEqual(_derivada, metodo.Clase);
        Assert.IsTrue(_derivada.MetodosInvocados.Contains(metodo));
    }

    [TestMethod]
    public void CompararParametros_CuandoUnaListaEsNula_RetornaFalse()
    {
        var lista = new List<Parametro> { new() { TipoDato = new TipoDato("int") } };
        Assert.IsFalse((bool)_servicio?.CompararParametros(lista, null) || (bool)_servicio?.CompararParametros(null, lista));
    }

    [TestMethod]
    public void CompararParametros_CuandoListasTienenDistintaCantidad_RetornaFalse()
    {
        var a = new List<Parametro> { new() { TipoDato = new TipoDato("int") } };
        var b = new List<Parametro>
        {
            new() { TipoDato = new TipoDato("int") },
            new() { TipoDato = new TipoDato("string") }
        };

        Assert.IsFalse(_servicio?.CompararParametros(a, b));
    }

    [TestMethod]
    public void CompararParametros_CuandoTiposSonDistintos_RetornaFalse()
    {
        var a = new List<Parametro> { new() { TipoDato = new TipoDato("int") } };
        var b = new List<Parametro> { new() { TipoDato = new TipoDato("string") } };

        Assert.IsFalse(_servicio?.CompararParametros(a, b));
    }

    [TestMethod]
    public void CompararParametros_CuandoTiposCoinciden_RetornaTrue()
    {
        var a = new List<Parametro>
        {
            new() { TipoDato = new TipoDato("int") },
            new() { TipoDato = new TipoDato("string") }
        };

        var b = new List<Parametro>
        {
            new() { TipoDato = new TipoDato("int") },
            new() { TipoDato = new TipoDato("string") }
        };

        Assert.IsTrue(_servicio?.CompararParametros(a, b));
    }

    [TestMethod]
    public void CompararParametros_CuandoTipoDatoEsNull_RetornaFalse()
    {
        var a = new List<Parametro> { new() { TipoDato = null } };
        var b = new List<Parametro> { new() { TipoDato = new TipoDato("int") } };

        Assert.IsFalse(_servicio?.CompararParametros(a, b));
    }

    [TestMethod]
    public void VerificarImplementacionInterfaz_CuandoClaseImplementaTodo_NoLanzaExcepcion()
    {
        var firma = new Metodo("Ejecutar", new TipoDato(), new List<Parametro>());
        var interfaz = new Interfaz("IEjecutable", new List<Metodo>());

        var clase = new Clase
        {
            Nombre = "MiClase",
            InterfacesImplementadas = new List<Interfaz> { interfaz },
            MetodosInvocados = new List<Metodo>
            {
                new Metodo("Ejecutar", new TipoDato(), new List<Parametro>())
            }
        };

        _servicio?.VerificarImplementacionInterfaz(clase);
    }

    [TestMethod]
    [ExpectedException(typeof(OperacionNoPermitidaExcepcion))]
    public void VerificarImplementacionInterfaz_CuandoMetodoNoEsta_LanzaExcepcion()
    {
        var firma = new Metodo("Faltante", new TipoDato(), new List<Parametro>());
        var interfaz = new Interfaz("IFaltante", new List<Metodo> { firma });

        var clase = new Clase
        {
            Nombre = "ClaseIncompleta",
            InterfacesImplementadas = new List<Interfaz> { interfaz },
            MetodosInvocados = new List<Metodo>()
        };

        _servicio?.VerificarImplementacionInterfaz(clase);
    }

    [TestMethod]
    [ExpectedException(typeof(OperacionNoPermitidaExcepcion))]
    public void VerificarImplementacionInterfaz_CuandoTipoRetornoEsDistinto_LanzaExcepcion()
    {
        var firma = new Metodo("ObtenerAtributoPorId", new TipoDato(), new List<Parametro>());
        var interfaz = new Interfaz("IObtenedor", new List<Metodo> { firma });

        var clase = new Clase
        {
            Nombre = "ClaseA",
            InterfacesImplementadas = new List<Interfaz> { interfaz },
            MetodosInvocados = new List<Metodo>
            {
                new Metodo("ObtenerAtributoPorId", new TipoDato(), new List<Parametro>())
            }
        };

        _servicio?.VerificarImplementacionInterfaz(clase);
    }

    [TestMethod]
    [ExpectedException(typeof(OperacionNoPermitidaExcepcion))]
    public void VerificarImplementacionInterfaz_CuandoParametrosSonDistintos_LanzaExcepcion()
    {
        var parametro1 = new Parametro("parametro1", new TipoDato("int"));
        var parametro2 = new Parametro("parametro2", new TipoDato("string"));
        var firma = new Metodo("Procesar", new TipoDato(), new List<Parametro>() { parametro1 });
        var interfaz = new Interfaz("IProcesable", new List<Metodo> { firma });

        var clase = new Clase
        {
            Nombre = "ClaseB",
            InterfacesImplementadas = new List<Interfaz> { interfaz },
            MetodosInvocados = new List<Metodo>
            {
                new Metodo("Procesar", new TipoDato(), new List<Parametro>() { parametro2 })
            }
        };

        _servicio?.VerificarImplementacionInterfaz(clase);
    }

    [TestMethod]
    public void VerificarImplementacionInterfaz_CuandoHayMultiplesInterfacesYTodoEstaBien_NoLanzaExcepcion()
    {
        var tipoInt = new TipoDato("int");
        var tipoVoid = new TipoDato("void");
        var metodoA = new Metodo("MetodoA", tipoVoid, []);
        var metodoB = new Metodo("MetodoB", tipoInt, [new Parametro { TipoDato = tipoInt }]);

        var interfaz1 = new Interfaz("I1", new List<Metodo> { metodoA });
        var interfaz2 = new Interfaz("I2", new List<Metodo> { metodoB });

        var clase = new Clase
        {
            Nombre = "ClaseCompleta",
            InterfacesImplementadas = new List<Interfaz> { interfaz1, interfaz2 },
            MetodosInvocados = new List<Metodo>
            {
                new Metodo("MetodoA", tipoVoid, []),
                new Metodo("MetodoB", tipoInt, [new Parametro { TipoDato = tipoInt }])
            }
        };

        _servicio?.VerificarImplementacionInterfaz(clase);
    }

    [TestMethod]
    public void AgregarAtributo_EstaticoValidoTest()
    {
        var claseVacia = new Clase { Id = Guid.NewGuid(), Atributos = [] };
        _repositorioClase?.Setup(r => r.ObtenerClase(claseVacia.Id)).Returns(claseVacia);
        var idAtributo = Guid.NewGuid();
        var atributo = new Atributo { Id = idAtributo, Nombre = "Pueba", EsEstatico = true };
        _atributoServicio?.Setup(s => s.ObtenerAtributoPorId(idAtributo)).Returns(atributo);
        _repositorioClase?.Setup(r => r.Actualizar(claseVacia)).Verifiable();
        _servicio?.AgregarAtributo(claseVacia.Id, idAtributo);
        Assert.IsTrue(claseVacia.Atributos.Contains(atributo));
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadYaExisteExcepcion))]
    public void AgregarAtributo_EstaticoDuplicado_LanzaExcepcionTest()
    {
        var idAtributo = Guid.NewGuid();
        var atributoExistente = new Atributo { Id = idAtributo, Nombre = "PuebaDuplicado", EsEstatico = true };
        var clase = new Clase { Id = Guid.NewGuid(), Atributos = new List<Atributo> { atributoExistente } };
        _repositorioClase?.Setup(r => r.ObtenerClase(clase.Id)).Returns(clase);
        var atributoNuevo = new Atributo { Id = idAtributo, Nombre = "PuebaDuplicado", EsEstatico = true };
        _atributoServicio?.Setup(s => s.ObtenerAtributoPorId(idAtributo)).Returns(atributoNuevo);
        _servicio?.AgregarAtributo(clase.Id, idAtributo);
    }

    [TestMethod]
    public void AgregarMetodo_EstaticoValidoTest()
    {
        var claseVacia = new Clase { Id = Guid.NewGuid(), MetodosInvocados = [] };
        _repositorioClase?.Setup(r => r.ObtenerClase(claseVacia.Id)).Returns(claseVacia);
        var idMetodo = Guid.NewGuid();
        var metodo = new Metodo { Id = idMetodo, Nombre = "Pueba", Parametros = new List<Parametro>(), EsEstatico = true, EsAbstract = false };
        _metodoServicio?.Setup(s => s.ObtenerMetodoPorId(idMetodo)).Returns(metodo);
        _repositorioClase?.Setup(r => r.Actualizar(claseVacia)).Verifiable();
        _servicio?.AgregarMetodo(claseVacia.Id, idMetodo);
        Assert.IsTrue(claseVacia.MetodosInvocados.Contains(metodo));
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadYaExisteExcepcion))]
    public void AgregarMetodoEstaticoDuplicado_LanzaExcepcionTest()
    {
        var idMetodo = Guid.NewGuid();
        var metodoExiste = new Metodo { Id = idMetodo, Nombre = "Duplicado", Parametros = new List<Parametro>(), EsEstatico = true, EsAbstract = false };
        var clase = new Clase { Id = Guid.NewGuid(), MetodosInvocados = new List<Metodo> { metodoExiste } };
        _repositorioClase?.Setup(r => r.ObtenerClase(clase.Id)).Returns(clase);
        var metodoNuevo = new Metodo { Id = idMetodo, Nombre = "Duplicado", Parametros = new List<Parametro>(), EsEstatico = true, EsAbstract = false };
        _metodoServicio?.Setup(s => s.ObtenerMetodoPorId(idMetodo)).Returns(metodoNuevo);
        _servicio?.AgregarMetodo(clase.Id, idMetodo);
    }

    [TestMethod]
    [ExpectedException(typeof(OperacionNoPermitidaExcepcion))]
    public void AgregarMetodoEstaticoYAbstracto_LanzaExcepcionTest()
    {
        var idMetodo = Guid.NewGuid();
        var idClase = Guid.NewGuid();
        var clase = new Clase { Id = idClase, MetodosInvocados = [] };
        _repositorioClase?.Setup(r => r.ObtenerClase(idClase)).Returns(clase);

        var metodo = new Metodo { Id = idMetodo, Nombre = "estatico", Parametros = new List<Parametro>(), EsEstatico = true, EsAbstract = true };
        _metodoServicio?.Setup(s => s.ObtenerMetodoPorId(idMetodo)).Returns(metodo);
        _servicio?.AgregarMetodo(idClase, idMetodo);
    }

    [TestMethod]
    [ExpectedException(typeof(OperacionNoPermitidaExcepcion))]
    public void AgregarMetodo_VirtualConBaseNoVirtual_LanzaExcepcionTest()
    {
        var idClase = Guid.NewGuid();
        var idMetodo = Guid.NewGuid();
        var metodo = new Metodo { Id = idMetodo, Nombre = "m", Parametros = new List<Parametro>(), EsVirtual = false, EsAbstract = false };

        var claseBase = new Clase { Id = Guid.NewGuid(), MetodosInvocados = new List<Metodo> { metodo } };
        var claseDerivada = new Clase { Id = idClase, ClaseBase = claseBase, MetodosInvocados = [] };

        _repositorioClase?.Setup(r => r.ObtenerClase(idClase)).Returns(claseDerivada);
        _repositorioClase?.Setup(r => r.ObtenerClase(claseBase.Id)).Returns(claseBase);

        var metodoOverride = new Metodo { Id = idMetodo, Nombre = "m", Parametros = new List<Parametro>(), EsVirtual = true, EsAbstract = false };
        _metodoServicio?.Setup(s => s.ObtenerMetodoPorId(metodoOverride.Id)).Returns(metodoOverride);
        _servicio?.AgregarMetodo(idClase, metodoOverride.Id);
    }

    [TestMethod]
    public void AgregarMetodoVirtualConFirmaNuevaTest()
    {
        var metodoVirtual = new Metodo
        {
            Id = Guid.NewGuid(),
            Nombre = "NuevoVirtual",
            Parametros = new List<Parametro>(),
            EsVirtual = true,
            EsAbstract = false,
            TipoRetorno = new TipoDato("void", null)
        };

        _metodoServicio?.Setup(s => s.ObtenerMetodoPorId(metodoVirtual.Id)).Returns(metodoVirtual);
        _repositorioClase?.Setup(r => r.Actualizar(_derivada!)).Verifiable();

        _servicio?.AgregarMetodo(_derivada!.Id, metodoVirtual.Id);

        Assert.IsTrue(_derivada?.MetodosInvocados.Contains(metodoVirtual));
    }

    [TestMethod]
    public void InicializarListasDeClase_CuandoAmbasListasSonNull_DeberiaInicializarlas()
    {
        var clase = new Clase { Atributos = null, MetodosInvocados = null };

        var metodo = typeof(ClaseServicio).GetMethod("InicializarListasDeClase", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        metodo?.Invoke(null, new object[] { clase });

        Assert.IsNotNull(clase.Atributos);
        Assert.IsNotNull(clase.MetodosInvocados);
    }

    [TestMethod]
    public void InicializarListasDeClase_CuandoSoloMetodosEsNull_DeberiaInicializarSoloMetodos()
    {
        var clase = new Clase { Atributos = new List<Atributo>(), MetodosInvocados = null };

        var metodo = typeof(ClaseServicio).GetMethod("InicializarListasDeClase", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        metodo?.Invoke(null, new object[] { clase });

        Assert.IsNotNull(clase.Atributos);
        Assert.IsNotNull(clase.MetodosInvocados);
    }

    [TestMethod]
    public void InicializarListasDeClase_CuandoSoloAtributosEsNull_DeberiaInicializarSoloAtributos()
    {
        var clase = new Clase { Atributos = null, MetodosInvocados = new List<Metodo>() };

        var metodo = typeof(ClaseServicio).GetMethod("InicializarListasDeClase", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        metodo?.Invoke(null, new object[] { clase });

        Assert.IsNotNull(clase.Atributos);
        Assert.IsNotNull(clase.MetodosInvocados);
    }

    [TestMethod]
    public void ValidarRedefinicionAbstract_MetodoBaseNull_NoLanza()
    {
        var claseServicio = _servicio!;
        var metodo = new Metodo { Nombre = "A", Parametros = new(), TipoRetorno = new TipoDato("int") };
        var lista = new List<Metodo>();

        var miMetodo = typeof(ClaseServicio).GetMethod("ValidarRedefinicionAbstract", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        miMetodo!.Invoke(claseServicio, new object[] { metodo, lista });
    }

    [TestMethod]
    public void ValidarRedefinicionAbstract_MetodoBaseExisteYOverride_NoLanza()
    {
        var tipo = new TipoDato("int");
        var parametros = new List<Parametro> { new Parametro("p", tipo) };

        var metodoBase = new Metodo { Nombre = "X", Parametros = parametros, TipoRetorno = tipo, EsAbstract = true };
        var metodo = new Metodo { Nombre = "X", Parametros = parametros, TipoRetorno = tipo, EsOverride = true };

        var miMetodo = typeof(ClaseServicio).GetMethod("ValidarRedefinicionAbstract", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        miMetodo!.Invoke(_servicio, new object[] { metodo, new List<Metodo> { metodoBase } });
    }

    [TestMethod]
    public void ValidarRedefinicionAbstract_MetodoBaseExisteYNoAbstractYNoOverride_NoLanza()
    {
        var tipo = new TipoDato("int");
        var parametros = new List<Parametro> { new Parametro("p", tipo) };

        var metodoBase = new Metodo { Nombre = "X", Parametros = parametros, TipoRetorno = tipo, EsAbstract = false };
        var metodo = new Metodo { Nombre = "X", Parametros = parametros, TipoRetorno = tipo, EsOverride = false };

        var miMetodo = typeof(ClaseServicio).GetMethod("ValidarRedefinicionAbstract", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        miMetodo!.Invoke(_servicio, new object[] { metodo, new List<Metodo> { metodoBase } });
    }

    [TestMethod]
    public void ValidarRedefinicionAbstract_MetodoBaseAbstractYNoOverride_LanzaExcepcion()
    {
        var tipo = new TipoDato("int");
        var parametros = new List<Parametro> { new Parametro("p", tipo) };

        var metodoBase = new Metodo { Nombre = "X", Parametros = parametros, TipoRetorno = tipo, EsAbstract = true };
        var metodo = new Metodo { Nombre = "X", Parametros = parametros, TipoRetorno = tipo, EsOverride = false };

        var miMetodo = typeof(ClaseServicio).GetMethod("ValidarRedefinicionAbstract",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        try
        {
            miMetodo!.Invoke(_servicio, new object[] { metodo, new List<Metodo> { metodoBase } });
            Assert.Fail("Se esperaba una OperacionNoPermitidaExcepcion pero no fue lanzada.");
        }
        catch (TargetInvocationException ex)
        {
            Assert.IsInstanceOfType(ex.InnerException, typeof(OperacionNoPermitidaExcepcion));
            Assert.AreEqual($"Debe usar override para implementar el metodo abstracto '{metodo.Nombre}'.",
                ex.InnerException!.Message);
        }
    }

    [TestMethod]
    public void ValidarRedefinicionSealed_MetodoBaseSealedYNoOverride_LanzaExcepcion()
    {
        var tipo = new TipoDato("int");
        var parametros = new List<Parametro> { new Parametro("p", tipo) };

        var metodoBase = new Metodo { Nombre = "X", Parametros = parametros, TipoRetorno = tipo, EsSealed = true };

        var metodo = new Metodo { Nombre = "X", Parametros = parametros, TipoRetorno = tipo, EsOverride = false };

        var miMetodo = typeof(ClaseServicio).GetMethod("ValidarRedefinicionSealed",
            BindingFlags.NonPublic | BindingFlags.Instance);

        try
        {
            miMetodo!.Invoke(_servicio, new object[] { metodo, new List<Metodo> { metodoBase } });
            Assert.Fail("Se esperaba una OperacionNoPermitidaExcepcion pero no fue lanzada.");
        }
        catch (TargetInvocationException ex)
        {
            Assert.IsInstanceOfType(ex.InnerException, typeof(OperacionNoPermitidaExcepcion));
            Assert.AreEqual(
                $"No se puede redefinir el metodo '{metodo.Nombre}' porque esta marcado como sealed en la clase base.",
                ex.InnerException!.Message);
        }
    }

    [TestMethod]
    [ExpectedException(typeof(OperacionNoPermitidaExcepcion))]
    public void AgregarMetodo_ClaseSealedConMetodoVirtual_LanzaExcepcion()
    {
        _derivada!.EsSealed = true;

        var metodo = new Metodo
        {
            Id = Guid.NewGuid(),
            Nombre = "MetodoVirtual",
            EsVirtual = true,
            EsAbstract = false
        };

        _metodoServicio?.Setup(m => m.ObtenerMetodoPorId(metodo.Id)).Returns(metodo);
        _repositorioClase?.Setup(r => r.ObtenerClase(_derivada.Id)).Returns(_derivada);

        _servicio?.AgregarMetodo(_derivada.Id, metodo.Id);
    }

    [TestMethod]
    public void AgregarMetodo_MetodoNoEsOverride_NoLanzaExcepcion()
    {
        var tipo = new TipoDato("int");
        var metodo = new Metodo
        {
            Id = Guid.NewGuid(),
            Nombre = "Procesar",
            TipoRetorno = tipo,
            EsOverride = false,
            Parametros = new List<Parametro>(),
        };
        var clase = new Clase
        {
            Id = Guid.NewGuid(),
            Nombre = "ClaseBase",
            MetodosInvocados = new List<Metodo>()
        };

        _repositorioClase?.Setup(r => r.ObtenerClase(clase.Id)).Returns(clase);
        _repositorioClase?.Setup(r => r.Existe(clase)).Returns(false);
        _metodoServicio?.Setup(m => m.ObtenerMetodoPorId(metodo.Id)).Returns(metodo);

        _servicio?.AgregarMetodo(clase.Id, metodo.Id);
    }

    [TestMethod]
    [ExpectedException(typeof(OperacionNoPermitidaExcepcion))]
    public void AgregarMetodo_OverrideSinMetodoBaseVirtualOAbstracto_LanzaExcepcion()
    {
        var tipo = new TipoDato("int");
        var metodo = new Metodo
        {
            Id = Guid.NewGuid(),
            Nombre = "Procesar",
            TipoRetorno = tipo,
            EsOverride = true,
            Parametros = new List<Parametro>()
        };
        var claseBase = new Clase
        {
            Id = Guid.NewGuid(),
            Nombre = "Base",
            EsAbstract = true,
            MetodosInvocados = new List<Metodo>
            {
                new Metodo
                {
                    Nombre = "Procesar",
                    TipoRetorno = tipo,
                    Parametros = new List<Parametro>(),
                    EsVirtual = false,
                    EsAbstract = false
                }
            }
        };
        var claseDerivada = new Clase
        {
            Id = Guid.NewGuid(),
            Nombre = "Derivada",
            ClaseBase = claseBase,
            MetodosInvocados = new List<Metodo>()
        };

        _repositorioClase?.Setup(r => r.ObtenerClase(claseDerivada.Id)).Returns(claseDerivada);
        _repositorioClase?.Setup(r => r.ObtenerClase(claseBase.Id)).Returns(claseBase);
        _metodoServicio?.Setup(m => m.ObtenerMetodoPorId(metodo.Id)).Returns(metodo);

        _servicio?.AgregarMetodo(claseDerivada.Id, metodo.Id);
    }

    [TestMethod]
    public void AgregarMetodo_MetodoNoEsAbstract_NoLanzaExcepcionNiModificaClase()
    {
        var tipo = new TipoDato("int");
        var metodo = new Metodo
        {
            Id = Guid.NewGuid(),
            Nombre = "Procesar",
            TipoRetorno = tipo,
            EsAbstract = false,
            Parametros = new List<Parametro>()
        };

        var clase = new Clase
        {
            Id = Guid.NewGuid(),
            Nombre = "MiClase",
            EsAbstract = false,
            MetodosInvocados = new List<Metodo>()
        };

        _metodoServicio?.Setup(m => m.ObtenerMetodoPorId(metodo.Id)).Returns(metodo);
        _repositorioClase?.Setup(r => r.ObtenerClase(clase.Id)).Returns(clase);

        _servicio?.AgregarMetodo(clase.Id, metodo.Id);

        Assert.IsFalse(clase.EsAbstract);
    }

    [TestMethod]
    [ExpectedException(typeof(OperacionNoPermitidaExcepcion))]
    public void AgregarMetodo_AbstractoYClaseBaseNoAbstracta_LanzaExcepcion()
    {
        var tipo = new TipoDato("int");
        var metodo = new Metodo
        {
            Id = Guid.NewGuid(),
            Nombre = "Procesar",
            TipoRetorno = tipo,
            EsAbstract = true,
            Parametros = new List<Parametro>()
        };

        var claseBase = new Clase
        {
            Id = Guid.NewGuid(),
            Nombre = "Base",
            EsAbstract = false
        };

        var clase = new Clase
        {
            Id = Guid.NewGuid(),
            Nombre = "Derivada",
            ClaseBase = claseBase,
            EsAbstract = false,
            MetodosInvocados = new List<Metodo>()
        };

        _metodoServicio?.Setup(m => m.ObtenerMetodoPorId(metodo.Id)).Returns(metodo);
        _repositorioClase?.Setup(r => r.ObtenerClase(clase.Id)).Returns(clase);
        _repositorioClase?.Setup(r => r.ObtenerClase(claseBase.Id)).Returns(claseBase);

        _servicio?.AgregarMetodo(clase.Id, metodo.Id);
    }

    [TestMethod]
    public void AgregarMetodo_AbstractoYClaseBaseAbstracta_ClaseSeVuelveAbstracta()
    {
        var tipo = new TipoDato("int");
        var metodo = new Metodo
        {
            Id = Guid.NewGuid(),
            Nombre = "Procesar",
            TipoRetorno = tipo,
            EsAbstract = true,
            Parametros = new List<Parametro>()
        };

        var claseBase = new Clase
        {
            Id = Guid.NewGuid(),
            Nombre = "Base",
            EsAbstract = true
        };

        var clase = new Clase
        {
            Id = Guid.NewGuid(),
            Nombre = "Derivada",
            ClaseBase = claseBase,
            EsAbstract = false,
            MetodosInvocados = new List<Metodo>()
        };

        _metodoServicio?.Setup(m => m.ObtenerMetodoPorId(metodo.Id)).Returns(metodo);
        _repositorioClase?.Setup(r => r.ObtenerClase(clase.Id)).Returns(clase);
        _repositorioClase?.Setup(r => r.ObtenerClase(claseBase.Id)).Returns(claseBase);

        _servicio?.AgregarMetodo(clase.Id, metodo.Id);

        Assert.IsTrue(clase.EsAbstract);
    }

    [TestMethod]
    public void QuitarAtributo_AtributoExistente_SeEliminaYActualizaTest()
    {
        _base?.Atributos.Add(_atributo!);

        _servicio?.QuitarAtributo(_base.Id, _atributo.Id);

        Assert.IsFalse(_base?.Atributos.Any(a => a.Id == _atributo?.Id));
        _repositorioClase?.Verify(r => r.Actualizar(_base), Times.Once);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNoEncontradaExcepcion))]
    public void QuitarAtributo_AtributoNoPerteneceALaClase_LanzaExcepcionTest()
    {
        _base!.Atributos.Clear();
        _servicio?.QuitarAtributo(_base.Id, Guid.NewGuid());
    }

    [TestMethod]
    public void QuitarMetodo_MetodoExistente_SeEliminaYActualizaTest()
    {
        var metodo = new Metodo { Id = Guid.NewGuid(), Nombre = "M" };
        _base!.MetodosInvocados.Add(metodo);

        _servicio?.QuitarMetodo(_base.Id, metodo.Id);
         Assert.IsFalse(_base.MetodosInvocados.Any(m => m.Id == metodo.Id));
        _repositorioClase?.Verify(r => r.Actualizar(_base), Times.Once);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNoEncontradaExcepcion))]
    public void QuitarMetodo_MetodoNoPerteneceALaClase_LanzaExcepcionTest()
    {
        _base!.MetodosInvocados.Clear();

        _servicio?.QuitarMetodo(_base.Id, Guid.NewGuid());
    }

    [TestMethod]
    public void AgregarMetodo_PendienteImplementaAbstracto_NoLanzaExcepcion()
    {
        var tipo = new TipoDato("void") { Id = Guid.NewGuid() };
        var abstracto = new Metodo
        {
            Nombre = "Ejecutar",
            EsAbstract = true,
            Parametros = [new Parametro("x", tipo)],
            TipoRetorno = tipo
        };
        var claseBase = new Clase
        {
            Id = Guid.NewGuid(),
            EsAbstract = true,
            MetodosInvocados = [abstracto]
        };
        var metodoOverride = new Metodo
        {
            Id = Guid.NewGuid(),
            Nombre = "Ejecutar",
            EsOverride = true,
            EsAbstract = false,
            Parametros = [new Parametro("x", tipo)],
            TipoRetorno = tipo
        };
        var clase = new Clase
        {
            Id = Guid.NewGuid(),
            ClaseBase = claseBase,
            EsAbstract = false,
            MetodosInvocados = []
        };
        _repositorioClase!.Setup(r => r.ObtenerClase(clase.Id)).Returns(clase);
        _repositorioClase!.Setup(r => r.ObtenerClase(claseBase.Id)).Returns(claseBase);
        _metodoServicio!.Setup(m => m.ObtenerMetodoPorId(metodoOverride.Id)).Returns(metodoOverride);

        _servicio!.AgregarMetodo(clase.Id, metodoOverride.Id);

        Assert.IsTrue(clase.MetodosInvocados.Contains(metodoOverride));
    }
}
