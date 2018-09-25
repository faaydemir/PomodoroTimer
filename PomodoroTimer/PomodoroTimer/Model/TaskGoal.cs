using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PomodoroTimer.Model
{
    public class TaskGoal
    {
        public int PomodoroCount { get; set; }
        public GoalFrequency GoalTime { get; set; }
        public int DailyPomodoroCount { get; set; }
    }
}
