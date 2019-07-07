using PomodoroTimer.Models;
using PomodoroTimer.Services;
using PomodoroTimer.Services.Interfaces;
using PomodoroTimer.Validations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinHelpers.MVVM;
using XamarinHelpers.Utils;

namespace PomodoroTimer.ViewModels
{
    public class SettingsViewModel : PageViewModel
    {
        private IAppService AppService;
        private int _pomodoroDuration = 5;
        private int _smallBreakDuration = 5;
        private int _sessionPomodoroCount = 5;
        private int _largeBreakDuration = 5;
        private bool _autoContinue = false;
        private bool _keepStatistic = true;
        private string _validataionMessage;
        private string _userName;
        private string _email;
        private bool _soundAlarm = true;
        private bool _vibrationAlarm = true;
        private bool _nightMode = false;

        public string Email
        {
            get { return _email; }
            set { SetProperty(ref _email, value); }
        }


        public string UserName
        {
            get { return _userName; }
            set { SetProperty(ref _userName, value); }
        }
        public bool SoundAlarm
        {
            get { return _soundAlarm; }
            set { SetProperty(ref _soundAlarm, value); }
        }
        public bool VibrationAlarm
        {
            get { return _vibrationAlarm; }
            set { SetProperty(ref _vibrationAlarm, value); }
        }
        public bool NightMode
        {
            get { return _nightMode; }
            set { SetProperty(ref _nightMode, value); }
        }
        public string ValidationMessage
        {
            get { return _validataionMessage; }
            set { SetProperty(ref _validataionMessage, value); }
        }
        public bool KeepStatistic
        {
            get { return _keepStatistic; }
            set { SetProperty(ref _keepStatistic, value); }
        }
        public bool AutoContinue
        {
            get { return _autoContinue; }
            set { SetProperty(ref _autoContinue, value); }
        }

        public int LargeBreakDuration
        {
            get { return _largeBreakDuration; }
            set { SetProperty(ref _largeBreakDuration, value); }
        }

        public int SessionPomodoroCount
        {
            get { return _sessionPomodoroCount; }
            set { SetProperty(ref _sessionPomodoroCount, value); }
        }

        public int SmallBreakDuration
        {
            get { return _smallBreakDuration; }
            set { SetProperty(ref _smallBreakDuration, value); }
        }


        public int PomodoroDuration
        {
            get { return _pomodoroDuration; }
            set { SetProperty(ref _pomodoroDuration, value); }
        }


        public ICommand Save { get; set; }
        public ICommand LoadDefault { get; set; }
        public ICommand ClearStatistic { get; set; }

        public List<string> Frequencies { get; set; }
        public List<string> Colors { get; set; }


        public SettingsViewModel(IAppService appService)
        {
            AppService = appService;
            LoadSettings(appService.AppSettings);


            ClearStatistic = new Command(
                 execute: async () =>
                 {
                     var displayAlert = new DialogProvider(Page);
                     var changeTask = await displayAlert.DisplayAlert("Clear Task Statistics", "All statistics about finished task will be deleted. Did you want to continue.", "ok", "cancel");
                     if (!changeTask)
                         return;
                     AppService.ClearStatistics();
                 }
             );

            Save = new Command(
                 execute: async () =>
                 {
                     var settings = this.CreatePomodoroSettings();
                     if (settings != null)
                     {
                         IsBusy = true;
                         var isSaved = await AppService.SaveSettingsAsync(settings);
                         if (isSaved)
                         {
                             var notificator = DependencyService.Get<INotification>();
                             notificator.Show("Saved.");
                         }
                         IsBusy = false;
                     }
                 }
             );

            LoadDefault = new Command(
                 execute: async () =>
                 {
                     var settings = AppConstants.DEFAULT_APP_SETTINGS;
                     if (settings != null)
                     {
                         this.IsBusy = true;
                         await AppService.SaveSettingsAsync(settings);
                         this.LoadSettings(settings);
                         this.IsBusy = false;
                     }
                 }
             );
        }

        private void LoadSettings(AppSettings appSettings)
        {
            SessionPomodoroCount = appSettings.PomodoroSettings.SessionPomodoroCount;
            LargeBreakDuration = (int)appSettings.PomodoroSettings.SessionBreakDuration.TotalMinutes;
            SmallBreakDuration = (int)appSettings.PomodoroSettings.PomodoroBreakDuration.TotalMinutes;
            PomodoroDuration = (int)appSettings.PomodoroSettings.PomodoroDuration.TotalMinutes;

            UserName = appSettings.UserSettings.UserName;
            Email = appSettings.UserSettings.Email;

            AutoContinue = appSettings.PomodoroSettings.AutoContinue;
            KeepStatistic = appSettings.KeepStatistic;

            SoundAlarm = appSettings.SoundAlarm;
            VibrationAlarm = appSettings.VibrationAlarm;
            NightMode = appSettings.ApplicationThema == Enums.ApplicationThema.NightThema;
        }

        public AppSettings CreatePomodoroSettings()
        {
            ValidationMessage = "";
            AppSettings appSettings = new AppSettings();
            PomodoroSettings pomodoroSettings = new PomodoroSettings();
            UserSettings userSettings = new UserSettings();

            appSettings.KeepStatistic = KeepStatistic;
            appSettings.SoundAlarm = SoundAlarm;
            appSettings.VibrationAlarm = VibrationAlarm;
            appSettings.VibrationAlarm = VibrationAlarm;
            appSettings.ApplicationThema = NightMode ? Enums.ApplicationThema.NightThema : Enums.ApplicationThema.DayThema;
            userSettings.UserName = UserName;



            if (Email == null || Email == "")
            {

            }
            else
            {
                var emailValidationResult = EmailValidationController.Check(Email);
                if (!emailValidationResult.result)
                {
                    ValidationMessage = emailValidationResult.message;
                    return null;
                }
                userSettings.Email = Email;
            }
            appSettings.UserSettings = userSettings;

            var isSmallBreakValid = IntergerBoundValidationController.Check(1, 180, this.SmallBreakDuration, "Small Break Duration");
            var isLargeBreakDurationValid = IntergerBoundValidationController.Check(1, 180, this.LargeBreakDuration, "Large Break Duration");
            var isPomodoroDurationValid = IntergerBoundValidationController.Check(1, 180, this.PomodoroDuration, "Pomodoro Duration");
            var isSessionPomodoroCountValid = IntergerBoundValidationController.Check(1, 180, this.SessionPomodoroCount, "Session Pomodoro Count");

            if (!isPomodoroDurationValid.result)
            {
                ValidationMessage = isPomodoroDurationValid.message;
                return null;
            }
            if (!isSmallBreakValid.result)
            {
                ValidationMessage = isSmallBreakValid.message;
                return null;
            }
            if (!isLargeBreakDurationValid.result)
            {
                ValidationMessage = isLargeBreakDurationValid.message;
                return null;
            }
            if (!isSessionPomodoroCountValid.result)
            {
                ValidationMessage = isSessionPomodoroCountValid.message;
                return null;
            }


            pomodoroSettings.AutoContinue = false;
            pomodoroSettings.SessionPomodoroCount = SessionPomodoroCount;
            pomodoroSettings.PomodoroBreakDuration = TimeSpan.FromMinutes(SmallBreakDuration);
            pomodoroSettings.SessionBreakDuration = TimeSpan.FromMinutes(LargeBreakDuration);
            pomodoroSettings.PomodoroDuration = TimeSpan.FromMinutes(PomodoroDuration);
            appSettings.PomodoroSettings = pomodoroSettings;




            return appSettings;
        }
    }
}
