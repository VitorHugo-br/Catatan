using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using MyBlazorApp.interfaces;
using MyBlazorApp.Models;
using MyBlazorApp.Models.DTO;
using MyBlazorApp.Utils;

namespace MyBlazorApp.Services;

public class MyTaskService(HttpClient httpClient, RequestUtil reqU) : IMyTaskService
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public async Task<PagedResponse<MyTask>?> GetTasksAsync(int page = 1, int pageSize = 10)
    {
        var response = await httpClient.GetAsync(
            $"MyTasks/GetTasks?page={page}&pageSize={pageSize}"
        );

        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<PagedResponse<MyTask>>(JsonOptions);
    }

    public async Task<List<MyTask>?> GetTasksFilteredAsync(FilterTasksDto filter)
    {
        var query = BuildFilterQuery(filter);
        var response = await httpClient.GetAsync($"MyTasks/GetTasksFiltered?{query}");

        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<List<MyTask>>(JsonOptions);
    }

    public async Task<bool> CreateTaskAsync(TaskDto task)
    {
        try
        {
            var token = await reqU.GetTokenFromSessionStorage();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Value);
            var response = await httpClient.PostAsJsonAsync("MyTasks/CreateTask", task);
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

        var response = await httpClient.PatchAsync($"MyTasks/UpdateTask/{id}", content);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> CreateTaskBulkAsync(IEnumerable<TaskDto> tasks)
    {
        var response = await httpClient.PostAsJsonAsync("MyTasks/CreateTaskBulk", tasks);
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
}