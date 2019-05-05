using System;
using System.Collections.Generic;
using System.Text;
//#laterusable
namespace MovieVocabulary.Utils
{
    
    public abstract class SingletonBase<BaseType> where BaseType : class
    {
        private static readonly Lazy<BaseType> lazy = new Lazy<BaseType>(() => Create());

        public static BaseType Instance { get { return lazy.Value; } }

        private static BaseType Create()
        {
            Type instanceType = typeof(BaseType);

            return (BaseType)Activator.CreateInstance(instanceType);
        }
    }
}
