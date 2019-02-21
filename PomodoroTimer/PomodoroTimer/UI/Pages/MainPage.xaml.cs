using PomodoroTimer.Models;
using PomodoroTimer.Services;
using PomodoroTimer.ViewModels;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinHelpers.Services;

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
            Detail = PageProvider.Get(typeof(HomePage));
            Menu.ListView.ItemSelected += ListView_ItemSelected;
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MasterViewMenuItem;
            if (item == null)
                return;

            var detailPage = PageProvider.Get(item.TargetType);
            Detail = detailPage;
            IsPresented = false;
            Menu.ListView.SelectedItem = null;
        }

        protected override bool OnBackButtonPressed()
        {
            bool exitapp = true; ;
            var previous = PageProvider.GetPrevious();
            if (previous == null)
            {

                if (AppMainService.Instance.PomodoroStatus?.TimerState == Enums.TimerState.Running)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        var displayAlert = new DialogService(Detail);
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
    }
}