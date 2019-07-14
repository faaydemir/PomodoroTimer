using PomodoroTimer.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PomodoroTimer.Services.Interfaces
{
    /// <summary>
    /// To Store and Sync App data 
    /// </summary>
    public interface IOnlineDataStore
    {
        Task<bool> AddLog(AppLog appLog);
        Task<bool> AddTaskStatisticsAsync(TaskStatistic taskStatistic);
    }
}
