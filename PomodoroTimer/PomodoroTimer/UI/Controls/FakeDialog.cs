using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Forms;
// #laterusable
namespace PomodoroTimer.Controls
{
    public class FakeDialog : ContentView
    {
        private Command _dialogFinished;
        private Grid DialogView;
        private View SelectionView;

        public Layout<View> DialogParent { get; set; }
        public Thickness DialogPadding { get; set; } = new Thickness(30);
        public float CornerRadius { get; set; } = 4;


        public static readonly BindableProperty CloseDialogProperty =
                    BindableProperty.Create(
                    "CloseDialog",
                    typeof(Command),
                    typeof(FakeDialog),
                    defaultValue: null,
                    defaultBindingMode: BindingMode.OneWay);

        public static readonly BindableProperty ExpandTemplateProperty =
                    BindableProperty.Create(
                    "DialogTemplate",
                    typeof(DataTemplate),
                    typeof(FakeDialog),
                    defaultValue: null,
                    defaultBindingMode: BindingMode.OneWay,
                    propertyChanged: OnTemplateChanged);

        public static readonly BindableProperty SelectionTemplateProperty =
                BindableProperty.Create(
                    "SelectionTemplate",
                    typeof(DataTemplate),
                    typeof(FakeDialog),
                    defaultValue: null,
                    defaultBindingMode: BindingMode.OneWay,
                    propertyChanged: OnTemplateChanged);

        public DataTemplate DialogTemplate
        {
            get { return (DataTemplate)GetValue(ExpandTemplateProperty); }
            set { SetValue(ExpandTemplateProperty, value); }
        }

        public DataTemplate SelectionTemplate
        {
            get { return (DataTemplate)GetValue(SelectionTemplateProperty); }
            set { SetValue(SelectionTemplateProperty, value); }
        }

        public Command DialogFinished
        {
            get { return _dialogFinished; }
            set
            {
                _dialogFinished = value;
                OnPropertyChanged();
            }
        }

        public Command CloseDialog
        {
            get { return (Command)GetValue(CloseDialogProperty); }
            set
            {
                SetValue(CloseDialogProperty, value);
            }
        }
        public FakeDialog()
        {
            this.BindingContext = this;

            DialogFinished = new Command(
                execute: (o) =>
                {
                    this.CloseDialog?.Execute(o);
                    OnDeleteDialogTapped(null, null);
                });
        }

        public void Build()
        {
            try
            {


                var selection = SelectionTemplate?.CreateContent() as View;
                var expand = DialogTemplate?.CreateContent() as View;
                DialogView = CreateDialog(expand);
                SelectionView = CreateSelection(selection);
                Content = SelectionView;
            }
            catch (Exception ex)
            {

            }
        }

        private static void OnTemplateChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = bindable as FakeDialog;
            control.Build();
        }
        private View CreateSelection(View selection)
        {
            var tapGestureRecognizer = new TapGestureRecognizer { NumberOfTapsRequired = 1 };
            tapGestureRecognizer.Tapped += OnCreateDialogTapped;

            if (selection == null)
            {
                return null;
            }

            var container = new Grid()
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };

            var grid = new Grid()
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };

            grid.GestureRecognizers.Add(tapGestureRecognizer);
            container.Children.Add(selection);
            container.Children.Add(grid);
            return container;
        }
        public Grid CreateDialog(View expandView)
        {
            var tapGestureRecognizer = new TapGestureRecognizer { NumberOfTapsRequired = 1 };
            tapGestureRecognizer.Tapped += OnDeleteDialogTapped;

            if (expandView == null)
            {
                return null;
            }

            var container = new Grid()
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };

            var grid = new Grid()
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor = Color.Black,
                Opacity = 0.3,
            };

            grid.GestureRecognizers.Add(tapGestureRecognizer);
            var frame = new Frame()
            {
                Margin = DialogPadding,
                CornerRadius = CornerRadius,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,

            };
            frame.Content = expandView;
            container.Children.Add(grid);
            container.Children.Add(frame);
            return container;
        }
        private void OnCreateDialogTapped(object sender, EventArgs e)
        {
            DialogParent.Children.Add(DialogView);
        }
        private void OnDeleteDialogTapped(object sender, EventArgs e)
        {
            if (DialogView != null)
                DialogParent.Children.Remove(DialogView);
        }

    }
}
