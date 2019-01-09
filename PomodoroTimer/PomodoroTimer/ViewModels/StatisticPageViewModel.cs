using Microcharts;
using PomodoroTimer.Models;
using PomodoroTimer.Services;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace PomodoroTimer.ViewModels
{
    public class StatisticPageViewModel : PageViewModel
    {
        private Chart _taskDonutChart;
        private Chart _weeklyPointChart;
        private DateTime _startTime;
        private DateTime _finishTime;
        private DateTime _startMinDate;
        private DateTime _startMaxDate;
        private DateTime _finishMinDate;
        private DateTime _finishMaxDate;

        public ICommand Export { get; set; }
        public ICommand UpdateChart { get; set; }

        private IAppService AppService { get; set; }


        public DateTime StartMinDate
        {
            get { return _startMinDate; }
            set { SetProperty(ref _startMinDate, value); }
        }
        public DateTime StartMaxDate
        {
            get { return _startMaxDate; }
            set { SetProperty(ref _startMaxDate, value); }
        }
        public DateTime FinishMinDate
        {
            get { return _finishMinDate; }
            set { SetProperty(ref _finishMinDate, value); }
        }
        public DateTime FinishMaxDate
        {
            get { return _finishMaxDate; }
            set
            {
                SetProperty(ref _finishMaxDate, value);
            }
        }

        public DateTime StartTime
        {
            get { return _startTime; }
            set { SetProperty(ref _startTime, value); }
        }

        public DateTime FinishTime
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

        public StatisticPageViewModel(IAppService appService)
        {
            AppService = appService;
            StartMinDate = DateTime.Today.AddYears(-1);
            FinishMinDate = DateTime.Today.AddYears(-1);
            FinishMaxDate = DateTime.Today;
            StartMaxDate = DateTime.Today;
            FinishTime = DateTime.Today;
            StartTime = DateTime.Today.AddDays(-7);
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
            UpdateChart = new Command(
                execute: (o) =>
                {
                    if (StartTime < FinishTime.AddDays(-40))
                        StartTime = FinishTime.AddDays(-40);
                    UpdateChartsAsync();
                }
                ,
                canExecute: (o) =>
                {
                      return StartTime < FinishTime;
                }

            );
            //Export = new Command(
            //    execute: async (o) =>
            //    {

            //    }
            //);

            UpdateChartsAsync();
        }
        private Task ExportStatistics()
        {
            return Task.Run(() =>
            {
                AppService.Export(StartTime, FinishTime);
            });
        }
        private Task UpdateChartsAsync()
        {
            return Task.Run(() =>
            {
                List<TaskStatistic> statistics = AppService.GetStatisticData(StartTime, FinishTime);
                DrawPointChart(statistics);
                DrawDonutChart(statistics);

            });
        }
        private async void UpdateCharts()
        {
            await UpdateChartsAsync();
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
                    Color = SKColor.Parse(ColorPickService.NextReverse())
                };
                entries.Add(entry);

            }
            Device.BeginInvokeOnMainThread(() =>
               TaskDonutChart = new Microcharts.DonutChart()
               {
                   IsAnimated = true,
                   AnimationDuration = TimeSpan.FromMilliseconds(300),
                   LabelTextSize = 25,
                   Entries = entries,
                   BackgroundColor = SkiaSharp.SKColors.Transparent,
                   Margin = 10,
               }
             );
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
                    Color = SKColor.Parse(ColorPickService.NextReverse())
                };
                entries.Add(entry);
            }
            Device.BeginInvokeOnMainThread(() =>
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
            });
        }
    }
}
