using System;
using PomodoroTimer.Services;
using Xamarin.Forms;

namespace PomodoroTimer
{
    public class AudioAlarmService
    {
        public void PlaySound(string sound)
        {
            DependencyService.Get<IAudioService>().PlayAudioFile(sound);
        }
    }
}