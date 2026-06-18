using MyBlazorApp.Models;
using MyBlazorApp.Models.DTO;

namespace MyBlazorApp.interfaces;

public interface IMyTaskService
{
    Task<Models.PagedResponse<MyTask>?> GetTasksAsync(int page = 1, int pageSize = 10);
    Task<List<MyTask>?> GetTasksFilteredAsync(FilterTasksDto filter);
    Task<bool> CreateTaskAsync(TaskDto task);
    Task<bool> UpdateTaskAsync(int id, TaskDto task);
    Task<bool> CreateTaskBulkAsync(IEnumerable<TaskDto> tasks);
}