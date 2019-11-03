using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace XamarinHelperLib.MVVM
{
    /// <summary>
    /// To initilize view model and associate with services
    /// </summary>
    public interface IViewModelPresenter<ViewModelType>
    {
        /// <summary>
        /// View Model
        /// </summary>
        /// <returns></returns>
        ViewModelType Model { get; set; }

        /// <summary>
        /// Initlize view Model
        /// </summary>
        Task InitilizeAsync();
    }
}
