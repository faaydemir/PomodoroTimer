using System;
using Xamarin.Forms;
using PomodoroTimer.Views;
using Xamarin.Forms.Xaml;
using Plugin.LocalNotifications;
using PomodoroTimer.Services;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace PomodoroTimer
{
    public partial class App : Application
    {
        public App()
        {
#if DEBUG
            LiveReload.Init();
#endif

            InitializeComponent();
            LoadThema();
            MainPage = new MainPage();
        }
        protected override void OnStart()
        {
            AppMainService.Instance.DisableNotification();
            AppMainService.Instance.LogEvent(new Exception("App Opened"));
        }

        private void LoadThema()
        {
            AppMainService.Instance.LoadTheme();
        }
        protected override void OnSleep()
        {
            AppMainService.Instance.OnSleep();
            AppMainService.Instance.EnableNotification();
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {

            AppMainService.Instance.OnResume();
            AppMainService.Instance.DisableNotification();
            // Handle when your app resumes
        }
    }
}
