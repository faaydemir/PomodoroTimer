using StorageHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PomodoroTimer.Model
{
    public class UserTask : IEntity
    {

        public Guid Id { get; set; }
        public string TaskName { get; set; }
        public TimeSpan TimeSpend { get; set; }
        public int CanceledPomodoroCount { get; set; }
        public int FinishedPomodoroCount { get; set; }
        public List<UserTask> SubTasks { get; set; }
        public TaskGoal TaskGoal { get; set; }
    }
}
