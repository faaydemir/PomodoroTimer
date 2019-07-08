using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

/*
from https://github.com/MGohil/XFThemesSample/blob/master/XFThemes/XFThemes/ThemeManager.cs
with some modifications.
properties in changed theme must be used as dynamicresource.
*/

namespace XamarinHelperLib.ThemeManager
{
    /// <summary>
    /// ThemeManager 
    /// </summary>
    public class ThemeManager
    {

        /// <summary>
        /// Changes the theme of the app.
        /// Add additional switch cases for more themes you add to the app.
        /// This also updates the local key storage value for the selected theme.
        /// </summary>
        /// <param name="theme"></param>
        public static void ChangeTheme(AppTheme theme)
        {
            var mergedDictionaries = Application.Current.Resources.MergedDictionaries;
            if (mergedDictionaries != null)
            {
                mergedDictionaries.Clear();

                //TODO check if merged dictionaries contain theme

                mergedDictionaries.Add(theme.Resource);
            }
        }

    }
}
