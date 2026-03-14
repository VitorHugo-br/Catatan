using Microsoft.AspNetCore.Components;
using MyBlazorApp.Utils;

namespace MyBlazorApp.Components.Layout
{
    public partial class MainLayout(RequestUtil requestUtil, NavigationManager nav) : LayoutComponentBase
    {
        protected override async Task OnInitializedAsync()
        {
            var token = await requestUtil.GetTokenFromSessionStorage();
            if (token.Value is null) nav.NavigateTo("/");
        }
    }
}