using Microcharts;
using Plugin.LocalNotifications;
using PomodoroTimer.ViewModels;
using SkiaSharp;
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
    public partial class HomePage : ContentPage
    {

        HomeViewModel vm;


        public HomeViewModel ViewModel
        {
            get => vm; set
            {
                vm = value;
                vm.Page = this;
                BindingContext = vm;
            }
        }

        public HomePage()
        { 
            InitializeComponent();
            
            if (vm == null)
                ViewModel = new HomeViewModel(AppMainService.Instance);
        }
    }
}