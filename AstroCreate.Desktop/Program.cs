using osu.Framework.Platform;
using osu.Framework;
using AstroCreate.Game;

namespace AstroCreate.Desktop
{
    public static class Program
    {
        public static void Main()
        {
            using (GameHost host = Host.GetSuitableDesktopHost(@"AstroCreate"))
            using (osu.Framework.Game game = new AstroCreateGame())
                host.Run(game);
        }
    }
}
