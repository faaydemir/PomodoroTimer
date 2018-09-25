using System;
using System.Collections.Generic;
using System.Text;

namespace PomodoroTimer.Models
{
    public class UserSettings
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}
