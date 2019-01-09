using System.IO;
using System.Xml.Serialization;
using Android.App;
using Android.Content;
using Android.Support.V4.App;
using Xamarin.Forms;

namespace Plugin.LocalNotifications
{
    /// <summary>
    /// Broadcast receiver
    /// </summary>
    [BroadcastReceiver(Enabled = true, Label = "Local Notifications Plugin Broadcast Receiver")]
    public class ScheduledAlarmHandler : BroadcastReceiver
    {
        /// <summary>
        /// 
        /// </summary>
        public const string TimerKey = "TimerKey";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="intent"></param>
        public override void OnReceive(Context context, Intent intent)
        {
            MessagingCenter.Send<ScheduledAlarmHandler>(this, "OnAlarm");
                
            ///    <ScheduledAlarmHandler>(this, "OnAlarm", (args) =>
            ///{
            ///    this.AlarmEvent.Invoke(this, null);
            ///});


            //var extra = intent.GetStringExtra(LocalNotificationKey);
            //var notification = DeserializeNotification(extra);

            //CrossLocalNotifications.Current.Show("finished", "fkajsldkfjslkdfj", 1);
        }
    }
}