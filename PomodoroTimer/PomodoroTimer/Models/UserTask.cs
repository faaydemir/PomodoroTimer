
using System;
using System.Collections.Generic;
using System.Text;

namespace PomodoroTimer.Models
{
    public class UserTask
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string TaskName { get; set; }
        public string TaskColor { get; set; }
        public string TaskIcon { get; set; }
        public TaskGroup TaskGroup {get;set;}
        public PomodoroSettings PomodoroSettings { get; set; }
        public TaskGoal TaskGoal { get; set; }
        public List<UserTask> SubTasks { get; internal set; }
        public FinishedTaskStatistic TaskStatistic { get; internal set; }
    }
}
