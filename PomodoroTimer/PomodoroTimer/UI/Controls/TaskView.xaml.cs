using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PomodoroTimer.Controls
{

    public partial class TaskView : FlexLayout
    {
        public static readonly BindableProperty OnUserTaskSelectedProperty =
            BindableProperty.Create(nameof(OnUserTaskSelected), typeof(ICommand), typeof(TaskView), null);
        public ICommand OnUserTaskSelected
        {
            get { return (ICommand)GetValue(OnUserTaskSelectedProperty); }
            set { SetValue(OnUserTaskSelectedProperty, value); OnPropertyChanged(nameof(OnUserTaskSelected)); }
        }
        public TaskView()
        {
            InitializeComponent();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            OnUserTaskSelected?.Execute(BindingContext);
        }
    }
}