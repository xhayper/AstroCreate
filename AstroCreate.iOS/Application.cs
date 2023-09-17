using osu.Framework.iOS;
using AstroCreate.Game;

namespace AstroCreate.iOS
{
    public static class Application
    {
        public static void Main(string[] args) => GameApplication.Main(new AstroCreateGame());
    }
}
