using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using MyBlazorApp.Models;
using MyBlazorApp.Utils;
using Newtonsoft.Json;
using RestSharp;

namespace MyBlazorApp.Components.Pages
{
    public partial class ViewTask(RequestUtil tku)
    {

        [Parameter] public int TaskId { get; set; }

        private TaskResponse? MyTask { get; set; }

        private List<UserResponse> Users { get; set; } = [];

        private List<UserResponse> Issuers { get; set; } = [];

        private List<CommentModel> Comments { get; set; } = [];

        protected override async Task OnInitializedAsync()
        {
            await FetchTask();
            await FetchUsers();
            await FetchIssuers();
            await FetchComments();
        }

        private async Task FetchTask()
        {
            var restClient = await tku.ConfigAuthorizationBeforeRequest();
            var req = new RestRequest("/MyTasks/GetTasksFiltered", Method.Get);
            req.AddQueryParameter("taskID", TaskId);
            var res = await restClient.GetAsync(req);

            if (res is { IsSuccessful: true, Content: not null })
            {
                MyTask = JsonConvert.DeserializeObject<List<TaskResponse>>(res.Content)?.FirstOrDefault();
            }
        }

        private async Task FetchUsers()
        {
            var restClient = await tku.ConfigAuthorizationBeforeRequest();
            var request = new RestRequest("/User/listUsers", Method.Get);
            var response = await restClient.GetAsync<List<UserResponse>>(request);
            if (response != null)
            {
                Users = response;
            }
        }

        private async Task FetchIssuers()
        {
            var restClient = await tku.ConfigAuthorizationBeforeRequest();
            var request = new RestRequest("/User/listIssuers", Method.Get);
            var response = await restClient.GetAsync<List<UserResponse>>(request);
            if (response != null)
            {
                Issuers = response;
            }
            else
            {
                Console.WriteLine("Failed to fetch issuers.");
            }
        }
        
        private async Task Rerender(bool b)
        {
            await FetchComments();
            StateHasChanged();
        }

        private async Task FetchComments()
        {
            var restClient = await tku.ConfigAuthorizationBeforeRequest();
            var request = new RestRequest($"/Comments/GetCommentsByTask/{TaskId}", Method.Get);
            var response = await restClient.GetAsync<List<CommentModel>>(request);
            if (response != null)
            {
                Comments = response;
            }
            else
            {
                Console.WriteLine("Failed to fetch comments.");
            }
        }
    }
}