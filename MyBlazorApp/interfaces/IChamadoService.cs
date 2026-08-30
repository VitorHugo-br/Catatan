using Microsoft.AspNetCore.Components.Forms;
using MyBlazorApp.Dto;
using MyBlazorApp.Models;
using MyBlazorApp.Models.DTO;
using System.Net;

namespace MyBlazorApp.interfaces;

public interface IChamadoService
{
    Task<PagedResponse<Chamado>?> ListarChamadosAsync(int page = 1, int pageSize = 10);

    Task<List<Chamado>?> ListarChamadosFiltradosAsync(FilterTasksDto filter);

    Task<(int numeroChamado, bool foiCriado)> CriarChamadoAsync(TaskDto task);

    Task<bool> AtualizarChamadoAsync(int id, TaskDto task);

    Task<string?> AutenticarAsync(string email, string password);

    Task<HttpStatusCode> RegistrarAsync(string name, string email, string password);

    Task<List<ChamadoApiResponse>?> ObterDadosViewChamadoAsync();

    Task<ChamadoApiResponse?> ObterChamadoPorIdAsync(int id);

    Task<IEnumerable<Grupo>?> ObterGruposAsync();

    Task<IEnumerable<UsuarioDto>?> ObterUsuariosAsync();

    Task<bool> ValidarTokenAsync();

    Task<(int id, bool usuariosAdicionados)> CriarGrupoAsync(string Nome, IReadOnlyCollection<UsuarioDto>? Usuarios);

    Task<bool> UploadArquivosAsync(int chamadoId, IReadOnlyList<IBrowserFile> arquivos);
}