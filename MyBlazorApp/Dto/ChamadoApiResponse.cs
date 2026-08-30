namespace MyBlazorApp.Models.DTO;

public class ChamadoApiResponse
{
    public int Id { get; set; }
    public Guid Guid { get; set; }
    public string Titulo { get; set; }
    public string Descricao { get; set; }
    public Status Status { get; set; }
    public DateTime? DataInicio { get; set; } = null;
    public DateTime? DataTermino { get; set; } = null;
    public DateTime? Prazo { get; set; }
    public int RemetenteId { get; set; }
    public Usuario Remetente { get; set; }
    public int? ResponsavelId { get; set; }
    public Usuario? Responsavel { get; set; }
    public DateTime CriadoEm { get; set; }
    public TimeSpan HorasGastas { get; set; }
}

public class Usuario
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Senha { get; set; }
    public string Role { get; set; }
}
