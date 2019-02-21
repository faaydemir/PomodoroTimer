
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using XamarinHelpers.Controls;

namespace XamarinHelpers.Utils
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
            /*
            if page instance is not created before 
            create page instance
            */

            if (!Pages.ContainsKey(pageType))
                Pages[pageType] = CreatePage(pageType);

            /*
             push type to stack for history.
             */

            PageStack.Push(pageType);

            // return insctance
            return Pages[pageType];
        }

        public Page GetPrevious()
        {
            if (PageStack.Count == 1)
                return null;

            PageStack.Pop();

            return Pages[PageStack.Peek()];
        }
    }
}
