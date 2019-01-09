

using System;
using Android.App;
using Android.Content;
using Plugin.LocalNotifications;
using PomodoroTimer.Services;
using Xamarin.Forms;
using Application = Android.App.Application;

[assembly: Dependency(typeof(AndroidAlarmService))]
namespace PomodoroTimer.Services
{
    public class AndroidAlarmService : IAlarmService
    {
        public event AlarmEventHandler AlarmEvent;

        public AndroidAlarmService()
        {
            MessagingCenter.Subscribe<ScheduledAlarmHandler>(this, "OnAlarm", (args) =>
            {
                AlarmEvent.Invoke(this, null);
            });
        }

        public void CancelAlarm()
        {
            var intent = CreateIntent();
            var pendingIntent = PendingIntent.GetBroadcast(Application.Context, 0, intent, PendingIntentFlags.CancelCurrent);
        }
        public static Intent GetLauncherActivity()
        {
            var packageName = Application.Context.PackageName;
            var intent = Application.Context.PackageManager.GetLaunchIntentForPackage(packageName);
            intent.SetFlags(ActivityFlags.NewTask);
            return intent;
        }


        public void SetAlarm(TimeSpan duration)
        {
            DateTime notifyTime = DateTime.Now.Add(duration);
            var intent = CreateIntent();
            var pendingIntent = PendingIntent.GetBroadcast(Application.Context, 0, intent, PendingIntentFlags.UpdateCurrent);
            var triggerTime = ToMiliseconds(notifyTime);
            var alarmManager = GetAlarmManager();
            alarmManager.SetExact(AlarmType.RtcWakeup, triggerTime, pendingIntent);


        }


        private Intent CreateIntent()
        {
            return new Intent(Application.Context, typeof(ScheduledAlarmHandler))
                .SetAction("CountDownIntent");
        }

        private AlarmManager GetAlarmManager()
        {
            var alarmManager = Application.Context.GetSystemService(Context.AlarmService) as AlarmManager;
            return alarmManager;
        }

        private long ToMiliseconds(DateTime notifyTime)
        {
            var utcTime = TimeZoneInfo.ConvertTimeToUtc(notifyTime);
            var epochDifference = (new DateTime(1970, 1, 1) - DateTime.MinValue).TotalSeconds;

            var utcAlarmTimeInMillis = utcTime.AddSeconds(-epochDifference).Ticks / 10000;
            return utcAlarmTimeInMillis;
        }
    }
}