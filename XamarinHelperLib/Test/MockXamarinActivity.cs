using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinHelperLib.Test
{

    /// <summary>
    /// Mock Activity to Test LifeCycle 
    /// </summary>
    public class MockXamarinActivity
    {
        #region Props
        public Action OnStart { get; set; }
        public Action OnDestroy { get; set; }
        public Action OnResume { get; set; }
        public Action OnConstrcut { get; set; }
        #endregion


        #region Methods
        public void Constrcut()
        {
            this.OnConstrcut.Invoke();
        }

        public void Destroy()
        {
            this.OnDestroy.Invoke();
        }

        public void Resume()
        {
            this.OnResume.Invoke();
        }

        public void Start()
        {
            this.OnStart.Invoke();
        }
        #endregion
    }
}
