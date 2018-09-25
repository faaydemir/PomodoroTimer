using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xamarin.Forms;
using PomodoroTimer.Services;
using Android.Content;
using PomodoroTimer.Messaging;
using Plugin.LocalNotifications;
using HockeyApp.Android;

namespace PomodoroTimer.Droid
{
    [Activity(Label = "PomodoroTimer", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private string AppId = "203cc868e8024963a8e2b155240f22a4";
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App());


            var intent = new Intent(this, typeof(AndroidTimerService));
            StartService(intent);

            LocalNotificationsImplementation.NotificationIconId = Resource.Drawable.clock;
        }
        protected override void OnResume()
        {
            base.OnResume();
            CrashManager.Register(this, AppId);
        }
    }
}

