using Microsoft.AspNetCore.SignalR.Client;
using MyBlazorApp.Dto;
using MyBlazorApp.Models.DTO;
using System.Text.Json;

namespace MyBlazorApp.Services;

public class NotificationService : IAsyncDisposable
{
    private HubConnection? _hubConnection;
    public event Action<NotificacaoNovaTarefa>? OnNovoChamado;
    public bool IsConnected => _hubConnection?.State == HubConnectionState.Connected;

    public async Task IniciarConexaoAsync(string apiBaseUrl, string token)
    {
        _hubConnection = new HubConnectionBuilder()
            .WithUrl($"{apiBaseUrl}/hubs/notification", options =>
            {
                var serializedToken = JsonSerializer.Deserialize<LoginResponse>(token);
                options.AccessTokenProvider = () => Task.FromResult<string?>(serializedToken?.token);
            })
            .WithAutomaticReconnect()
            .Build();

        _hubConnection.On<NotificacaoNovaTarefa>("NovaTarefaNotificacao", (dados) =>
        {
            OnNovoChamado?.Invoke(dados);
        });

        await _hubConnection.StartAsync();
    }

    public async ValueTask DisposeAsync()
    {
        if (_hubConnection is not null)
        {
            await _hubConnection.DisposeAsync();
            _hubConnection = null;
        }

    }
}
