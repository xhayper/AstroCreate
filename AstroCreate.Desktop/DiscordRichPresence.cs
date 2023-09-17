using DiscordRPC;
using DiscordRPC.Message;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Logging;

namespace AstroCreate.Desktop
{
    internal partial class DiscordRichPresence : Component
    {
        private const string client_id = "1152983484990767125";

        private DiscordRpcClient client = null!;

        [BackgroundDependencyLoader]
        private void load()
        {
            client = new DiscordRpcClient(client_id)
            {
                SkipIdenticalPresence = false
            };

            client.OnError += (_, e) => Logger.Log($"An error occurred with Discord RPC Client: {e.Code} {e.Message}", LoggingTarget.Network);
            client.OnReady += onReady;

            client.Initialize();
        }

        private void onReady(object _, ReadyMessage __)
        {
            Logger.Log("Discord RPC Client ready.", LoggingTarget.Network, LogLevel.Debug);

            var presence = new RichPresence
            {
                Assets = new Assets
                {
                    LargeImageKey = "https://maimaidx-eng.com/maimai-mobile/img/Music/5461cc48817a3c7a.png"
                },
                Details = "Working on VeRForTe αRtE:VEiN...",
                State = "Line 69; Column 420"
            };

            client.SetPresence(presence);
        }

        protected override void Dispose(bool isDisposing)
        {
            client.Dispose();
            base.Dispose(isDisposing);
        }
    }
}
