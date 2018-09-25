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

        public TasksPage()
        {
            BindingContext = viewModel = new TasksViewModel(AppMainService.Instance) { Page = this };
            viewModel.Page = this;
            InitializeComponent();
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