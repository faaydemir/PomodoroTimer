#region usings
using Helper.Services;
using Microcharts;
using PomodoroTimer.Models;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using XamarinHelpers.MVVM;
#endregion

namespace PomodoroTimer.ViewModels
{
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
        public Chart PointChart
        {
            get { return _weeklyPointChart; }
            set { SetProperty(ref _weeklyPointChart, value); }
        }

        public ChartingViewModel(List<TaskStatistic> statistics, DateTime startTime, DateTime finishtime)
        {
            StartDay = startTime;
            FinishDay = finishtime;
            _statistics = statistics;

            PointChart = new PointChart()
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
            var entries = new List<Microcharts.ChartEntry>();

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

                var entry = new Microcharts.ChartEntry(item.Value.count)
                {
                    Label = item.Value.name,
                    ValueLabel = item.Value.count.ToString(),
                    Color = SKColor.Parse(ColorPickService.GetRandom()),

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

        private void DrawPointChart(List<TaskStatistic> statistics)
        {
            var dayGroup = statistics.GroupBy(u => u.FinishedTime.Day)
                                     .ToDictionary(x => x.Key, x => x.ToList().Count());

            //fill zero for days that has no statistic
            for (int i = StartDay.Day; i <= FinishDay.Day; i++)
            {
                if (!dayGroup.ContainsKey(i))
                {
                    dayGroup[i] = 0;
                }
            }

            var entries = dayGroup.OrderBy(x => x.Key)
                              .Select(x =>
                              {
                                  return new Microcharts.ChartEntry(x.Value)
                                  {
                                      Label = x.Key.ToString(),
                                      ValueLabel = x.Value.ToString(),
                                      Color = SKColor.Parse(ColorPickService.GetByKey(x.Key)),
                                      TextColor = SKColor.Parse(ColorPickService.GetByKey(x.Key))
                                  };
                              });

            PointChart = new Microcharts.PointChart()
            {
                IsAnimated = false,
                LabelTextSize = 25,
                LabelOrientation = Orientation.Vertical,
                ValueLabelOrientation = Orientation.Vertical,
                PointSize = 25,
                Entries = entries,
                BackgroundColor = SkiaSharp.SKColors.Transparent,
                Margin = 15,
            };
        }
    }
}
