using System;
using System.Collections.Generic;
using System.Text;
//#laterusable
namespace MovieVocabulary.Utils
{
    /// <summary>
    ///  To Make A Type Singleton  
    /// </summary>
    /// <typeparam name="BaseType">Type that become singleton</typeparam>
    public abstract class SingletonBase<BaseType> where BaseType : class
    {
        private static readonly Lazy<BaseType> lazy = new Lazy<BaseType>(() => Create());

        /// <summary>
        /// Instance
        /// </summary>
        public static BaseType Instance { get { return lazy.Value; } }
        
        /// <summary>
        /// Create BaseType Instance
        /// </summary>
        /// <returns></returns>
        private static BaseType Create()
        {
            Type instanceType = typeof(BaseType);

            return (BaseType)Activator.CreateInstance(instanceType);
        }
    }
}
