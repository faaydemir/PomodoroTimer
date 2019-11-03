using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinHelperLib.Utils
{
    /// <summary>
    /// Persistent Object
    /// </summary>
    /// <typeparam name="ObjectType"></typeparam>
    public class PersistentObject<ObjectType> where ObjectType : new()
    {
        private ObjectType _value;

        private bool _isReaded = false;

        public ObjectType Value
        {
            get
            {
                return Get();
            }
            set
            {
                Set(value);
            }
        }

        /// <summary>
        /// Item Id
        /// </summary>
        private string Id { get; }

        private IPersistanceService PersistanceService { get; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public PersistentObject(IPersistanceService persistanceService)
        {
            Id = Guid.NewGuid().ToString() + typeof(ObjectType).ToString();
            PersistanceService = persistanceService;

            PersistanceService.Set<ObjectType>(Id, Value);
        }

        public PersistentObject(IPersistanceService persistanceService, ObjectType defaultValue) : this(persistanceService)
        {
            Value = defaultValue;
            Set(defaultValue);
        }

        private ObjectType Get()
        {
            if (!_isReaded)
            {
                _value = PersistanceService.Get<ObjectType>(Id);
                _isReaded = true;
            }
            return _value;
        }

        private void Set(ObjectType objectType)
        {
            if (PersistanceService.Set<ObjectType>(Id, Value))
                _value = objectType;
        }
    }
}
