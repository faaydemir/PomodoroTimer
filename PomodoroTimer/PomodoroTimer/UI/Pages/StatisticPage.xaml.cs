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
		public StatisticPage ()
		{
			InitializeComponent ();
            BindingContext = new StatisticPageViewModel(AppMainService.Instance.StorageService);
		}

        private void SwipeGestureRecognizer_Swiped(object sender, SwipedEventArgs e)
        {

        }
    }
}