using MyBlazorApp.Models.DTO;

namespace MyBlazorApp.Dto;

public record Grupo(int Id, string Nome, List<string> Usuarios);