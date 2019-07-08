using PomodoroTimer.ViewModels;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PomodoroTimer.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AboutPage : ContentPage
    {
        private AboutViewModel _viewModel;

        public AboutViewModel ViewModel
        {
            get { return _viewModel; }
            set
            {
                _viewModel = value;
                BindingContext = _viewModel;
            }
        }

        public AboutPage()
        {
            InitializeComponent();
            BindingContext = new AboutViewModel();
        }
    }
}