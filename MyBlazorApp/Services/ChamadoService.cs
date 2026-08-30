using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Forms;
using MyBlazorApp.Dto;
using MyBlazorApp.interfaces;
using MyBlazorApp.Models;
using MyBlazorApp.Models.DTO;

namespace MyBlazorApp.Services;

public class ChamadoService(HttpClient httpClient, TokenProvider tokenProvider) : IChamadoService
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public async Task<PagedResponse<Chamado>?> ListarChamadosAsync(int page = 1, int pageSize = 10)
    {
        var response = await httpClient.GetAsync(
            $"Chamado/GetTasks?page={page}&pageSize={pageSize}"
        );

        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<PagedResponse<Chamado>>(JsonOptions);
    }

    public async Task<List<Chamado>?> ListarChamadosFiltradosAsync(FilterTasksDto filter)
    {
        var query = BuildFilterQuery(filter);
        var response = await httpClient.GetAsync($"Chamado/GetTasksFiltered?{query}");

        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<List<Chamado>>(JsonOptions);
    }

    public async Task<(int numeroChamado, bool foiCriado)> CriarChamadoAsync(TaskDto task)
    {
        try
        {
            var response = await httpClient.PostAsJsonAsync("Chamado", task);
            var value = Convert.ToInt32(await response.Content.ReadAsStringAsync());
            return (value, response.IsSuccessStatusCode);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<bool> AtualizarChamadoAsync(int id, TaskDto task)
    {
        var content = new StringContent(
            JsonSerializer.Serialize(task),
            Encoding.UTF8,
            "application/json"
        );

        var response = await httpClient.PatchAsync($"Chamado/UpdateTask/{id}", content);
        return response.IsSuccessStatusCode;
    }

    private static string BuildFilterQuery(FilterTasksDto filter)
    {
        var parametros = new List<string>();

        if (filter.TaskId.HasValue)
            parametros.Add($"taskId={filter.TaskId}");
        if (filter.UserId.HasValue)
            parametros.Add($"userId={filter.UserId}");
        if (filter.IssuerId.HasValue)
            parametros.Add($"issuerId={filter.IssuerId}");
        if (filter.Status.HasValue)
            parametros.Add($"status={(int)filter.Status}");
        if (filter.CreationDate.HasValue)
            parametros.Add($"creationDate={filter.CreationDate.Value:yyyy-MM-dd}");
        if (filter.DueDate.HasValue)
            parametros.Add($"dueDate={filter.DueDate.Value:yyyy-MM-dd}");

        return string.Join("&", parametros);
    }

    public async Task<string?> AutenticarAsync(string email, string password)
    {
        var content = new StringContent(
            JsonSerializer.Serialize(new { email, password }),
            Encoding.UTF8,
            "application/json"
        );

        var response = await httpClient.PostAsync("/Auth/login", content);

        return response.IsSuccessStatusCode ? await response.Content.ReadAsStringAsync() : null;
    }

    public async Task<List<ChamadoApiResponse>?> ObterDadosViewChamadoAsync()
    {
        try
        {
            var response = await httpClient.GetFromJsonAsync<List<ChamadoApiResponse>>("/Chamado/GetAllToTaskPage");
            return response;
        }
        catch (Exception ex) { return null; }

    }

    public async Task<HttpStatusCode> RegistrarAsync(string name, string email, string password)
    {
        var content = new StringContent(
            JsonSerializer.Serialize(new { name, email, password }),
            Encoding.UTF8,
            "application/json"
        );
        var response = await httpClient.PostAsync("/Auth/register", content);
        return response.StatusCode;
    }

    public async Task<ChamadoApiResponse?> ObterChamadoPorIdAsync(int id)
    {
        var res = await httpClient.GetFromJsonAsync<ChamadoApiResponse>($"Chamado/{id}");
        return res;
    }

    public async Task<IEnumerable<Grupo>?> ObterGruposAsync()
    {
        try
        {
            var data = await httpClient.GetFromJsonAsync<IEnumerable<Grupo>>("Grupo/");
            return data;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }

    public async Task<IEnumerable<UsuarioDto>?> ObterUsuariosAsync()
    {
        return await httpClient.GetFromJsonAsync<IEnumerable<UsuarioDto>>("Usuario/listar-usuarios");
    }

    public async Task<bool> ValidarTokenAsync()
    {
        if (string.IsNullOrWhiteSpace(tokenProvider.Jwt)) return false;
        var tokenSerialized = JsonSerializer.Deserialize<LoginResponse>(tokenProvider.Jwt);

        if (tokenSerialized is null) return false;

        var token = tokenSerialized.token;

        if (token == null) return false;

        var req = new StringContent(
            JsonSerializer.Serialize(new { token }),
            Encoding.UTF8,
            "application/json"
        );

        var res = await httpClient.PostAsync("Auth/validar-token", req);

        if (!res.IsSuccessStatusCode) return false;

        var result = await res.Content.ReadFromJsonAsync<bool>();

        return result;
    }

    public async Task<(int id, bool usuariosAdicionados)> CriarGrupoAsync(string Nome, IReadOnlyCollection<UsuarioDto>? Usuarios)
    {
        var req = new StringContent(JsonSerializer.Serialize(new { Nome }), Encoding.UTF8, "application/json");

        var res = await httpClient.PostAsync("Grupo", req);

        var grupoId = Convert.ToInt32(await res.Content.ReadAsStringAsync());
        var usuariosadicinados = false;

        if (Usuarios != null)
        {
            var usuariosId = Usuarios.Select(x => x.Id);
            var adicionarUsuariosReq = new StringContent(JsonSerializer.Serialize(new { grupoId, usuariosId }), Encoding.UTF8, "application/json");
            var adicionarUsuariosRes = await httpClient.PostAsync("GrupoUsuario/bulk", adicionarUsuariosReq);
            if (adicionarUsuariosRes.IsSuccessStatusCode) usuariosadicinados = true;
        }


        return (grupoId, usuariosadicinados);
    }

    public async Task<bool> UploadArquivosAsync(int chamadoId, IReadOnlyList<IBrowserFile> arquivos)
    {
        const long MaxFileSize = 10485760;
        using var content = new MultipartFormDataContent();

        foreach (var arquivo in arquivos)
        {
            var fileContent = new StreamContent(arquivo.OpenReadStream(MaxFileSize));
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(arquivo.ContentType);
            content.Add(fileContent, "arquivos", arquivo.Name);
        }

        var response = await httpClient.PostAsync($"Upload/{chamadoId}", content);

        return response.IsSuccessStatusCode;
    }
}