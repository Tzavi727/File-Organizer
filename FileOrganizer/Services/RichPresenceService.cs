using DiscordRPC;
using System.Threading;
using System.Threading.Tasks;

namespace FileOrganizer.Services
{
    internal class RichPresenceService
    {
        public static DiscordRpcClient? Client { get; private set; }
        private static CancellationTokenSource? _timerCts;

        public static void InitializeRpc()
        {
            if (PreferencesService.Preferences.IsDiscordEnabled)
            {
                Client = new DiscordRpcClient("1515420956527693944");
                Client.Initialize();
            }
        }

        public static void DisposeClient()
        {
            Client?.Dispose();
        }

        public static void SetIdlePresence()
        {
            Client?.SetPresence(new RichPresence()
            {
                Details = "Idle",
                State = "Waiting for Action...",
                Assets = new Assets()
                {
                    LargeImageKey = "wolf",
                    LargeImageText = "File Organizer by Tzavi"
                }
            });
        }

        public static async void SetActionPresence(string details, string state)
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

        public static void UpdateStatus(string details, string state)
        {
            Client?.SetPresence(new RichPresence()
            {
                Details = details,
                State = state,
                Assets = new Assets()
                {
                    LargeImageKey = "wolf.png",
                    LargeImageText = "File Organizer by Tzavi"
                }
            });
        }
    }
}
