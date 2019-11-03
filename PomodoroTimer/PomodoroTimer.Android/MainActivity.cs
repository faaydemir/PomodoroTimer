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
using CarouselView.FormsPlugin.Android;
using System.Threading.Tasks;

namespace PomodoroTimer.Droid
{
    [Activity(Label = "PomodoroTimer", Theme = "@style/MainTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            LocalNotificationsImplementation.NotificationIconId = Resource.Drawable.clock_white;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
            TaskScheduler.UnobservedTaskException += TaskSchedulerOnUnobservedTaskException;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App());
            CarouselViewRenderer.Init();

        }

        private void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e != null)
            {
                Exception exception = e.ExceptionObject as Exception;
                if (exception != null)
                {
                    exception = new Exception("CurrentDomainOnUnhandledException");
                }
                AppMainService.Instance.LogEvent(exception);
            }
        }

        private void TaskSchedulerOnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            AppMainService.Instance.LogEvent(e.Exception);
        }

        protected override void OnDestroy()
        {
            //TO DO  ???????????????????????????????????????????????????
            AppMainService.Instance.OnDestroy();
            base.OnDestroy();
        }
        protected override void OnResume()
        {
            base.OnResume();
        }
        protected override void OnStart()
        {
            base.OnStart();
        }
    }
}

