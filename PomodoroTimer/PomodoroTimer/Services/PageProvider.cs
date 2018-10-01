using PomodoroTimer.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace PomodoroTimer.Services
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

        public Page Get(Type pageType)
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
}
