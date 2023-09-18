using osu.Framework.iOS;

namespace AstroCreate.Game.Tests.iOS
{
    public static class Application
    {
        public static void Main(string[] args)
        {
            GameApplication.Main(new AstroCreateTestBrowser());
        }
    }
}
