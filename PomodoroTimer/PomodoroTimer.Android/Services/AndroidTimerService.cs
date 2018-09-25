using Android.App;
using Android.Content;
using Android.OS;
using PomodoroTimer.Droid.Services;
using PomodoroTimer.Enums;
using PomodoroTimer.Messaging;
using PomodoroTimer.Services;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PomodoroTimer.Droid
{
    [Service]
    public class AndroidTimerService : Service
    {
        CancellationTokenSource _cts;
        AndroidTimer AndroidTimer;
        public AndroidTimerService()
        {

        }
        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            _cts = new CancellationTokenSource();
            Task.Run(() =>
            {
                MessagingCenter.Subscribe<TimerControlMessage>(this, AppMessages.TimerControlMessage, message =>
                {
                    OnMessageReceived(message);
                });
                try
                {
                    AndroidTimer = new AndroidTimer();
                    AndroidTimer.Run(_cts.Token).Wait();
                }
                catch (Android.Accounts.OperationCanceledException)
                {
                }
                catch (Exception ex)
                {
                }
                finally
                {
                }

            }, _cts.Token);

            return StartCommandResult.Sticky;
        }

        private void OnMessageReceived(TimerControlMessage message)
        {
            if (message == null)
                return;
            if (message.TimerAction == TimerAction.Pause)
            {
                AndroidTimer.Pause();
            }
            else if (message.TimerAction == TimerAction.Continue)
            {
                AndroidTimer.Start();
            }
            else if (message.TimerAction == TimerAction.Run)
            {
                if (AndroidTimer == null)
                    AndroidTimer = new AndroidTimer();
                AndroidTimer.Start(message.RunningTime);
            }
            else if (message.TimerAction == TimerAction.Stop)
            {
                //Kill Timer Here ?
                AndroidTimer.Stop();
            }
        }

        public override void OnDestroy()
        {
            AndroidTimer?.Stop();
            if (_cts != null)
            {
                _cts.Token.ThrowIfCancellationRequested();

                _cts.Cancel();
            }
            base.OnDestroy();
        }
    }
}