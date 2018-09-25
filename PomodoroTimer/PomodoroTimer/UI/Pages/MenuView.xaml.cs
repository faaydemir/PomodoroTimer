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
	public partial class MenuView : ContentPage
	{
        public ListView ListView;
        public MenuView ()
		{
			InitializeComponent ();
            BindingContext = new MasterViewModel();
            ListView = menuItemsListView;
        }
        
	}
}