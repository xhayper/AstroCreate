using AstroCreate.Game;

namespace AstroCreate.Desktop
{
    public partial class AstroCreateDesktopGame : AstroCreateGame
    {
        protected override void LoadComplete()
        {
            Add(new DiscordRichPresence());

            base.LoadComplete();
        }
    }
}
