using System;
using Xamarin.Forms;
using PomodoroTimer.Views;
using Xamarin.Forms.Xaml;
using Plugin.LocalNotifications;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace PomodoroTimer
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            AppMainService.Instance.DisableNotification();
            // Handle when your app starts
        }

        protected override void OnSleep()
        {

            AppMainService.Instance.EnableNotification();
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            AppMainService.Instance.DisableNotification();
            // Handle when your app resumes
        }
    }
}
