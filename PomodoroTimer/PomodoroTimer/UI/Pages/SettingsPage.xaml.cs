using PomodoroTimer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PomodoroTimer.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {

        private SettingsViewModel _viewModel;

        public SettingsViewModel ViewModel
        {
            get => _viewModel;
            set
            {
                _viewModel = value;
                BindingContext = _viewModel;
            }
        }


        public SettingsPage()
        {
            InitializeComponent();
            if (ViewModel == null)
                ViewModel = new SettingsViewModel(AppMainService.Instance) { Page = this };
        }
    }
}