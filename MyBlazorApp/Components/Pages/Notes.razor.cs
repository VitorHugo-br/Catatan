using MyBlazorApp.Models;
using Microsoft.AspNetCore.Components;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;

namespace MyBlazorApp.Components.Pages
{
    public partial class Notes : ComponentBase
    {
        [Parameter]
        public SearchNoteModel SearchNoteModel { get; set; } = new();
        //public List<TaskResponse> Tasks { get; set; } = [];
        public List<UserResponse> Issuers { get; set; } = [];
        public List<UserResponse> Users { get; set; } = [];

        protected override async Task OnInitializedAsync()
        {
            await FetchUsers();
            await FetchIssuers();

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


        private void HandleSubmit()
        {

        }
    }
}