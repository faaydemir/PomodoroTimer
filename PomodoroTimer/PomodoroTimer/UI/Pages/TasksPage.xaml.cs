using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using PomodoroTimer.Models;
using PomodoroTimer.Views;
using PomodoroTimer.ViewModels;

namespace PomodoroTimer.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TasksPage : ContentPage
    {
        TasksViewModel viewModel;

        private TasksViewModel _viewModel;

        public TasksViewModel ViewModel
        {
            get { return _viewModel; }
            set {
                _viewModel = value;
                BindingContext = _viewModel;
            }
        }


        public TasksPage()
        {
            InitializeComponent();
            ViewModel = new TasksViewModel(AppMainService.Instance) { Page = this };

        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewItemPage()));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.UserTasks.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }
    }
}