using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
//#laterusable
namespace MovieVocabulary.Utils
{


    public abstract class AppDependencyServiceBase
    {
        protected bool IsInitilized { get; set; } = false;

        protected IDictionary<Type, object> Singletons { get; set; }
        protected IDictionary<Type, Func<object>> Factories { get; set; }
        protected IDictionary<Type, Func<Task<object>>> AsyncFactories { get; set; }
        protected abstract void Init();

        protected void Add(Type type, Func<object> createMetod)
        {
            Factories[type] = createMetod;
        }


        //next will be lazy  
        protected void AddSingleton(Type type, Func<object> createMetod)
        {
            Singletons[type] = createMetod.Invoke();
        }
        public async Task<T> GetAsync<T>() where T : class
        {
            if (!IsInitilized)
                Init();

            Type instanceType = typeof(T);

            if (Singletons.ContainsKey(instanceType))
                return (T)Singletons[instanceType];

            if (Factories.ContainsKey(instanceType))
                return (T)Factories[instanceType].Invoke();

            if (AsyncFactories.ContainsKey(instanceType))
            {
                return (T)(await AsyncFactories[instanceType].Invoke());
            }
            return null;
        }

        public T Get<T>() where T : class
        {
            if (!IsInitilized)
                Init();

            Type instanceType = typeof(T);

            if (Singletons.ContainsKey(instanceType))
                return (T)Singletons[instanceType];

            if (Factories.ContainsKey(instanceType))
                return (T)Factories[instanceType].Invoke();

            if (AsyncFactories.ContainsKey(instanceType))
            {
                return (T)AsyncFactories[instanceType].Invoke().Result;
            }
            return null;
        }
    }
}
