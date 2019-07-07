using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;


namespace XamarinHelpers.Behaviors
{
    //from https://www.c-sharpcorner.com/article/input-validation-in-xamarin-forms-behaviors/
    public class NumberValidationBehavior : Behavior<Entry>
    {
        protected override void OnAttachedTo(Entry entry)
        {
            entry.TextChanged += OnEntryTextChanged;
            base.OnAttachedTo(entry);
        }

        protected override void OnDetachingFrom(Entry entry)
        {
            entry.TextChanged -= OnEntryTextChanged;
            base.OnDetachingFrom(entry);
        }

        void OnEntryTextChanged(object sender, TextChangedEventArgs args)
        {

            bool isValid = int.TryParse(args.NewTextValue, out int result);
            if (!isValid)
            {
                ((Entry)sender).Text = "0";
            }
            //((Entry)sender).TextColor = isValid ? Color.Default : Color.Red;
        }
    }
}
