using StorageHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PomodoroTimer.Model
{
    public class Pomodoro :IEntity
    {
        public Guid Id { get; set; }
        public TimeSpan Duration { get; set; }
        public PomodoroState State { get; set; }
        public TimeSpan ElapsedTime { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime FinishTime { get; set; }
    }
}
