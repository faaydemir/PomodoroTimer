using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace XamarinHelpers.MVVM
{
    public interface IPageViewModel
    {
        Page Page { get; set; }
        bool IsBusy { get; set; }
        string Title { get; set; }
        bool Initilize();
    }
}
