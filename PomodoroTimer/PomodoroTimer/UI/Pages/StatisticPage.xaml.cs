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
    public partial class StatisticPage : ContentPage
    {
        private StatisticPageViewModel _viewModel;

        public StatisticPageViewModel ViewModel
        {
            get { return _viewModel; }
            set
            {
                _viewModel = value;
                BindingContext = _viewModel;
            }
        }

        public StatisticPage()
        {
            InitializeComponent();
            ViewModel = new StatisticPageViewModel(AppMainService.Instance.StorageService);
        }
    }
}