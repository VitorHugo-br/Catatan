using Microsoft.AspNetCore.Components;
using MyBlazorApp.Models;
using MyBlazorApp.Utils;
using RestSharp;


namespace MyBlazorApp.Components.Pages;

public partial class CommentComponent(RequestUtil tku) : ComponentBase
{
    [Parameter] public int CommentId { get; set; }

    [Parameter] public string? IssuerInitial { get; set; } = string.Empty;

    [Parameter] public string? IssuerName { get; set; } = string.Empty;

    [Parameter] public string? Content { get; set; } = string.Empty;

    [Parameter] public DateTime CreatedAt { get; set; }

    [Parameter] public EventCallback<bool> CommentCallback { get; set; }
    private CommentForm CommentForm { get; set; } = new CommentForm();

    private bool IsEditing { get; set; } 

    private async Task DeleteComment()
    {
        var client = await tku.ConfigAuthorizationBeforeRequest();
        if (client is null) return;
        var req = new RestRequest($"/Comments/DeleteComment/", Method.Patch);
        req.AddQueryParameter("commentId", CommentId);
        var res = await client.ExecuteAsync(req);
        if (res.IsSuccessful)
        {
            Console.WriteLine("Comment deleted successfully.");
        }

        await CommentCallback.InvokeAsync(true);
    }

    private async Task EditComment()
    {
        try
        {
            var client = await tku.ConfigAuthorizationBeforeRequest();
            if (client is null) return;
            var req = new RestRequest("/Comments/EditComment/", Method.Patch);
            req.AddJsonBody(CommentForm);
            req.AddQueryParameter("commentId", CommentId);
            var res = await client.ExecuteAsync(req);
            if (res.IsSuccessful)
            {
                Console.WriteLine("Comment edited successfully.");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        finally
        {
            IsEditing = false;
            await CommentCallback.InvokeAsync(true);
        }
    }

    private async Task ToggleEdit()
    {
        IsEditing = !IsEditing;
        CommentForm.Content = Content;
        await CommentCallback.InvokeAsync(IsEditing);
    }
}