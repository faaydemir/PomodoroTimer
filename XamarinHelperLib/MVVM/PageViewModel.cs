using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace XamarinHelpers.MVVM
{
    public abstract class PageViewModel : BaseViewModel, IPageViewModel
    {
        bool isBusy = false;
        string title = string.Empty;

        public Page Page { get; set; }

        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        public bool Initilize()
        {
            throw new NotImplementedException();
        }
    }
}
