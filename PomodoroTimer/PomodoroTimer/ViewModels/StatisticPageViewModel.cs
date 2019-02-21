using Helper.Services;
using Microcharts;
using PomodoroTimer.Models;
using PomodoroTimer.Services;
using PomodoroTimer.Services.Interfaces;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
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
        private IAppService AppService { get; set; }

        private ObservableCollection<ChartingViewModel> _chartViewModels = new ObservableCollection<ChartingViewModel>();

        public ObservableCollection<ChartingViewModel> ChartViewModels
        {
            get { return _chartViewModels; }
            set { SetProperty(ref _chartViewModels, value); }
        }

        private int _position = 0;

        public int Position
        {
            get { return _position; }
            set
            {   
                if (_position==value)
                {
                    return;
                }
                SetProperty(ref _position, value);
                //TODO add dynamic cache
                //if (value == 1)
                //{
                //    //add previous date
                //    ChartViewModels.Add(0, GetPrevious());
                //    _position = value + 1;
                //    OnPropertyChanged();
                //}
                //else if (value == ChartViewModels.Count - 1)
                //{
                //    // check if date is last of ChartViewModels's data is current 

                //    //if not add next date

                //    // set property
                //    SetProperty(ref _position, value);
                //}
                //else
                //{

                //}
            }
        }


        public int IntervalType
        {
            get { return _selectedDate; }
            set
            {
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
                else
                {
                    IntervalType = DayConstant;
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
        public StatisticPageViewModel(IAppService appService)
        {
            AppService = appService;
            ChartViewModels = new ObservableCollection<ChartingViewModel>();
            IntervalType = DayConstant;

        }

        private void Init()
        {
            var chartViewModels = new ObservableCollection<ChartingViewModel>
            {
                new ChartingViewModel(AppService.GetStatisticData(StartDay, FinishDay), StartDay, FinishDay)
            };

            for (int i = 0; i < CachedCount - 1; i++)
            {
                chartViewModels.Add(AddPrevious());
            }
            chartViewModels.Reverse();
            ChartViewModels = chartViewModels;
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

            return new ChartingViewModel(AppService.GetStatisticData(StartDay, FinishDay), StartDay, FinishDay);
        }

        private ChartingViewModel GetNext()
        {
            DateTime newStartDate;
            DateTime newFinishDate;
            if (IntervalType == DayConstant)
            {
                newStartDate = StartDay.AddDays(1);
                newFinishDate = FinishDay.AddDays(1);
            }
            else if (IntervalType == WeekConstant)
            {
                newStartDate = StartDay.AddDays(7).FirstDayOfWeek();
                newFinishDate = FinishDay.AddDays(7).LastDayOfWeek();
            }
            else
            {
                var previousMonth = StartDay.AddMonths(1);
                newStartDate = new DateTime(previousMonth.Year, previousMonth.Month, 1);
                newFinishDate = StartDay.AddMonths(1).AddDays(-1);
            }
            // start date can be future date
            if (newStartDate < DateTime.Now)
            {
                StartDay = newStartDate;
                FinishDay = newFinishDate;
            }

            return new ChartingViewModel(AppService.GetStatisticData(StartDay, FinishDay), StartDay, FinishDay);
        }

    }
    public class ChartingViewModel : PageViewModel
    {

        private Chart _taskDonutChart;
        private Chart _weeklyPointChart;
        private DateTime _startTime;
        private DateTime _finishTime;

        private List<TaskStatistic> _statistics;

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

        public Chart TaskDonutChart
        {
            get { return _taskDonutChart; }
            set { SetProperty(ref _taskDonutChart, value); }
        }
        public Chart WeeklyPointChart
        {
            get { return _weeklyPointChart; }
            set { SetProperty(ref _weeklyPointChart, value); }
        }

        public ChartingViewModel(List<TaskStatistic> statistics, DateTime startTime, DateTime finishtime)
        {
            StartDay = startTime;
            FinishDay = finishtime;
            _statistics = statistics;

            WeeklyPointChart = new PointChart()
            {
                IsAnimated = true,
                BackgroundColor = SkiaSharp.SKColors.Transparent,
                Margin = 0,
            };
            TaskDonutChart = new DonutChart()
            {
                IsAnimated = true,
                BackgroundColor = SkiaSharp.SKColors.Transparent,
                Margin = 0,
            };

            //Previous = new Command(
            //     execute: async () =>
            //     {
            //         PreviousTimeInterval();
            //     });

            //Next = new Command(
            //     execute: async () =>
            //     {
            //         NextTimeInterval();
            //     });


            UpdateCharts();
        }



        private void UpdateCharts()
        {

            if ((_statistics != null) && (!_statistics.Any()))
            {
                //var notificator = DependencyService.Get<INotification>();
                //notificator.Show("Not have any data.");

            }
            else
            {
                IsBusy = true;
                DrawPointChart(_statistics);
                DrawDonutChart(_statistics);
                IsBusy = false;
            }
        }

        private void DrawDonutChart(List<TaskStatistic> statistics)
        {
            var entries = new List<Microcharts.Entry>();
            Dictionary<Guid, (int count, string name)> taskDictionary = new Dictionary<Guid, (int count, string name)>();

            foreach (var statistic in statistics)
            {
                if (!taskDictionary.ContainsKey(statistic.TaskId))
                {
                    taskDictionary[statistic.TaskId] = (count: 1, name: statistic.TaskName);
                }
                else
                {
                    taskDictionary[statistic.TaskId] = (count: taskDictionary[statistic.TaskId].count + 1, name: taskDictionary[statistic.TaskId].name);
                }
            }

            foreach (var item in taskDictionary)
            {

                var entry = new Microcharts.Entry(item.Value.count)
                {
                    Label = item.Value.name,
                    ValueLabel = item.Value.count.ToString(),
                    Color = SKColor.Parse(ColorPickService.GetRandom())
                };
                entries.Add(entry);

            }

            TaskDonutChart = new Microcharts.DonutChart()
            {
                IsAnimated = true,
                AnimationDuration = TimeSpan.FromMilliseconds(300),
                LabelTextSize = 25,
                Entries = entries,
                BackgroundColor = SkiaSharp.SKColors.Transparent,
                Margin = 10,
            };
        }
        private void DrawPointChart(List<TaskStatistic> Statistic)
        {
            var entries = new List<Microcharts.Entry>();
            var DayGroup = Statistic.GroupBy(u => u.FinishedTime.Day).ToDictionary(x => x.Key, x => x.ToList());
            foreach (var group in DayGroup)
            {
                var value = group.Value.Count;
                var day = group.Value[0].FinishedTime.Day;
                var entry = new Microcharts.Entry(value)
                {
                    Label = group.Key.ToString(),
                    ValueLabel = value.ToString(),
                    Color = SKColor.Parse(ColorPickService.GetRandom())
                };
                entries.Add(entry);
            }

            WeeklyPointChart = new Microcharts.PointChart()
            {
                IsAnimated = false,
                LabelTextSize = 25,
                LabelOrientation = Orientation.Horizontal,
                ValueLabelOrientation = Orientation.Horizontal,
                PointSize = 25,
                Entries = entries,
                BackgroundColor = SkiaSharp.SKColors.Transparent,
                Margin = 15,
            };
        }
    }
}
