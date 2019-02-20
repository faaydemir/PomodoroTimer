using Android.App;
using Android.Widget;
using PomodoroTimer.Services.Interfaces;
using Your.Namespace;


// #laterusable
//from https://stackoverflow.com/questions/35279403/toast-equivalent-for-xamarin-forms
[assembly: Xamarin.Forms.Dependency(typeof(MessageAndroid))]
namespace Your.Namespace
{
    public class MessageAndroid : INotification
    {
        public void LongAlert(string message)
        {
            Toast.MakeText(Application.Context, message, ToastLength.Long).Show();
        }

        public void ShortAlert(string message)
        {
            Toast.MakeText(Application.Context, message, ToastLength.Short).Show();
        }

        public void Show(string message = "")
        {
            ShortAlert(message);
            //throw new System.NotImplementedException();
        }
    }
}