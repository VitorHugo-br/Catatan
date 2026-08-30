using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using MyBlazorApp.Components.Modals;
using MyBlazorApp.Dto;
using MyBlazorApp.interfaces;
using MyBlazorApp.Services;

namespace MyBlazorApp.Components.Layout;

public partial class MainLayout(NavigationManager nav, IDialogService dialog, ILocalStorageService localStorage, TokenProvider tokenProvider, IChamadoService chamadoService, NotificationService notificationService) : LayoutComponentBase
{

    private bool _open;

    private bool _modoEscuro;

    private int? chamadonumero;

    public NotificacaoNovaTarefa? _ultimaNotificacao;

    private List<NotificacaoNovaTarefa> Notificacoes = [];

    private string NotificacaoIcone => Notificacoes is { Count: > 0} ? Icons.Material.Filled.Notifications : Icons.Material.Outlined.Notifications;

    protected override async Task OnInitializedAsync()
    {
        var EhTokenValido = await chamadoService.ValidarTokenAsync();
        if (!EhTokenValido)
        {
            nav.NavigateTo("/", forceLoad: true);
            return;
        }

        notificationService.OnNovoChamado += HandleNovoChamado;

        if (!string.IsNullOrEmpty(tokenProvider.Jwt))
        {
            await notificationService.IniciarConexaoAsync("https://localhost:7049", tokenProvider.Jwt);
        }
    }

    private void HandleNovoChamado(NotificacaoNovaTarefa dados)
    {
        _ultimaNotificacao = dados;
        Notificacoes.Add(dados);
        InvokeAsync(StateHasChanged);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            string? darkMode = await localStorage.GetItemAsync("darkMode");
            if (bool.TryParse(darkMode, out bool result))
            {
                _modoEscuro = result;
            }
            else
            {
                _modoEscuro = false;
            }
            StateHasChanged();
        }
    }

    private Task<IDialogReference> AdicionarTarefa()
    {
        var options = new DialogOptions
        {
            CloseButton = true,
            Position = DialogPosition.Center,
            FullWidth = true,

        };
        return dialog.ShowAsync<AdicionarChamadoDialog>("Adicionar tarefa.", options);
    }

    private async void Sair() => nav.NavigateTo("/api/logout", forceLoad: true);

    private void BuscarChamado()
    {
        if (chamadonumero != null && chamadonumero > 0)
        {
            nav.NavigateTo($"/Chamados/{chamadonumero}");
            return;
        }

    }

    public Task<IDialogReference> AdicionarGrupo()
    {
        var options = new DialogOptions
        {
            CloseButton = true,
            Position = DialogPosition.Center,
            FullWidth = true,

        };
        return dialog.ShowAsync<AdicionarGrupoDialog>("Adicionar grupo.", options);
    }

    private async Task SaveTheme() => await localStorage.SetItemAsync("darkMode", _modoEscuro.ToString());

    private void NavegarPara(string pagina) => nav.NavigateTo($"{pagina}");

    // Controle do Chat FAB e Balão de Chat
    private bool _chatAberto;
    private string _novaMensagemChat = string.Empty;
    private List<ChatMessage> _mensagensChat =
    [
        new("TaskMan Bot", "Olá! Bem-vindo ao suporte TaskMan. Como posso ajudar você hoje?", DateTime.Now, false)
    ];

    public record ChatMessage(string Autor, string Texto, DateTime Horario, bool EnviadoPeloUsuario);

    private void AlternarChat() => _chatAberto = !_chatAberto;

    private void EnviarMensagemChat()
    {
        if (string.IsNullOrWhiteSpace(_novaMensagemChat)) return;

        _mensagensChat.Add(new("Você", _novaMensagemChat.Trim(), DateTime.Now, true));
        _novaMensagemChat = string.Empty;
    }

    private void HandleChatKeyDown(Microsoft.AspNetCore.Components.Web.KeyboardEventArgs e)
    {
        if (e.Key == "Enter" && !e.ShiftKey)
        {
            EnviarMensagemChat();
        }
    }
}