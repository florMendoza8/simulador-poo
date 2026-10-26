namespace API.Modelos.VariableLocal;

public class CrearVariableLocalRespuesta(Dominio.VariableLocal variableLocal)
{
    public Guid Id { get; set; } = variableLocal.Id;
}
