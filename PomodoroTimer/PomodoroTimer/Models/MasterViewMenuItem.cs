using System;

namespace PomodoroTimer.Models
{
    public class MasterViewMenuItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Icon { get; set; }
        public Type TargetType { get; set; }
    }
}