using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using PomodoroTimer.Models;
using PomodoroTimer.ViewModels;
using PomodoroTimer.Messaging;

namespace PomodoroTimer.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewItemPage : ContentPage
    {

        private NewTaskPageViewModel ViewModel { get; set; }

        public NewItemPage()
        {

            ViewModel = new NewTaskPageViewModel
            {
                Page = this
            };
            InitializeComponent();
            BindingContext = ViewModel;

            picker.DialogParent = MainContent;
        }

        public NewItemPage(UserTask userTask)
        {

            ViewModel = new NewTaskPageViewModel(userTask)
            {
                Page = this
            };

            InitializeComponent();
            BindingContext = ViewModel;
            picker.DialogParent = MainContent;
        }

        private void ItemsListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            picker.DialogFinished?.Execute(null);
        }
    }
}