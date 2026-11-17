using System;
using System.Threading;
using System.Threading.Tasks;
using DiscordRPC;
using FileOrganizer.Interfaces;

namespace FileOrganizer.Services;

internal class RichPresenceService : IRichPresenceService, IDisposable
{
    private readonly IPreferencesService _preferencesService;
    public DiscordRpcClient? Client { get; private set; }
    private CancellationTokenSource? _timerCts;

    public RichPresenceService(IPreferencesService preferencesService)
    {
        _preferencesService = preferencesService;
    }

    public void InitializeRpc()
    {
        if (_preferencesService.IsDiscordEnabled)
        {
            Client = new DiscordRpcClient("1515420956527693944");
            Client.Initialize();
        }
    }

    public void DisposeClient()
    {
        _timerCts?.Cancel();
        _timerCts?.Dispose();
        Client?.Dispose();
    }

    public void SetIdlePresence()
    {
        Client?.SetPresence(new RichPresence
        {
            Details = "Idle",
            State = "Waiting for Action...",
            Assets = new Assets
            {
                LargeImageKey = "wolf",
                LargeImageText = "File Organizer by Tzavi"
            }
        });
    }

    public async Task SetActionPresence(string details, string state)
    {
        _timerCts?.Cancel();
        _timerCts = new CancellationTokenSource();
        var token = _timerCts.Token;

        try
        {
            UpdateStatus(details, state);
            await Task.Delay(120000, token);
            SetIdlePresence();
        }
        catch (TaskCanceledException)
        {
        }
    }

    public void UpdateStatus(string details, string state)
    {
        Client?.SetPresence(new RichPresence
        {
            Details = details,
            State = state,
            Assets = new Assets
            {
                LargeImageKey = "wolf.png",
                LargeImageText = "File Organizer by Tzavi"
            }
        });
    }

    public void Dispose()
    {
        DisposeClient();
    }
}