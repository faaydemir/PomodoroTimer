using System;
using System.Collections.Generic;
using System.Text;

namespace PomodoroTimer.Services.Interfaces
{
    public interface INotification
    {
        
        void Show(string message ="" );
    }
}
