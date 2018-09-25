using PomodoroTimer.Models;
using PomodoroTimer.ViewModels;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PomodoroTimer.Views
{
    public partial class MainPage : MasterDetailPage
    {
        string currentPage;
        public MainPage()
        {
            InitializeComponent();
            Menu.ListView.ItemSelected += ListView_ItemSelected;
            currentPage = "Home";
        }

        private  void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MasterViewMenuItem;
            if (item == null)
                return;
            currentPage = item.Title;
            var page = (Xamarin.Forms.Page)Activator.CreateInstance(item.TargetType);

            Detail = new CustomNavigationPage(page)
            {

            };

            
            IsPresented = false;
            Menu.ListView.SelectedItem = null;
        }

    }
}