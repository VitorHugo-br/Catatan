using MyBlazorApp.Models;
using MyBlazorApp.Models.DTO;
using System.Net;

namespace MyBlazorApp.interfaces;

public interface IChamadoService
{
    Task<PagedResponse<Chamado>?> GetTasksAsync(int page = 1, int pageSize = 10);

    Task<List<Chamado>?> GetTasksFilteredAsync(FilterTasksDto filter);

    Task<bool> CreateTaskAsync(TaskDto task);

    Task<bool> UpdateTaskAsync(int id, TaskDto task);

    Task<bool> CreateTaskBulkAsync(IEnumerable<TaskDto> tasks);

    Task<string?> AutenticarAsync(string email, string password);

    Task<HttpStatusCode> RegistrarAsync(string name, string email, string password);

    Task<List<TaskApiResponse>?> GetDataForTasksPage();

    Task<TaskApiResponse?> GetChamadoPorIdAsync(int id);
}