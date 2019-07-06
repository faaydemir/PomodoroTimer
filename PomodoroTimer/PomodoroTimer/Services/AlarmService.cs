using System;
using Xamarin.Forms;
using PomodoroTimer.Services;

namespace PomodoroTimer
{
    public class AlarmService
    {
        readonly AudioAlarmService AudioAlarmService;
        readonly VibrationAlarmService VibrationAlarmService;

        public bool SoundEnable { get; set; }
        public bool VibrationEnable { get; set; }
        public string AlarmSound { get; set; } = "beep.mp3";
        public int VibrationDuration { get; set; } = 1500;

        public AlarmService()
        {
            AudioAlarmService = new AudioAlarmService();
            VibrationAlarmService = new VibrationAlarmService();
        }

        public void RunPomodoroFinishedAlarm()
        {
            if (SoundEnable)
                AudioAlarmService.PlaySound(AlarmSound);

            if (VibrationEnable)
                VibrationAlarmService.Vibrate(VibrationDuration);
        }

        public void RunBreakFinishedAlarm()
        {
            if (SoundEnable)
                AudioAlarmService.PlaySound(AlarmSound);

            if (VibrationEnable)
                VibrationAlarmService.Vibrate(VibrationDuration / 2);
        }
    }
}