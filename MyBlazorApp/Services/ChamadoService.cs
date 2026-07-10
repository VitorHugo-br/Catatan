using System.Net;
using System.Text;
using System.Text.Json;
using MyBlazorApp.interfaces;
using MyBlazorApp.Models;
using MyBlazorApp.Models.DTO;

namespace MyBlazorApp.Services;

public class ChamadoService(HttpClient httpClient) : IChamadoService
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public async Task<PagedResponse<Chamado>?> GetTasksAsync(int page = 1, int pageSize = 10)
    {
        var response = await httpClient.GetAsync(
            $"Chamado/GetTasks?page={page}&pageSize={pageSize}"
        );

        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<PagedResponse<Chamado>>(JsonOptions);
    }

    public async Task<List<Chamado>?> GetTasksFilteredAsync(FilterTasksDto filter)
    {
        var query = BuildFilterQuery(filter);
        var response = await httpClient.GetAsync($"Chamado/GetTasksFiltered?{query}");

        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<List<Chamado>>(JsonOptions);
    }

    public async Task<bool> CreateTaskAsync(TaskDto task)
    {
        try
        {
            var response = await httpClient.PostAsJsonAsync("Chamado/CreateTask", task);
            return response.IsSuccessStatusCode;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<bool> UpdateTaskAsync(int id, TaskDto task)
    {
        var content = new StringContent(
            JsonSerializer.Serialize(task),
            Encoding.UTF8,
            "application/json"
        );

        var response = await httpClient.PatchAsync($"Chamado/UpdateTask/{id}", content);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> CreateTaskBulkAsync(IEnumerable<TaskDto> tasks)
    {
        var response = await httpClient.PostAsJsonAsync("Chamado/CreateTaskBulk", tasks);
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

    public async Task<List<TaskApiResponse>?> GetDataForTasksPage()
    {
        var response = await httpClient.GetFromJsonAsync<List<TaskApiResponse>>("/Chamado/GetAllToTaskPage");
        return response;
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

    public async Task<TaskApiResponse?> GetChamadoPorIdAsync(int id)
    {
        return await httpClient.GetFromJsonAsync<TaskApiResponse>($"Chamado/{id}");
    }
}