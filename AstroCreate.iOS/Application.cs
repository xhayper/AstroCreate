using AstroCreate.Game;
using osu.Framework.iOS;

namespace AstroCreate.iOS
{
    public static class Application
    {
        public static void Main(string[] args) => GameApplication.Main(new AstroCreateGame());
    }
}
