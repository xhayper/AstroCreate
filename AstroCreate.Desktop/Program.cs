using osu.Framework.Platform;
using osu.Framework;

namespace AstroCreate.Desktop
{
    public static class Program
    {
        public static void Main()
        {
            using (GameHost host = Host.GetSuitableDesktopHost(@"AstroCreate"))
            using (osu.Framework.Game game = new AstroCreateDesktopGame())
                host.Run(game);
        }
    }
}
