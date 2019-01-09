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

        bool UpdateUserTask(UserTask task);
        bool AddNewUserTask(UserTask userTask);
        bool RemoveUserTask(UserTask userTask);
        bool SetAppSettings(AppSettings settings);
        bool UpdateSessionInfo(PomodoroSession currentSession);
        bool ClearStatistics(DateTime startTime, DateTime finishTime);

        List<UserTask> GetAllUserTask(AplicationUser user);
        List<TaskStatistic> GetStatisticData(DateTime startTime, DateTime finishTime);
        PomdoroStatus ReadLastState();
        void SaveAppState(PomdoroStatus appState);
    }
}
