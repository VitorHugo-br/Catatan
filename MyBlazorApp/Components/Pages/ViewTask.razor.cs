
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.ResponseCompression;
using MyBlazorApp.Models;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Serializers.NewtonsoftJson;

namespace MyBlazorApp.Components.Pages
{
    public partial class ViewTask
    {

        [Inject]
        private ProtectedSessionStorage PSS { get; set; } = default!;

        [Parameter]
        public int TaskId { get; set; }

        private TaskResponse? MyTask { get; set; }

        private List<UserResponse> Users { get; set; } = [];

        public List<UserResponse> Issuers { get; set; } = [];

        private bool OpenModal { get; set; } = false;

        protected override async Task OnInitializedAsync()
        {
            await FetchTask();
            await FetchUsers();
            await FetchIssuers();
        }

        private async Task FetchTask()
        {
            var apiUrl = "https://localhost:7049";
            var token = await PSS.GetAsync<string>("authToken");
            var authenticator = new JwtAuthenticator(token.Value!);
            var clientOptions = new RestClientOptions(apiUrl) { Authenticator = authenticator };
            var restClient = new RestClient(clientOptions, configureSerialization: c => c.UseNewtonsoftJson());
            var req = new RestRequest("/MyTasks/GetTasksFiltered", Method.Get);
            req.AddQueryParameter("taskID", TaskId);
            var res = await restClient.GetAsync(req);

            if (res.IsSuccessful && res.Content != null)
            {
                MyTask = JsonConvert.DeserializeObject<List<TaskResponse>>(res.Content)?.FirstOrDefault();
            }
        }

        private async Task FetchUsers()
        {
            var apiUrl = "https://localhost:7049";
            var token = await PSS.GetAsync<string>("authToken");
            var authenticator = new JwtAuthenticator(token.Value!);
            var clientOptions = new RestClientOptions(apiUrl) { Authenticator = authenticator };
            var restClient = new RestClient(clientOptions, configureSerialization: c => c.UseNewtonsoftJson());
            var request = new RestRequest("/User/listUsers", Method.Get);
            var response = await restClient.GetAsync<List<UserResponse>>(request);
            if (response != null)
            {
                Users = response;
            }
        }

        private async Task FetchIssuers()
        {
            var apiUrl = "https://localhost:7049";
            var token = await PSS.GetAsync<string>("authToken");
            var authenticator = new JwtAuthenticator(token.Value!);
            var clientOptions = new RestClientOptions(apiUrl) { Authenticator = authenticator };
            var client = new RestClient(clientOptions, configureSerialization: c => c.UseNewtonsoftJson());
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
    }
}