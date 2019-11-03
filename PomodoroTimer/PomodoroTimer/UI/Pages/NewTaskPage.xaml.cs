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
        private NewTaskPageViewModel _viewModel;

        private NewTaskPageViewModel ViewModel
        {
            get
            {
                return _viewModel;
            }
            set
            {
                _viewModel = value;
                BindingContext = _viewModel;
            }
        }

        public NewItemPage()
        {


            InitializeComponent();
            ViewModel = new NewTaskPageViewModel
            {
                Page = this
            };
            picker.DialogParent = MainContent;
        }

        public NewItemPage(UserTask userTask)
        {
            InitializeComponent();
            ViewModel = new NewTaskPageViewModel(userTask)
            {
                Page = this
            };
            picker.DialogParent = MainContent;
        }

        private void ItemsListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            picker.DialogFinished?.Execute(null);
        }
    }
}