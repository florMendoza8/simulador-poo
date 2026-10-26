using AccesoADatos.InterfacesRepositorios;
using Dominio;
using Dominio.Excepciones;
using Moq;
using Servicios;
using Servicios.InterfacesServicios;

namespace ServiciosTest;

[TestClass]
public class MetodoServicioTest
{
    private Mock<IMetodoRepositorio>? _metodoRepositorioMock;
    private MetodoServicio? _metodoServicio;
    private Mock<IParametroServicio>? _parametroServicioMock;
    private Mock<IVariableLocalServicio>? _variableLocalServicioMock;
    private Metodo _metodo = null;
    private Metodo _metodoInvocado = null;
    private Parametro _parametro = null;
    private VariableLocal _variableLocal = null;
    private Guid _id;

    [TestInitialize]
    public void Inizializar()
    {
        _metodoRepositorioMock = new Mock<IMetodoRepositorio>(MockBehavior.Strict);
        _parametroServicioMock = new Mock<IParametroServicio>(MockBehavior.Strict);
        _variableLocalServicioMock = new Mock<IVariableLocalServicio>(MockBehavior.Strict);
        _metodoServicio = new MetodoServicio(_metodoRepositorioMock.Object, _parametroServicioMock.Object,
            _variableLocalServicioMock.Object);
        _id = Guid.NewGuid();
        _metodo = new Metodo
        {
            Id = _id,
            Nombre = "Nombre del metodo",
            TipoRetorno = new TipoDato(),
            Accesibilidad = Accesibilidad.Public,
            EsAbstract = false,
            EsSealed = false
        };
        _parametro = new Parametro
        {
            Id = Guid.NewGuid(),
            Nombre = "Nombre del parametro",
            TipoDato = new TipoDato()
        };
        _parametroServicioMock.Setup(repo => repo.ObtenerParametroPorId(_parametro.Id)).Returns(_parametro);
        _variableLocal = new VariableLocal
        {
            Id = Guid.NewGuid(),
            Nombre = "Nombre de variable local",
            TipoReferencia = new TipoDato(),
            Valor = "Valor"
        };
        _variableLocalServicioMock
            .Setup(repo => repo.ObtenerVariableLocalPorId(_variableLocal.Id))
            .Returns(_variableLocal);
        _metodoInvocado = new Metodo
        {
            Id = Guid.NewGuid(),
            Nombre = "Metodo Invocado",
            TipoRetorno = new TipoDato(),
            Accesibilidad = Accesibilidad.Public,
            EsAbstract = false,
            EsSealed = false
        };
    }

