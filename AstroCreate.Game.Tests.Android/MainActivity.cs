using Android.App;
using Android.Content.PM;
using Android.OS;
using osu.Framework.Android;

namespace AstroCreate.Game.Tests.Android
{
    [Activity(ConfigurationChanges = DEFAULT_CONFIG_CHANGES, Exported = true, LaunchMode = DEFAULT_LAUNCH_MODE, MainLauncher = true)]
    public class MainActivity : AndroidGameActivity
    {
        protected override osu.Framework.Game CreateGame() => new AstroCreateTestBrowser();

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RequestedOrientation = ScreenOrientation.SensorLandscape;
        }
    }
}
