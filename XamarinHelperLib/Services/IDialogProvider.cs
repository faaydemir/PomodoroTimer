using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace XamarinHelpers.Services
{
    //from https://xamarinforms.wordpress.com/2014/12/09/creating-a-xamarin-forms-app-part-9-working-with-alerts-and-dialogs/
    public interface IDialogProvider
    {
        Task DisplayAlert(string title, string message, string cancel);

        Task<bool> DisplayAlert(string title, string message, string accept, string cancel);

        Task<string> DisplayActionSheet(string title, string cancel, string destruction, params string[] buttons);
    }
}
