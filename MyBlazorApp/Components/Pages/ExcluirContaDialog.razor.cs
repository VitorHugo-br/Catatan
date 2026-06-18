using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MyBlazorApp.Components.Pages;

public partial class ExcluirContaDialog : ComponentBase
{
    [CascadingParameter]
    private IMudDialogInstance Dialog { get; set; }
    
    private void Submit() => Dialog.Close(DialogResult.Ok(true));
    
    private void Cancel() => Dialog.Cancel();
}