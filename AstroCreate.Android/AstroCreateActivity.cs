using Android.App;
using AstroCreate.Game;
using osu.Framework.Android;

namespace AstroCreate.Android
{
    [Activity(ConfigurationChanges = DEFAULT_CONFIG_CHANGES, Exported = true, LaunchMode = DEFAULT_LAUNCH_MODE, MainLauncher = true)]
    public class AstroCreateActivity : AndroidGameActivity
    {
        protected override osu.Framework.Game CreateGame() => new AstroCreateGame();
    }
}
