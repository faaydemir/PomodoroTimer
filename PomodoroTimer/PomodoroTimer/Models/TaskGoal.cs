using PomodoroTimer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PomodoroTimer.Models
{
    public class TaskGoal
    {
        public int PomodoroCount { get; set; }
        public GoalFrequency GoalInterval { get; set; }
    }
}
