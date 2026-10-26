using System.Text.Json.Nodes;
using Reflection;
using Reflection.TransformadorHtml;

namespace ReflectionTest;

[TestClass]
public class TransformadorHtmlTest
{
    private ITransformador? _transformador;

    [TestInitialize]
    public void Inicializar()
    {
        _transformador = new TransformadorHtmlServicio();
    }

    [TestMethod]
    public void Transformar_DeberiaIncluirListaRaizSinAnidados()
    {
        var json = JsonNode.Parse("""
                                  {
                                      "LlamadaInicial": "Clase1.metodo1()",
                                      "MetodosEjecutados": []
                                  }
                                  """)!.AsObject();

        var resultado = _transformador.Transformar(json);
        var cantidadLi = resultado.Split("<li>").Length - 1;

        Assert.AreEqual(1, cantidadLi);
    }

    [TestMethod]
    public void Transformar_DeberiaIncluirLlamadasAnidadas()
    {
        var json = JsonNode.Parse("""
                                  {
                                      "LlamadaInicial": "Clase1.metodo1()",
                                      "MetodosEjecutados": [
                                          {
                                              "LlamadaInicial": "this.Metodo2()",
                                              "MetodosEjecutados": [
                                                  {
                                                      "LlamadaInicial": "obj2.Metodo3()",
                                                      "MetodosEjecutados": []
                                                  }
                                              ]
                                          }
                                      ]
                                  }
                                  """)!.AsObject();

        var resultado = _transformador.Transformar(json);
        var cantidadLi = resultado.Split("<li>").Length - 1;

        Assert.AreEqual(3, cantidadLi);
    }

    [TestMethod]
    public void Transformar_DeberiaUsarTextoPorDefectoSiFaltaLlamadaInicial()
    {
        var json = JsonNode.Parse("""
                                  {
                                      "MetodosEjecutados": []
                                  }
                                  """)!.AsObject();

        var resultado = _transformador.Transformar(json);

        Assert.IsTrue(resultado.Contains("Llamada desconocida"));
    }

    [TestMethod]
    public void ProcesarLlamadas_DeberiaProcesarLlamadasSimples()
    {
        var json = JsonNode.Parse("""
                                  {
                                      "LlamadaInicial": "Raiz()",
                                      "MetodosEjecutados": [
                                          "this.Metodo1()",
                                          "this.Metodo2()"
                                      ]
                                  }
                                  """)!.AsObject();

        var resultado = _transformador.Transformar(json);
        var cantidadLi = resultado.Split("<li>").Length - 1;

        Assert.AreEqual(3, cantidadLi);
    }

    [TestMethod]
    public void ProcesarLlamadas_DeberiaUsarTextoPorDefectoSiFaltaLlamadaInicial()
    {
        var json = JsonNode.Parse("""
                                  {
                                      "LlamadaInicial": "Principal()",
                                      "MetodosEjecutados": [
                                          {
                                              "MetodosEjecutados": []
                                          }
                                      ]
                                  }
                                  """)!.AsObject();

        var resultado = _transformador.Transformar(json);

        Assert.IsTrue(resultado.Contains("(sin texto)"));
    }

    [TestMethod]
    public void Nombre_DeberiaSerHtml()
    {
        var transformador = new TransformadorHtmlServicio();

        Assert.AreEqual("HTML", transformador.Nombre);
    }
}
