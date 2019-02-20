using PomodoroTimer.Models;
using PomodoroTimer.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using XamarinHelpers.MVVM;

namespace PomodoroTimer.ViewModels
{
    public class MasterViewModel : PageViewModel
    {
        public ObservableCollection<MasterViewMenuItem> MenuItems { get; set; }

        public MasterViewModel()
        {
            MenuItems = new ObservableCollection<MasterViewMenuItem>(new[]
            {
                    new MasterViewMenuItem { Id = 0, Icon="home_white.png", Title = "Home", TargetType = typeof(HomePage) },
                    new MasterViewMenuItem { Id = 1, Icon="task_white.png",Title = "Task", TargetType = typeof(TasksPage) },
                    new MasterViewMenuItem { Id = 2, Icon="bar_white.png",Title = "Statistic", TargetType = typeof(StatisticPage) },
                    new MasterViewMenuItem { Id = 3, Icon="settings_white.png",Title = "Settings ", TargetType = typeof(SettingsPage) },
                    new MasterViewMenuItem { Id = 4, Icon="about_white.png",Title = "About", TargetType = typeof(AboutPage) },
                });
        }
    }
}
