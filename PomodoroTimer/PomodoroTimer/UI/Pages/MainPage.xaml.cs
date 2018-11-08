using PomodoroTimer.Models;
using PomodoroTimer.Services;
using PomodoroTimer.ViewModels;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PomodoroTimer.Views
{
    [XamlCompilation (XamlCompilationOptions.Compile)]
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
            var previus = PageProvider.Back();
            if (previus == null)
            {
                return base.OnBackButtonPressed();
            }
            else
            {
                Detail = previus;
                return true;
            }
        }
    }
}