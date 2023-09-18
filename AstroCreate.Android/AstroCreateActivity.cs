using Android.App;
using Android.Content.PM;
using Android.OS;
using AstroCreate.Game;
using osu.Framework.Android;

namespace AstroCreate.Android
{
    [Activity(ConfigurationChanges = DEFAULT_CONFIG_CHANGES, Exported = true, LaunchMode = DEFAULT_LAUNCH_MODE, MainLauncher = true)]
    public class AstroCreateActivity : AndroidGameActivity
    {
        protected override osu.Framework.Game CreateGame() => new AstroCreateGame();

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RequestedOrientation = ScreenOrientation.SensorLandscape;
        }
    }
}
