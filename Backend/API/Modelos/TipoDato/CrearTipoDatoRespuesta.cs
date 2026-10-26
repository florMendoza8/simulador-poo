namespace API.Modelos.TipoDato;
using Dominio;

public class CrearTipoDatoRespuesta(TipoDato tipoDato)
{
    public Guid Id { get; set; } = tipoDato.Id;
}
