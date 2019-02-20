using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PomodoroTimer.Models;

namespace PomodoroTimer.Services
{
    public interface IStorageService
    {
        AplicationUser GetUser();
        AppSettings GetAppSettings();
        PomodoroSession GetSession();
        List<UserTask> GetAllUserTask(AplicationUser user);
        List<TaskStatistic> GetStatisticData(DateTime startTime, DateTime finishTime);
        PomdoroStatus GetLastState();


        Task<bool> UpdateUserTask(UserTask task);
        Task<bool> AddNewUserTask(UserTask userTask);
        Task<bool> RemoveUserTask(UserTask userTask);
        Task<bool> SetAppSettings(AppSettings settings);
        Task<bool> UpdateSessionInfo(PomodoroSession currentSession);
        Task<bool> ClearStatistics(DateTime startTime, DateTime finishTime);
        Task<bool> SaveAppState(PomdoroStatus appState);
    }
}
