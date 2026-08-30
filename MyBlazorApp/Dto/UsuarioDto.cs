namespace MyBlazorApp.Dto;

public class UsuarioDto(int id, string nome) : IEquatable<UsuarioDto>
{
    public int Id { get; set; } = id;
    public string Nome { get; set; } = nome;

    public bool Equals(UsuarioDto? other)
    {
        if (other is null) return false;
        return Id == other.Id;
    }

    public override bool Equals(object? obj) => Equals(obj as UsuarioDto);

    public override int GetHashCode() => Id.GetHashCode();

    public override string ToString() => Nome;
}