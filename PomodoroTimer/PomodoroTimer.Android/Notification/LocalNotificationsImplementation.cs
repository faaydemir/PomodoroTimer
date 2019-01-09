using Android.App;
using Android.Content;
using Android.Support.V4.App;
using Plugin.LocalNotifications.Abstractions;
using System;
using System.IO;
using System.Xml.Serialization;
using Android.OS;
using Android;
using System.Collections.Generic;
using PomodoroTimer.Droid;

namespace Plugin.LocalNotifications
{

    /// <summary>
    /// Local Notifications implementation for Android
    /// from https://github.com/edsnider/localnotificationsplugin/tree/master/samples/LocalNotificationsSample
    /// changes for disable sound for each notification
    /// </summary>
    public class LocalNotificationsImplementation : ILocalNotifications
    {
        string _packageName => Application.Context.PackageName;
        NotificationManager _manager => (NotificationManager)Application.Context.GetSystemService(Context.NotificationService);
        Dictionary<int, Notification.Builder> BuilderDic { get; set; } = new Dictionary<int, Notification.Builder>();

        /// <summary>
        /// Get or Set Resource Icon to display
        /// </summary>
        public static int NotificationIconId { get; set; }

        /// <summary>
        /// Show a local notification
        /// </summary>
        /// <param name="title">Title of the notification</param>
        /// <param name="body">Body or description of the notification</param>
        /// <param name="id">Id of the notification</param>
        public void Show(string title, string body, int id = 0)
        {
            if (!BuilderDic.ContainsKey(id))
            {
                BuilderDic[id] = new Notification.Builder(Application.Context);
                BuilderDic[id].SetAutoCancel(true);
                BuilderDic[id].SetOnlyAlertOnce(true);
            }
            var builder = BuilderDic[id];
            builder.SetContentTitle(title);
            builder.SetContentText(body);

            if (NotificationIconId != 0)
            {
                builder.SetSmallIcon(NotificationIconId);
            }
            else
            {
                builder.SetSmallIcon(Android.Resource.Drawable.AlertDarkFrame);
            }

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var channelId = $"{_packageName}.general";
                var channel = new NotificationChannel(channelId, "General", NotificationImportance.Default);

                _manager.CreateNotificationChannel(channel);

                builder.SetChannelId(channelId);
            }

            var resultIntent = GetLauncherActivity();
            var resultPendingIntent = PendingIntent.GetActivity(Application.Context, 0, resultIntent, PendingIntentFlags.UpdateCurrent);
            builder.SetContentIntent(resultPendingIntent);

            var notificaton = builder.Build();

            _manager.Notify(id, notificaton);
        }

        public static Intent GetLauncherActivity()
        {
            var packageName = Application.Context.PackageName;
            var intent = Application.Context.PackageManager.GetLaunchIntentForPackage(packageName);
            intent.SetFlags(ActivityFlags.NewTask);
            return intent;
        }

        /// <summary>
        /// Show a local notification at a specified time
        /// </summary>
        /// <param name="title">Title of the notification</param>
        /// <param name="body">Body or description of the notification</param>
        /// <param name="id">Id of the notification</param>
        /// <param name="notifyTime">Time to show notification</param>
        public void Show(string title, string body, int id, DateTime notifyTime)
        {
            var intent = CreateIntent(id);

            var localNotification = new LocalNotification();
            localNotification.Title = title;
            localNotification.Body = body;
            localNotification.Id = id;
            localNotification.NotifyTime = notifyTime;
            if (NotificationIconId != 0)
            {
                localNotification.IconId = NotificationIconId;
            }
            else
            {
                localNotification.IconId = Android.Resource.Drawable.AlertDarkFrame;
            }

            var serializedNotification = SerializeNotification(localNotification);
            intent.PutExtra(ScheduledAlarmHandler.TimerKey, serializedNotification);

            var pendingIntent = PendingIntent.GetBroadcast(Application.Context, 0, intent, PendingIntentFlags.CancelCurrent);
            var triggerTime = NotifyTimeInMilliseconds(localNotification.NotifyTime);
            var alarmManager = GetAlarmManager();

            alarmManager.Set(AlarmType.RtcWakeup, triggerTime, pendingIntent);
        }

        /// <summary>
        /// Cancel a local notification
        /// </summary>
        /// <param name="id">Id of the notification to cancel</param>
        public void Cancel(int id)
        {
            var intent = CreateIntent(id);
            var pendingIntent = PendingIntent.GetBroadcast(Application.Context, 0, intent, PendingIntentFlags.CancelCurrent);

            var alarmManager = GetAlarmManager();
            alarmManager.Cancel(pendingIntent);

            var notificationManager = NotificationManagerCompat.From(Application.Context);
            notificationManager.Cancel(id);
        }

        private Intent CreateIntent(int id)
        {
            return new Intent(Application.Context, typeof(ScheduledAlarmHandler))
                .SetAction("LocalNotifierIntent" + id);
        }


        private AlarmManager GetAlarmManager()
        {
            var alarmManager = Application.Context.GetSystemService(Context.AlarmService) as AlarmManager;
            return alarmManager;
        }

        private string SerializeNotification(LocalNotification notification)
        {
            var xmlSerializer = new XmlSerializer(notification.GetType());
            using (var stringWriter = new StringWriter())
            {
                xmlSerializer.Serialize(stringWriter, notification);
                return stringWriter.ToString();
            }
        }

        private long NotifyTimeInMilliseconds(DateTime notifyTime)
        {
            var utcTime = TimeZoneInfo.ConvertTimeToUtc(notifyTime);
            var epochDifference = (new DateTime(1970, 1, 1) - DateTime.MinValue).TotalSeconds;

            var utcAlarmTimeInMillis = utcTime.AddSeconds(-epochDifference).Ticks / 10000;
            return utcAlarmTimeInMillis;
        }
    }
}