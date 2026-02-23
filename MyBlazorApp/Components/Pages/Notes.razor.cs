using MyBlazorApp.Models;
using Microsoft.AspNetCore.Components;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using RestSharp.Authenticators;

namespace MyBlazorApp.Components.Pages
{
    public partial class Notes()
    {

        [Inject]
        private ProtectedSessionStorage Pss { get; set; } = default!;

        [Parameter]
        public SearchNoteModel SearchNoteModel { get; set; } = new();
        public List<TaskResponse> Tasks { get; set; } = [];
        public List<UserResponse> Issuers { get; set; } = [];
        public List<UserResponse> Users { get; set; } = [];

        private string token = string.Empty;

        protected override async Task OnInitializedAsync()
        {

            await FetchUsers();
            await FetchIssuers();
            await FetchTasks();

        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            var result = await Pss.GetAsync<string>("authToken");
            if (result.Success && result.Value is not null)
            {
                token = result.Value;
            }
            else
            {
                Console.WriteLine("Failed to retrieve auth token from session storage.");
            }
        }

        private async Task FetchUsers()
        {
            var apiUrl = "https://localhost:7049";
            var client = new RestClient(apiUrl, configureSerialization: c => c.UseNewtonsoftJson());
            var request = new RestRequest("/User/listUsers", Method.Get);
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
            var apiUrl = "https://localhost:7049";
            var client = new RestClient(apiUrl, configureSerialization: c => c.UseNewtonsoftJson());
            var request = new RestRequest("/User/listIssuers", Method.Get);
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
            var apiUrl = "https://localhost:7049";
            if (string.IsNullOrEmpty(token))
            {
                Console.WriteLine("Auth token is missing. Cannot fetch tasks.");
                return;
            }
            var authenticator = new JwtAuthenticator(token);
            var clientOptions = new RestClientOptions(apiUrl)
            {
                Authenticator = authenticator
            };
            var client = new RestClient(clientOptions, configureSerialization: c => c.UseNewtonsoftJson());
            var request = new RestRequest("/MyTasks/GetTasks", Method.Get);
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


        private void HandleSubmit()
        {
            var apiUrl = "https://localhost:7049";
            if (string.IsNullOrEmpty(token))
            {
                Console.WriteLine("Auth token is missing. Cannot fetch tasks.");
                return;
            }
            var authenticator = new JwtAuthenticator(token);
            var clientOptions = new RestClientOptions(apiUrl)
            {
                Authenticator = authenticator
            };
            var client = new RestClient(clientOptions, configureSerialization: c => c.UseNewtonsoftJson());
            var request = new RestRequest("/MyTasks/GetTasksFiltered", Method.Get);

            if (!string.IsNullOrEmpty(SearchNoteModel.TaskID))
            {
                request.AddQueryParameter("taskID", SearchNoteModel.TaskID);
            }
            if (!string.IsNullOrEmpty(SearchNoteModel.UserID))
            {
                request.AddQueryParameter("userID", SearchNoteModel.UserID);
            }
            if (!string.IsNullOrEmpty(SearchNoteModel.TaskIssuerID))
            {
                request.AddQueryParameter("taskIssuerID", SearchNoteModel.TaskIssuerID);
            }
            if (SearchNoteModel.DueDate.HasValue)
            {
                request.AddQueryParameter("dueDate", SearchNoteModel.DueDate.Value.ToString("o"));
            }
            if (SearchNoteModel.CreatedAt.HasValue)
            {
                request.AddQueryParameter("createdAt", SearchNoteModel.CreatedAt.Value.ToString("o"));
            }
            if (SearchNoteModel.Status.HasValue)
            {
                request.AddQueryParameter("status", SearchNoteModel.Status.Value.ToString());
            }

            var response = client.GetAsync<List<TaskResponse>>(request).Result;
            if (response != null)
            {
                Tasks = response;
            }
            else
            {
                Console.WriteLine("Failed to fetch filtered tasks.");
            }
        }
    }
}