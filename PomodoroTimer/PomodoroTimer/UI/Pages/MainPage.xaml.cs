using PomodoroTimer.Models;
using PomodoroTimer.Services;
using PomodoroTimer.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinHelpers.Utils;

namespace PomodoroTimer.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : MasterDetailPage
    {

        PageProvider PageProvider;

        public MainPage()
        {
            InitializeComponent();
            PageProvider = new PageProvider();
            PageProvider.SetLifeTime(typeof(StatisticPage), PageLifeTime.OneTime);
            Detail = PageProvider.Get(typeof(HomePage));
            Menu.ListView.ItemSelected += ListView_ItemSelected;
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                if (!(e.SelectedItem is MasterViewMenuItem item))
                    return;

                var detailPage = PageProvider.Get(item.TargetType);

                IsPresented = false;
                Detail = detailPage;
                Menu.ListView.SelectedItem = null;
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.Message);
            }


        }

        protected override bool OnBackButtonPressed()
        {

            try
            {


                bool exitapp = true; ;
                var previous = PageProvider.GetPrevious();
                if (previous == null)
                {

                    if (AppMainService.Instance.PomodoroStatus?.TimerState == Enums.TimerState.Running)
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            var displayAlert = new DialogProvider(Detail);
                            var cancelRunnigTimer = await displayAlert.DisplayAlert("Cancel Timer", "Running timer will be stopped. Do you want to continue ?", "ok", "cancel");
                            if (cancelRunnigTimer)
                            {
                                AppMainService.Instance.StopPomodoro();
                                exitapp = true;
                            }
                            else
                            {
                                exitapp = false;
                            }
                            if (exitapp)
                            {
                                OnBackButtonPressed();
                            }
                        });
                        return true;
                    }
                    else
                    {
                        base.OnBackButtonPressed();
                        return false;
                    }


                }
                else
                {
                    Detail = previous;
                    return true;
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.Message);
                return true;
            }
        }
    }
}