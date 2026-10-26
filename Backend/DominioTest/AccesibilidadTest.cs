using Dominio;
namespace DominioTest;

[TestClass]
public class AccesibilidadTest
{
    [TestMethod]
    public void Accesibilidad_DeberiaContenerValoresEsperados()
    {
        var valores = Enum.GetValues(typeof(Accesibilidad)).Cast<Accesibilidad>().ToList();

        CollectionAssert.Contains(valores, Accesibilidad.Public);
        CollectionAssert.Contains(valores, Accesibilidad.Protected);
        CollectionAssert.Contains(valores, Accesibilidad.Private);
    }
}
