using PomodoroTimer.Models;
using PomodoroTimer.ViewModels;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PomodoroTimer.Views
{
    public class PageProvider
    {
        private Stack<Type> PageStack = new Stack<Type>();
        private Dictionary<Type, Page> Pages = new Dictionary<Type, Page>();

        private Page CreatePage(Type pageType)
        {
            var page = (Xamarin.Forms.Page)Activator.CreateInstance(pageType);
            var detailPage = new CustomNavigationPage(page)
            {

            };
            return detailPage;
        }

        public Page Go(Type pageType)
        {
            if (!Pages.ContainsKey(pageType))
                Pages[pageType] = CreatePage(pageType);
            PageStack.Push(pageType);
            return Pages[pageType];
        }
        public Page Back()
        {
            if (PageStack.Count == 1)
                return null;
            PageStack.Pop();

            return Pages[PageStack.Peek()];
        }
    }

    public partial class MainPage : MasterDetailPage
    {

        PageProvider PageProvider;
        public MainPage()
        {
            PageProvider = new PageProvider();
            InitializeComponent();
            Menu.ListView.ItemSelected += ListView_ItemSelected;
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MasterViewMenuItem;
            if (item == null)
                return;

            var detailPage = PageProvider.Go(item.TargetType);
            Detail = detailPage;

            IsPresented = false;
            Menu.ListView.SelectedItem = null;
        }

        protected override bool OnBackButtonPressed()
        {
            var previus = PageProvider.Back();
            if (previus == null)
            {
                return base.OnBackButtonPressed();
            }
            else
            {
                Detail = previus;
                return true;
            }
        }
    }
}