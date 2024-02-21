using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ReDI
{
    internal class ServiceRegistration
    {
        private readonly ConstructorDelegate _constructor;
        private readonly InjectDelegate _inject;
        
        public bool AlwaysNewInstance { get; }
        public Type ServiceType { get; }
        public bool IsDisposable { get; }

        public bool IsInjected { get; private set; }
        public object? Instance { get; set; }
 
        public ServiceRegistration(Type serviceType, bool alwaysNewInstance, bool isDisposable, object instance)
        {
            ServiceType = serviceType;
            AlwaysNewInstance = alwaysNewInstance;
            IsDisposable = isDisposable;
            Instance = instance;
            IsInjected = false;
            
            _constructor = ServiceReflectionHelper.GetConstructorDelegate(ServiceType);
            _inject = ServiceReflectionHelper.GetInjectingFieldsAndPropertiesDelegate(ServiceType);
            _inject += ServiceReflectionHelper.GetInjectingMethodsDelegate(ServiceType);
        }

        public object Create(Container container)
        {
            var obj = _constructor(container);
            
            if (!AlwaysNewInstance)
            {
                Instance = obj;
            }

            return obj;
        }

        public object Inject(object obj, Container container)
        {
            IsInjected = true;
            
            _inject(obj, container);
            
            return obj;
        }
    }
}