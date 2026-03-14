using MyBlazorApp.Models;
using Microsoft.AspNetCore.Components;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using MyBlazorApp.Utils;
using RestSharp.Authenticators;

namespace MyBlazorApp.Components.Pages
{
    public partial class Notes(RequestUtil tku)
    {
        [Inject] private ProtectedSessionStorage Pss { get; set; } = null!;

        [Parameter] public SearchNoteModel SearchNoteModel { get; set; } = new();
        private List<TaskResponse> Tasks { get; set; } = [];
        private List<UserResponse> Issuers { get; set; } = [];
        private List<UserResponse> Users { get; set; } = [];

        private string _token = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            await FetchUsers();
            await FetchIssuers();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var result = await Pss.GetAsync<string>("authToken");
                if (result is { Success: true, Value: not null })
                {
                    _token = result.Value;
                    await FetchTasks();
                    StateHasChanged(); 
                }
            }
        }

        private async Task FetchUsers()
        {
            var client = await tku.ConfigAuthorizationBeforeRequest();
            if (client is null) return;
            var request = new RestRequest("/User/listUsers");
            var response = await client.GetAsync<List<UserResponse>>(request);
            if (response != null)
            {
                Users = response;
            }
            else
            {
                Console.WriteLine("Failed to fetch users.");
            }
        }

        private async Task FetchIssuers()
        {
            const string apiUrl = "https://localhost:7049";
            var client = new RestClient(apiUrl, configureSerialization: c => c.UseNewtonsoftJson());
            var request = new RestRequest("/User/listIssuers");
            var response = await client.GetAsync<List<UserResponse>>(request);
            if (response != null)
            {
                Issuers = response;
            }
            else
            {
                Console.WriteLine("Failed to fetch issuers.");
            }
        }

        private async Task FetchTasks()
        {
            const string apiUrl = "https://localhost:7049";
            if (string.IsNullOrEmpty(_token))
            {
                Console.WriteLine("Auth token is missing. Cannot fetch tasks.");
                return;
            }

            var authenticator = new JwtAuthenticator(_token);
            var clientOptions = new RestClientOptions(apiUrl)
            {
                Authenticator = authenticator
            };
            var client = new RestClient(clientOptions, configureSerialization: c => c.UseNewtonsoftJson());
            var request = new RestRequest("/MyTasks/GetTasks");
            var response = await client.GetAsync<List<TaskResponse>>(request);
            if (response != null)
            {
                Tasks = response;
            }
            else
            {
                Console.WriteLine("Failed to fetch tasks.");
            }
        }


        private async Task HandleSubmit()
        {
            if (string.IsNullOrEmpty(_token))
            {
                Console.WriteLine("Auth token is missing. Cannot fetch tasks.");
                return;
            }

            var client = await tku.ConfigAuthorizationBeforeRequest();
            if (client is null) return;
            var request = new RestRequest("/MyTasks/GetTasksFiltered");

            if (!string.IsNullOrEmpty(SearchNoteModel.TaskID))
                request.AddQueryParameter("taskID", SearchNoteModel.TaskID);

            if (!string.IsNullOrEmpty(SearchNoteModel.UserID))
                request.AddQueryParameter("userID", SearchNoteModel.UserID);

            if (!string.IsNullOrEmpty(SearchNoteModel.TaskIssuerID))
                request.AddQueryParameter("IssuerId", SearchNoteModel.TaskIssuerID);

            if (SearchNoteModel.DueDate.HasValue)
                request.AddQueryParameter("dueDate", SearchNoteModel.DueDate.Value.ToString("o"));

            if (SearchNoteModel.CreatedAt.HasValue)
                request.AddQueryParameter("createdAt", SearchNoteModel.CreatedAt.Value.ToString("o"));

            if (SearchNoteModel.Status.HasValue)
                request.AddQueryParameter("status", SearchNoteModel.Status.Value.ToString());

            var response = await client.GetAsync<List<TaskResponse>>(request);

            if (response != null)
            {
                Tasks = response;
                StateHasChanged();
            }
            else
            {
                Console.WriteLine("Failed to fetch filtered tasks.");
            }
        }
    }
}