using Microsoft.AspNetCore.Components;
using MudBlazor;
using MyBlazorApp.interfaces;
using MyBlazorApp.Models;
using MyBlazorApp.Models.DTO;
using MyBlazorApp.Utils;
using RestSharp;

namespace MyBlazorApp.Components.Pages;

public partial class AdicionarTarefaDialog(IMyTaskService taskService, RequestUtil tku) : ComponentBase
{
    [Inject] private ISnackbar Snackbar { get; set; }
    
    private bool _success;
    private MudForm _form;
    private string[] _errors = [];
    
    private NewTaskFormModel _formModel = new();

    private UserResponse[] _userResponses;
    
    private List<UserResponse> Users { get; set; } = [];
    
    protected override async Task OnInitializedAsync()
    {
        await FetchUsers();
    }

    private async Task FetchUsers()
    {
        var client = await tku.ConfigAuthorizationBeforeRequest();
        if (client is null) return;
        var request = new RestRequest("/User/listUsers");
        var response = await client.GetAsync<List<UserResponse>>(request);
        if (response != null)
        {
            Users = response.OrderBy(u => u.Name).ToList();
        }
        else
        {
            Console.WriteLine("Failed to fetch users.");
        }
    }
    
    private async Task Submit()
    {
        var task = new TaskDto
        {
            Title = _formModel.Title,
            Description = _formModel.Description,
            DueDate = _formModel.DueDate,
            StartDate = DateTime.Now,
            Status = (Status)_formModel.Status,
            UserId = _formModel.UserId,
        };
        
        var created = await taskService.CreateTaskAsync(task);
        
        if (!created)
        {
            Snackbar.Add("Algo deu errado", Severity.Error);
            return;
        }
        
        Snackbar.Add("Tarefa criada com sucesso", Severity.Success);
    }
}