
using PomodoroTimer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PomodoroTimer
{
    public class AppConstants
    {
        public static string StartText { get; set; } = "Start";
        public static string StopText { get; set; } = "Stop";
        public static int PomodoroDuration { get; set; } = 1;
        public static int LittleBreakTime { get; set; } = 1;
        public static int LargeBreakDuration { get; set; } = 1;

        public static AplicationUser DEFAULT_USER
        {
            get
            {
                return new AplicationUser()
                {
                    Email = "",
                    Name = "",
                    Pass = "",
                };
            }
        }
        public static UserTask FREE_RUN_TASK
        {
            get
            {
                return new UserTask()
                {
                    Id = Guid.Parse("BEC42159-A542-4976-B14D-EB9E558E7540"),
                    TaskName = "Default",
                    TaskColor = "#27CC4C"
                };
            }
        }
        public static AppSettings DEFAULT_APP_SETTINGS
        {
            get
            {

                return new AppSettings()
                {
                    KeepStatistic = true,
                    PomodoroSettings = new PomodoroSettings()
                    {

                        AutoContinue = false,
                        SessionBreakDuration = 5,
                        PomodoroDuration = 25,
                        SessionPomodoroCount = 15,
                        PomodoroBreakDuration = 1,
                    },
                    UserSettings = new UserSettings()
                    {
                        UserName = "",
                        Email = "",
                        Password = "",
                    },
                    VibrationAlarm = true,
                    SoundAlarm = true,
                };

            }
        }
    }

}
