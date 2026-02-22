using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace MyBlazorApp.Components.Layout
{
    public partial class MainLayout(IJSRuntime JS) : LayoutComponentBase
    {

        protected override void OnInitialized()
        {
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender) {
                var token = await JS.InvokeAsync<string>("localStorage.getItem", "authToken");
            }

        }
            
    }
}