    [TestMethod]
    public void AgregarMetodo_DatosValidos_DebeAgregarloCorrectamente()
    {
        _metodoRepositorioMock!
            .Setup(r => r.Agregar(_metodo));

        var resultado = _metodoServicio!.AgregarMetodo(_metodo);

        _metodoRepositorioMock.Verify(r => r.Agregar(_metodo), Times.Once);
        Assert.AreEqual(_metodo, resultado);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNulaExcepcion))]
    public void AgregarMetodo_Nulo_DebeLanzarArgumentNullException()
    {
        _metodoServicio!.AgregarMetodo(null);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNulaExcepcion))]
    public void AgregarMetodo_NombreNuloOVacio_DebeLanzarArgumentException()
    {
        var metodoNuevo = new Metodo
        {
            Id = Guid.NewGuid(),
            Nombre = null,
            TipoRetorno = new TipoDato(),
            Accesibilidad = Accesibilidad.Public,
            Parametros = new List<Parametro>(),
            VariablesLocales = new List<VariableLocal>()
        };

        _metodoServicio!.AgregarMetodo(metodoNuevo);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNulaExcepcion))]
    public void AgregarMetodo_TipoRetornoNulo_DebeLanzarArgumentException()
    {
        var metodoNuevo = new Metodo
        {
            Id = Guid.NewGuid(),
            Nombre = "Nuevo nombre metodo",
            TipoRetorno = null,
            Accesibilidad = Accesibilidad.Public,
            Parametros = new List<Parametro>(),
            VariablesLocales = new List<VariableLocal>()
        };

        _metodoServicio!.AgregarMetodo(metodoNuevo);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNulaExcepcion))]
    public void AgregarMetodo_AccesibilidadNula_DebeLanzarArgumentException()
    {
        var metodoNuevo = new Metodo
        {
            Id = Guid.NewGuid(),
            Nombre = "Nuevo nombre metodo",
            TipoRetorno = new TipoDato(),
            Accesibilidad = null,
            Parametros = new List<Parametro>(),
            VariablesLocales = new List<VariableLocal>()
        };

        _metodoServicio!.AgregarMetodo(metodoNuevo);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNulaExcepcion))]
    public void AgregarMetodo_EsSealedNulo_DebeLanzarArgumentException()
    {
        var metodoNuevo = new Metodo
        {
            Id = Guid.NewGuid(),
            Nombre = "Nuevo nombre metodo",
            TipoRetorno = new TipoDato(),
            Accesibilidad = Accesibilidad.Private,
            EsSealed = null
        };

        _metodoServicio!.AgregarMetodo(metodoNuevo);
    }

    [TestMethod]
    [ExpectedException(typeof(OperacionNoPermitidaExcepcion))]
    public void AgregarMetodo_EsSealedYEsAbstact_DebeLanzarException()
    {
        _metodo.EsAbstract = true;
        _metodo.EsSealed = true;

        _metodoServicio!.AgregarMetodo(_metodo);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNulaExcepcion))]
    public void AgregarMetodo_ListaParametrosNula_DebeLanzarArgumentException()
    {
        _metodo.Parametros = null;

        _metodoServicio!.AgregarMetodo(_metodo);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNulaExcepcion))]
    public void AgregarMetodo_ListaVariablesLocalesNula_DebeLanzarArgumentException()
    {
        _metodo.VariablesLocales = null;

        _metodoServicio!.AgregarMetodo(_metodo);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNulaExcepcion))]
    public void AgregarMetodo_ListaMetodosInvocadosNula_DebeLanzarArgumentException()
    {
        _metodo.MetodosInvocados = null;

        _metodoServicio!.AgregarMetodo(_metodo);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadYaExisteExcepcion))]
    public void AgregarMetodo_CuandoMetodosInvocadosTienenDuplicados_DebeLanzarArgumentException()
    {
        var metodoInvocado = new Metodo
        {
            Id = Guid.NewGuid(),
            Nombre = "Metodo Invocado",
            TipoRetorno = new TipoDato(),
            Accesibilidad = Accesibilidad.Public,
            EsSealed = false
        };

        _metodo.MetodosInvocados = new List<Metodo>
        {
            metodoInvocado,
            metodoInvocado
        };

        _metodoServicio!.AgregarMetodo(_metodo);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNulaExcepcion))]
    public void AgregarMetodo_CuandoParametroNombreNuloOVacio_LanzaException()
    {
        var tipoDato = new TipoDato{ Id = Guid.NewGuid(), Nombre = "int" };
        _metodo.Parametros = new List<Parametro>
        {
            new Parametro { Id = Guid.NewGuid(), Nombre = "Nombre", TipoDato = tipoDato },
            new Parametro { Id = Guid.NewGuid(), Nombre = null, TipoDato = tipoDato }
        };

        _metodoServicio!.AgregarMetodo(_metodo);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNulaExcepcion))]
    public void AgregarMetodo_CuandoParametroTipoDatoNulo_LanzaException()
    {
        var tipoDato = new TipoDato{ Id = Guid.NewGuid(), Nombre = "int" };
        _metodo.Parametros = new List<Parametro>
        {
            new Parametro { Id = Guid.NewGuid(), Nombre = "NombreA", TipoDato = tipoDato },
            new Parametro { Id = Guid.NewGuid(), Nombre = "NombreB", TipoDato = null }
        };

        _metodoServicio!.AgregarMetodo(_metodo);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadYaExisteExcepcion))]
    public void AgregarMetodo_CuandoParametroNombresDuplicados_LanzaException()
    {
        var tipoDato = new TipoDato{ Id = Guid.NewGuid(), Nombre = "int" };
        _metodo.Parametros = new List<Parametro>
        {
            new Parametro { Id = Guid.NewGuid(), Nombre = "Nombre", TipoDato = tipoDato },
            new Parametro { Id = Guid.NewGuid(), Nombre = "Nombre", TipoDato = tipoDato }
        };

        _metodoServicio!.AgregarMetodo(_metodo);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNulaExcepcion))]
    public void AgregarMetodo_CuandoParametroNulo_LanzaException()
    {
        var tipoDato = new TipoDato{ Id = Guid.NewGuid(), Nombre = "int" };
        _metodo.Parametros = new List<Parametro>
        {
            new Parametro { Id = Guid.NewGuid(), Nombre = "Nombre", TipoDato = tipoDato },
            null
        };

        _metodoServicio!.AgregarMetodo(_metodo);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNulaExcepcion))]
    public void AgregarMetodo_CuandoVariableLocalNombreNuloOVacio_LanzaException()
    {
        var tipoDato = new TipoDato{ Id = Guid.NewGuid(), Nombre = "int" };
        _metodo.VariablesLocales = new List<VariableLocal>
        {
            new VariableLocal
            {
                Id = Guid.NewGuid(),
                Nombre = "Nombre",
                TipoReferencia = tipoDato,
                Valor = string.Empty
            },
            new VariableLocal
            {
                Id = Guid.NewGuid(),
                Nombre = null,
                TipoReferencia = tipoDato,
                Valor = string.Empty
            }
        };

        _metodoServicio!.AgregarMetodo(_metodo);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNulaExcepcion))]
    public void AgregarMetodo_CuandoVariableLocalTipoDatoNulo_LanzaException()
    {
        var tipoDato = new TipoDato{ Id = Guid.NewGuid(), Nombre = "int" };
        _metodo.VariablesLocales = new List<VariableLocal>
        {
            new VariableLocal
            {
                Id = Guid.NewGuid(),
                Nombre = "NombreA",
                TipoReferencia = tipoDato,
                Valor = string.Empty
            },
            new VariableLocal
            {
                Id = Guid.NewGuid(),
                Nombre = "NombreB",
                TipoReferencia = null,
                Valor = string.Empty
            }
        };

        _metodoServicio!.AgregarMetodo(_metodo);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadYaExisteExcepcion))]
    public void AgregarMetodo_CuandoVariableLocalNombresDuplicados_LanzaException()
    {
        var tipoDato = new TipoDato{ Id = Guid.NewGuid(), Nombre = "int" };
        _metodo.VariablesLocales = new List<VariableLocal>
        {
            new VariableLocal
            {
                Id = Guid.NewGuid(),
                Nombre = "Nombre",
                TipoReferencia = tipoDato,
                Valor = string.Empty
            },
            new VariableLocal
            {
                Id = Guid.NewGuid(),
                Nombre = "Nombre",
                TipoReferencia = tipoDato,
                Valor = string.Empty
            }
        };

        _metodoServicio!.AgregarMetodo(_metodo);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNulaExcepcion))]
    public void AgregarMetodo_CuandoVariableLocalNulo_LanzaException()
    {
        var tipoDato = new TipoDato{ Id = Guid.NewGuid(), Nombre = "int" };
        _metodo.VariablesLocales = new List<VariableLocal>
        {
            new VariableLocal
            {
                Id = Guid.NewGuid(),
                Nombre = "Nombre",
                TipoReferencia = tipoDato,
                Valor = String.Empty
            },
            null
        };

        _metodoServicio!.AgregarMetodo(_metodo);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNulaExcepcion))]
    public void AgregarMetodo_CuandoMetodosInvocadosNulo_LanzaException()
    {
        var tipoDato = new TipoDato{ Id = Guid.NewGuid(), Nombre = "int" };
        _metodo.MetodosInvocados = new List<Metodo>
        {
            new Metodo
            {
                Id = Guid.NewGuid(),
                Nombre = "Nombre",
                TipoRetorno = tipoDato,
                Accesibilidad = Accesibilidad.Public,
                EsAbstract = false,
                EsSealed = false
            },
            null
        };

        _metodoServicio!.AgregarMetodo(_metodo);
    }

    [TestMethod]
    public void AgregarMetodo_CuandoMetodoInvocadoEsValido_DebeAgregarCorrectamente()
    {
        var tipoDato = new TipoDato { Id = Guid.NewGuid(), Nombre = "int" };

        var metodoInvocado = new Metodo
        {
            Id = Guid.NewGuid(),
            Nombre = "MetodoInvocado",
            TipoRetorno = tipoDato,
            Accesibilidad = Accesibilidad.Public,
            EsAbstract = false,
            EsSealed = false,
            Parametros = new List<Parametro>(),
            VariablesLocales = new List<VariableLocal>(),
            MetodosInvocados = new List<Metodo>()
        };

        _metodo.MetodosInvocados = new List<Metodo> { metodoInvocado };
        _metodo.Parametros = new List<Parametro>();
        _metodo.VariablesLocales = new List<VariableLocal>();

        _metodoRepositorioMock!
            .Setup(r => r.Agregar(_metodo));

        var resultado = _metodoServicio!.AgregarMetodo(_metodo);

        Assert.AreEqual(_metodo, resultado);
        _metodoRepositorioMock.Verify(r => r.Agregar(_metodo), Times.Once);
    }

    [TestMethod]
    public void EliminarMetodo_Valido_DebeLlamarRepositorio()
    {
        _metodoRepositorioMock!
            .Setup(r => r.Eliminar(_metodo));

        _metodoServicio!.EliminarMetodo(_metodo);

        _metodoRepositorioMock!.Verify(r => r.Eliminar(_metodo), Times.Once);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNulaExcepcion))]
    public void EliminarMetodo_CuandoNulo_DebeLanzarArgumentNullException()
    {
        _metodoServicio!.EliminarMetodo(null);
    }

    [TestMethod]
    public void ObtenerMetodoPorId_Valido_DebeDevolverlo()
    {
        _metodoRepositorioMock!
            .Setup(repo => repo.Obtener(_metodo.Id))
            .Returns(_metodo);

        var resultado = _metodoServicio!.ObtenerMetodoPorId(_metodo.Id);

        Assert.AreEqual(_metodo.Nombre, resultado.Nombre);
        _metodoRepositorioMock.Verify(r => r.Obtener(_metodo.Id), Times.Once);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNulaExcepcion))]
    public void ObtenerMetodoPorId_MetodoNulo_DebeLanzarExcepcion()
    {
        var id = Guid.NewGuid();
        _metodoRepositorioMock!
            .Setup(repo => repo.Obtener(id))
            .Returns<Metodo>(null);

        _metodoServicio!.ObtenerMetodoPorId(id);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNulaExcepcion))]
    public void ObtenerMetodoPorId_IdVacio_DebeLanzarExcepcion()
    {
        _metodoServicio!.ObtenerMetodoPorId(Guid.Empty);
    }

    [TestMethod]
    public void AgregarParametro_Valido_DebeAgregarlo()
    {
        _metodoRepositorioMock!
            .Setup(r => r.Obtener(_metodo.Id))
            .Returns(_metodo);

        _parametroServicioMock!
            .Setup(s => s.ObtenerParametroPorId(_parametro.Id))
            .Returns(_parametro);

        _metodoRepositorioMock.Setup(r => r.Actualizar(_metodo));

        _metodoServicio!.AgregarParametro(_metodo.Id, _parametro.Id);

        Assert.IsTrue(_metodo.Parametros.Contains(_parametro));
        _metodoRepositorioMock.Verify(r => r.Actualizar(_metodo), Times.Once);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNoEncontradaExcepcion))]
    public void AgregarParametro_CuandoMetodoNoExiste_DebeLanzarExcepcion()
    {
        var idMetodoInexistente = Guid.NewGuid();
        var idParametro = _parametro.Id;

        _metodoRepositorioMock!
            .Setup(r => r.Obtener(idMetodoInexistente))
            .Returns<Metodo>(null);

        _metodoServicio!.AgregarParametro(idMetodoInexistente, idParametro);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadYaExisteExcepcion))]
    public void AgregarParametro_ParametroYaExiste_DebeLanzarExcepcion()
    {
        _metodo.Parametros = new List<Parametro> { _parametro };

        _metodoRepositorioMock!
            .Setup(r => r.Obtener(_metodo.Id))
            .Returns(_metodo);

        _parametroServicioMock!
            .Setup(s => s.ObtenerParametroPorId(_parametro.Id))
            .Returns(_parametro);

        _metodoServicio!.AgregarParametro(_metodo.Id, _parametro.Id);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNulaExcepcion))]
    public void AgregarParametro_CuandoMetodoIdEsVacio_DebeLanzarArgumentException()
    {
        var parametroId = Guid.NewGuid();

        _metodoServicio!.AgregarParametro(Guid.Empty, parametroId);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNulaExcepcion))]
    public void AgregarParametro_CuandoParametroIdEsVacio_DebeLanzarArgumentException()
    {
        var metodoId = Guid.NewGuid();

        _metodoServicio!.AgregarParametro(metodoId, Guid.Empty);
    }

    [TestMethod]
    public void AgregarParametro_CuandoParametrosEsNull_DebeInicializarListaYAgregar()
    {
        _metodo.Parametros = null;

        _metodoRepositorioMock!
            .Setup(r => r.Obtener(_metodo.Id))
            .Returns(_metodo);

        _parametroServicioMock!
            .Setup(s => s.ObtenerParametroPorId(_parametro.Id))
            .Returns(_parametro);

        _metodoRepositorioMock!
            .Setup(r => r.Actualizar(_metodo));

        _metodoServicio!.AgregarParametro(_metodo.Id, _parametro.Id);

        Assert.IsTrue(_metodo.Parametros!.Contains(_parametro));
        _metodoRepositorioMock.Verify(r => r.Actualizar(_metodo), Times.Once);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNoEncontradaExcepcion))]
    public void AgregarParametro_CuandoParametroNoExiste_DebeLanzarInvalidOperationException()
    {
        _metodoRepositorioMock!
            .Setup(r => r.Obtener(_metodo.Id))
            .Returns(_metodo);

        _parametroServicioMock!
            .Setup(s => s.ObtenerParametroPorId(_parametro.Id))
            .Returns<Parametro>(null);

        _metodoServicio!.AgregarParametro(_metodo.Id, _parametro.Id);
    }

    [TestMethod]
    public void AgregarVariableLocal_Valida_DebeAgregarla()
    {
        _metodoRepositorioMock!
            .Setup(r => r.Obtener(_metodo.Id))
            .Returns(_metodo);

        _variableLocalServicioMock!
            .Setup(s => s.ObtenerVariableLocalPorId(_variableLocal.Id))
            .Returns(_variableLocal);

        _metodoRepositorioMock.Setup(r => r.Actualizar(_metodo));

        _metodoServicio!.AgregarVariableLocal(_metodo.Id, _variableLocal.Id);

        Assert.IsTrue(_metodo.VariablesLocales.Contains(_variableLocal));
        _metodoRepositorioMock.Verify(r => r.Actualizar(_metodo), Times.Once);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNoEncontradaExcepcion))]
    public void AgregarVariableLocal_CuandoMetodoNoExiste_DebeLanzarExcepcion()
    {
        var idMetodoInexistente = Guid.NewGuid();
        var idVariable = _variableLocal.Id;

        _metodoRepositorioMock!
            .Setup(r => r.Obtener(idMetodoInexistente))
            .Returns<Metodo>(null);

        _metodoServicio!.AgregarVariableLocal(idMetodoInexistente, idVariable);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadYaExisteExcepcion))]
    public void AgregarVariableLocal_VariableYaExiste_DebeLanzarExcepcion()
    {
        _metodo.VariablesLocales = new List<VariableLocal> { _variableLocal };

        _metodoRepositorioMock!
            .Setup(r => r.Obtener(_metodo.Id))
            .Returns(_metodo);

        _variableLocalServicioMock!
            .Setup(s => s.ObtenerVariableLocalPorId(_variableLocal.Id))
            .Returns(_variableLocal);

        _metodoServicio!.AgregarVariableLocal(_metodo.Id, _variableLocal.Id);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNulaExcepcion))]
    public void AgregarVariableLocal_CuandoMetodoIdEsVacio_DebeLanzarArgumentException()
    {
        var variableId = Guid.NewGuid();

        _metodoServicio!.AgregarVariableLocal(Guid.Empty, variableId);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNulaExcepcion))]
    public void AgregarVariableLocal_CuandoVariableIdEsVacio_DebeLanzarArgumentException()
    {
        var metodoId = Guid.NewGuid();

        _metodoServicio!.AgregarVariableLocal(metodoId, Guid.Empty);
    }

    [TestMethod]
    public void AgregarVariableLocal_CuandoVariableEsNull_DebeInicializarListaYAgregar()
    {
        _metodo.VariablesLocales = null;

        _metodoRepositorioMock!
            .Setup(r => r.Obtener(_metodo.Id))
            .Returns(_metodo);

        _variableLocalServicioMock!
            .Setup(s => s.ObtenerVariableLocalPorId(_variableLocal.Id))
            .Returns(_variableLocal);

        _metodoRepositorioMock!
            .Setup(r => r.Actualizar(_metodo));

        _metodoServicio!.AgregarVariableLocal(_metodo.Id, _variableLocal.Id);

        Assert.IsTrue(_metodo.VariablesLocales!.Contains(_variableLocal));
        _metodoRepositorioMock.Verify(r => r.Actualizar(_metodo), Times.Once);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNoEncontradaExcepcion))]
    public void AgregarVariableLocal_CuandoVariableLocalNoExiste_DebeLanzarInvalidOperationException()
    {
        _metodoRepositorioMock!
            .Setup(r => r.Obtener(_metodo.Id))
            .Returns(_metodo);

        _variableLocalServicioMock!
            .Setup(s => s.ObtenerVariableLocalPorId(_variableLocal.Id))
            .Returns<VariableLocal>(null);

        _metodoServicio!.AgregarVariableLocal(_metodo.Id, _variableLocal.Id);
    }

    [TestMethod]
    public void AgregarMetodoInvocado_Valido_DebeAgregarlo()
    {
        _metodoRepositorioMock!
            .Setup(r => r.Obtener(_metodo.Id))
            .Returns(_metodo);

        _metodoRepositorioMock!
            .Setup(r => r.Obtener(_metodoInvocado.Id))
            .Returns(_metodoInvocado);

        _metodoRepositorioMock.Setup(r => r.Actualizar(_metodo));

        _metodoServicio!.AgregarMetodoInvocado(_metodo.Id, _metodoInvocado.Id);

        Assert.IsTrue(_metodo.MetodosInvocados.Contains(_metodoInvocado));
        _metodoRepositorioMock.Verify(r => r.Actualizar(_metodo), Times.Once);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNoEncontradaExcepcion))]
    public void AgregarMetodoInvocado_CuandoMetodoOrigenNoExiste_DebeLanzarExcepcion()
    {
        var idMetodoOrigenInexistente = Guid.NewGuid();
        var idMetodoInvocado = _metodoInvocado.Id;

        _metodoRepositorioMock!
            .Setup(r => r.Obtener(idMetodoOrigenInexistente))
            .Returns<Metodo>(null);

        _metodoServicio!.AgregarMetodoInvocado(idMetodoOrigenInexistente, idMetodoInvocado);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadYaExisteExcepcion))]
    public void AgregarMetodoInvocado_MetodoInvocadoYaExiste_DebeLanzarExcepcion()
    {
        _metodo.MetodosInvocados = new List<Metodo> { _metodoInvocado };

        _metodoRepositorioMock!
            .Setup(r => r.Obtener(_metodo.Id))
            .Returns(_metodo);

        _metodoRepositorioMock!
            .Setup(r => r.Obtener(_metodoInvocado.Id))
            .Returns(_metodoInvocado);

        _metodoServicio!.AgregarMetodoInvocado(_metodo.Id, _metodoInvocado.Id);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNulaExcepcion))]
    public void AgregarMetodoInvocado_CuandoMetodoOrigenIdEsVacio_DebeLanzarArgumentException()
    {
        var metodoInvocadoId = Guid.NewGuid();

        _metodoServicio!.AgregarMetodoInvocado(Guid.Empty, metodoInvocadoId);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNulaExcepcion))]
    public void AgregarMetodoInvocado_CuandoMetodoInvocadoIdEsVacio_DebeLanzarArgumentException()
    {
        var metodoId = Guid.NewGuid();

        _metodoServicio!.AgregarMetodoInvocado(metodoId, Guid.Empty);
    }

    [TestMethod]
    public void AgregarMetodoInvocado_CuandoMetodosInvocadosEsNull_DebeInicializarListaYAgregar()
    {
        _metodo.MetodosInvocados = null;

        _metodoRepositorioMock!
            .Setup(r => r.Obtener(_metodo.Id))
            .Returns(_metodo);

        _metodoRepositorioMock!
            .Setup(r => r.Obtener(_metodoInvocado.Id))
            .Returns(_metodoInvocado);

        _metodoRepositorioMock!
            .Setup(r => r.Actualizar(_metodo));

        _metodoServicio!.AgregarMetodoInvocado(_metodo.Id, _metodoInvocado.Id);

        Assert.IsTrue(_metodo.MetodosInvocados!.Contains(_metodoInvocado));
        _metodoRepositorioMock.Verify(r => r.Actualizar(_metodo), Times.Once);
    }

    [TestMethod]
    [ExpectedException(typeof(EntidadNoEncontradaExcepcion))]
    public void AgregarMetodoInvocado_CuandoMetodoInvocadoNoExiste_DebeLanzarException()
    {
        _metodoRepositorioMock!
            .Setup(r => r.Obtener(_metodo.Id))
            .Returns(_metodo);

        _metodoRepositorioMock!
            .Setup(r => r.Obtener(_metodoInvocado.Id))
            .Returns<Metodo>(null);

        _metodoServicio!.AgregarMetodoInvocado(_metodo.Id, _metodoInvocado.Id);
    }

    [TestMethod]
    public void ActualizarMetodo_CuandoMetodoValido_ActualizaCorrectamente()
    {
        var clase = new Clase { Nombre = "Clase1" };
        var tipoDato = new TipoDato("int", null);

        var metodo = new Metodo("Metodo1", tipoDato, false, false, Accesibilidad.Public,
            clase, new List<Parametro>(), new List<VariableLocal>(), new List<Metodo>());

        _metodoRepositorioMock!
            .Setup(r => r.Actualizar(metodo))
            .Verifiable();

        _metodoServicio!.ActualizarMetodo(metodo);

        _metodoRepositorioMock.Verify(r => r.Actualizar(metodo), Times.Once);
    }

    [TestMethod]
    [ExpectedException(typeof(OperacionNoPermitidaExcepcion))]
    public void AgregarMetodoVirtualYOverride_DebeLanzarExcepcion()
    {
        _metodo.EsVirtual = true;
        _metodo.EsOverride = true;

        _metodoServicio!.AgregarMetodo(_metodo);
    }

    [TestMethod]
    [ExpectedException(typeof(OperacionNoPermitidaExcepcion))]
    public void AgregarMetodo_CuandoEsEstaticoVirtual_DebeLanzarExcepcion()
    {
        _metodo.EsEstatico = true;
        _metodo.EsVirtual = true;

        _metodoServicio!.AgregarMetodo(_metodo);
    }

    [TestMethod]
    [ExpectedException(typeof(OperacionNoPermitidaExcepcion))]
    public void AgregarMetodo_CuandoEsEstaticoOverride_DebeLanzarExcepcion()
    {
        _metodo.EsEstatico = true;
        _metodo.EsOverride = true;

        _metodoServicio!.AgregarMetodo(_metodo);
    }

    [TestMethod]
    [ExpectedException(typeof(OperacionNoPermitidaExcepcion))]
    public void AgregarMetodo_CuandoEsEstaticoYEsAbstract_DebeLanzarExcepcion()
    {
        _metodo.EsEstatico = true;
        _metodo.EsAbstract = true;

        _metodoServicio!.AgregarMetodo(_metodo);
    }

    [TestMethod]
    [ExpectedException(typeof(OperacionNoPermitidaExcepcion))]
    public void AgregarMetodo_CuandoEsEstaticoYEsSealed_DebeLanzarExcepcion()
    {
        _metodo.EsEstatico = true;
        _metodo.EsSealed = true;

        _metodoServicio!.AgregarMetodo(_metodo);
    }

    [TestMethod]
    [ExpectedException(typeof(OperacionNoPermitidaExcepcion))]
    public void AgregarMetodo_CuandoEsAbstractoYEsVirtual_DebeLanzarExcepcion()
    {
        _metodo.EsAbstract = true;
        _metodo.EsVirtual = true;

        _metodoServicio!.AgregarMetodo(_metodo);
    }

    [TestMethod]
    [ExpectedException(typeof(OperacionNoPermitidaExcepcion))]
    public void AgregarMetodo_CuandoEsAbstractoYEsOverride_DebeLanzarExcepcion()
    {
        _metodo.EsAbstract = true;
        _metodo.EsOverride = true;

        _metodoServicio!.AgregarMetodo(_metodo);
    }

    [TestMethod]
    [ExpectedException(typeof(OperacionNoPermitidaExcepcion))]
    public void AgregarMetodo_CuandoEsSealedYNoEsOverride_DebeLanzarExcepcion()
    {
        _metodo.EsSealed = true;
        _metodo.EsOverride = false;

        _metodoServicio!.AgregarMetodo(_metodo);
    }

    [TestMethod]
    [ExpectedException(typeof(OperacionNoPermitidaExcepcion))]
    public void AgregarMetodo_CuandoEsAbstractoYTieneVariablesLocales_DebeLanzarExcepcion()
    {
        _metodo.EsAbstract = true;
        _metodo.VariablesLocales!.Add(_variableLocal);

        _metodoServicio!.AgregarMetodo(_metodo);
    }

    [TestMethod]
    [ExpectedException(typeof(OperacionNoPermitidaExcepcion))]
    public void AgregarMetodo_CuandoEsAbstractoYTieneMetodosInvocados_DebeLanzarExcepcion()
    {
        _metodo.EsAbstract = true;
        _metodo.MetodosInvocados!.Add(_metodoInvocado);

        _metodoServicio!.AgregarMetodo(_metodo);
    }

    [TestMethod]
    public void ObtenerTodosMetodos_CuandoHayMetodos_DevuelveCantidadCorrecta()
    {
        var lista = new List<Metodo> { _metodo };
        _metodoRepositorioMock!
            .Setup(r => r.ObtenerTodos())
            .Returns(lista);

        var resultado = _metodoServicio!.ObtenerTodosMetodos();

        Assert.AreEqual(1, resultado.Count);
    }

    [TestMethod]
    public void ObtenerTodosMetodos_CuandoHayMetodos_ContieneMetodoEsperado()
    {
        var lista = new List<Metodo> { _metodo };
        _metodoRepositorioMock!
            .Setup(r => r.ObtenerTodos())
            .Returns(lista);

        var resultado = _metodoServicio!.ObtenerTodosMetodos();

        Assert.IsTrue(resultado.Contains(_metodo));
    }

    [TestMethod]
    public void ObtenerTodosMetodos_CuandoNoHayMetodos_DevuelveListaVacia()
    {
        var lista = new List<Metodo>();
        _metodoRepositorioMock!
            .Setup(r => r.ObtenerTodos())
            .Returns(lista);

        var resultado = _metodoServicio!.ObtenerTodosMetodos();

        Assert.AreEqual(0, resultado.Count);
    }

    [TestMethod]
    public void ObtenerTodosMetodos_LlamaAlRepositorioUnaVez()
    {
        _metodoRepositorioMock!
            .Setup(r => r.ObtenerTodos())
            .Returns(new List<Metodo>());

        _metodoServicio!.ObtenerTodosMetodos();

        _metodoRepositorioMock.Verify(r => r.ObtenerTodos(), Times.Once);
    }

    [TestMethod]
    [ExpectedException(typeof(OperacionNoPermitidaExcepcion))]
    public void AgregarVariableLocal_CuandoMetodoEsAbstracto_DebeLanzarOperacionNoPermitidaExcepcion()
    {
        _metodo.EsAbstract = true;
        _metodoRepositorioMock!
            .Setup(r => r.Obtener(_metodo.Id))
            .Returns(_metodo);

        _metodoServicio!.AgregarVariableLocal(_metodo.Id, _variableLocal.Id);
    }

    [TestMethod]
    [ExpectedException(typeof(OperacionNoPermitidaExcepcion))]
    public void AgregarMetodoInvocado_CuandoMetodoEsAbstracto_DebeLanzarOperacionNoPermitidaExcepcion()
    {
        _metodo.EsAbstract = true;
        _metodoRepositorioMock!
            .Setup(r => r.Obtener(_metodo.Id))
            .Returns(_metodo);

        _metodoServicio!.AgregarMetodoInvocado(_metodo.Id, _metodoInvocado.Id);
    }
}
