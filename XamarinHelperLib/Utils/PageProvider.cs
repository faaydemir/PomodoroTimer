
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using XamarinHelpers.Controls;

namespace XamarinHelpers.Utils
{
    public enum PageLifeTime
    {
        StayAlive,
        OneTime,
    }


    /// <summary>
    /// Page Provider to create instance of page
    /// </summary>
    public class PageProvider
    {
        private readonly Stack<Type> PageStack;
        private readonly Dictionary<Type, Page> Pages;
        private readonly Dictionary<Type, PageLifeTime> PageLifeTimes;
        private readonly Dictionary<Type, object> PageViewModels;


        /// <summary>
        /// Default Constructor
        /// </summary>
        public PageProvider()
        {
            PageStack = new Stack<Type>();
            Pages = new Dictionary<Type, Page>();
            PageLifeTimes = new Dictionary<Type, PageLifeTime>();
        }

        public void SetLifeTime(Type type, PageLifeTime pageLifeTime)
        {
            PageLifeTimes[type] = pageLifeTime;
        }

        public Page Get(Type pageType)
        {
            PageLifeTime pageLifeTime = PageLifeTime.StayAlive;

            if (PageLifeTimes.ContainsKey(pageType))
            {
                pageLifeTime = PageLifeTimes[pageType];
            }

            /*
            if page instance is not created before 
            create page instance
            */
            if (pageLifeTime == PageLifeTime.StayAlive)
            {
                if (!Pages.ContainsKey(pageType))
                    Pages[pageType] = CreatePage(pageType);
                /*
                 push type to stack for history.
                 */
                PageStack.Push(pageType);

                // return insctance
                return Pages[pageType];
            }
            else
            {
                PageStack.Push(pageType);
                return CreatePage(pageType);
            }
        }

        public Page GetPrevious()
        {
            if (PageStack.Count == 1)
                return null;

            PageStack.Pop();

            return Get(PageStack.Peek());
        }


        private Page CreatePage(Type pageType)
        {
            var page = (Xamarin.Forms.Page)Activator.CreateInstance(pageType);
            var detailPage = new CustomNavigationPage(page)
            {
            };
            return detailPage;
        }
    }
}
