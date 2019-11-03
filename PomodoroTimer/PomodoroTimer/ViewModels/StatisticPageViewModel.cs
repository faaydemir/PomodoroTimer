using Helper.Services;
using Microcharts;
using PomodoroTimer.Models;
using PomodoroTimer.Services;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using XamarinHelpers.MVVM;
using Helpers.Extentions;

namespace PomodoroTimer.ViewModels
{
    public class StatisticPageViewModel : PageViewModel
    {
        private readonly int MonthConstant = 2;
        private readonly int WeekConstant = 1;
        private readonly int DayConstant = 0;
        private int CachedCount = 30;
        private DateTime _startTime;
        private DateTime _finishTime;
        private int _selectedDate;
        private int _position = 0;

        private IStorageService StorageService { get; set; }

        private ObservableCollection<ChartingViewModel> _chartViewModels = new ObservableCollection<ChartingViewModel>();

        public ObservableCollection<ChartingViewModel> ChartViewModels
        {
            get { return _chartViewModels; }
            set { SetProperty(ref _chartViewModels, value); }
        }

        public int Position
        {
            get { return _position; }
            set
            {
                SetProperty(ref _position, value);
            }
        }

        public int IntervalType
        {
            get { return _selectedDate; }
            set
            {
                if (value != _selectedDate)
                    SetProperty(ref _selectedDate, value);

                if (IntervalType == DayConstant)
                {
                    FinishDay = DateTime.Today;
                    StartDay = DateTime.Today;
                    CachedCount = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
                }
                else if (IntervalType == WeekConstant)
                {
                    StartDay = DateTime.Now.FirstDayOfWeek();
                    FinishDay = DateTime.Now.LastDayOfWeek();
                    CachedCount = 20;
                }
                else if (IntervalType == MonthConstant)
                {
                    DateTime now = DateTime.Now;
                    StartDay = new DateTime(now.Year, now.Month, 1);
                    FinishDay = StartDay.AddMonths(1).AddDays(-1);
                    CachedCount = 12;
                }

                Init();
            }
        }
        public DateTime StartDay
        {
            get { return _startTime; }
            set { SetProperty(ref _startTime, value); }
        }

        public DateTime FinishDay
        {
            get { return _finishTime; }
            set { SetProperty(ref _finishTime, value); }
        }
        public StatisticPageViewModel(IStorageService storageService)
        {
            StorageService = storageService;
            ChartViewModels = new ObservableCollection<ChartingViewModel>();
            IntervalType = DayConstant;

        }

        private async void Init()
        {
            var chartViewModels = new ObservableCollection<ChartingViewModel>
            {
                new ChartingViewModel(StorageService.GetStatisticData(StartDay, FinishDay), StartDay, FinishDay)
            };

            for (int i = 0; i < CachedCount - 1; i++)
            {
                chartViewModels.Add(AddPrevious());
            }

            Position = 0;
            await Task.Delay(100);
            ChartViewModels = new ObservableCollection<ChartingViewModel>(chartViewModels.Reverse());

            await Task.Delay(200);
            Position = ChartViewModels.Count - 1;
        }

        private ChartingViewModel AddPrevious()
        {

            if (IntervalType == DayConstant)
            {
                StartDay = StartDay.AddDays(-1);
                FinishDay = FinishDay.AddDays(-1);
            }
            else if (IntervalType == WeekConstant)
            {
                StartDay = StartDay.AddDays(-7).FirstDayOfWeek();
                FinishDay = FinishDay.AddDays(-7).LastDayOfWeek();
            }
            else
            {
                var previousMonth = StartDay.AddMonths(-1);
                StartDay = new DateTime(previousMonth.Year, previousMonth.Month, 1);
                FinishDay = StartDay.AddMonths(1).AddDays(-1);
            }

            return new ChartingViewModel(StorageService.GetStatisticData(StartDay, FinishDay), StartDay, FinishDay);
        }
    }
}
