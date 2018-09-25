using PomodoroTimer.ViewModels;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PomodoroTimer.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AboutPage : ContentPage
	{
		public AboutPage ()
		{
            BindingContext = new AboutViewModel();

            InitializeComponent ();
		}
	}
}