using System;
using System.Collections.Generic;

namespace ReDI
{
    internal class ServiceRegistration
    {
        private readonly ConstructorDelegate _constructor;
        private readonly InjectDelegate _inject;
        
        public bool AlwaysNewInstance { get; }
        public Type ServiceType { get; }
        public bool IsDisposable { get; }
        public bool IsGeneric { get; }
        
        private Dictionary<Type, ConstructorDelegate> GenericConstructorsByType { get; } = new Dictionary<Type, ConstructorDelegate>();
        private Dictionary<Type, InjectDelegate> GenericInjectsByType { get; } = new Dictionary<Type, InjectDelegate>();

        public bool IsInjected { get; private set; }
        public object? Instance { get; set; }
        
        public ServiceRegistration(Type serviceType, bool alwaysNewInstance, bool isDisposable, object instance)
        {
            ServiceType = serviceType;
            AlwaysNewInstance = alwaysNewInstance;
            IsDisposable = isDisposable;
            Instance = instance;
            IsInjected = false;

            if (serviceType.IsGenericType)
            {
                IsGeneric = true;
                return;
            }

            if (Instance == default || AlwaysNewInstance)
            {
                _constructor = ServiceReflectionHelper.GetConstructorDelegate(ServiceType);
                _inject = ServiceReflectionHelper.GetInjectingFieldsAndPropertiesDelegate(ServiceType);
                _inject += ServiceReflectionHelper.GetInjectingMethodsDelegate(ServiceType);
            }
        }

        public object Create(Container container, Type concreteType)
        {
            if (IsGeneric)
            {
                return CreateForGeneric(container, concreteType);
            }
            
            var obj = _constructor(container);
            
            if (!AlwaysNewInstance)
            {
                Instance = obj;
            }

            return obj;
        }

        private object CreateForGeneric(Container container, Type concreteType)
        {
            if (concreteType == null)
            {
                return null;
            }
            
            ConstructorDelegate constructor;
            if (GenericConstructorsByType.ContainsKey(concreteType))
            {
                constructor = GenericConstructorsByType[concreteType];
            }
            else
            {
                constructor = ServiceReflectionHelper.GetConstructorDelegate(concreteType);
                GenericConstructorsByType[concreteType] = constructor;
            }
            
            var concreteObj = constructor(container);
                
            return concreteObj;
        }

        public object Inject(object obj, Container container, Type concreteType)
        {
            if (IsGeneric)
            {
                InjectForGeneric(obj, container, concreteType);
                return obj;
            }
            
            IsInjected = true;
            
            _inject(obj, container);
            
            return obj;
        }

        private void InjectForGeneric(object obj, Container container, Type concreteType)
        {
            if (concreteType == null)
            {
                return;
            }
            
            InjectDelegate inject;
            if (GenericInjectsByType.ContainsKey(concreteType))
            {
                inject = GenericInjectsByType[concreteType];
            }
            else
            {
                inject = ServiceReflectionHelper.GetInjectingFieldsAndPropertiesDelegate(concreteType);
                inject += ServiceReflectionHelper.GetInjectingMethodsDelegate(concreteType);
                GenericInjectsByType[concreteType] = inject;
            }
            
            inject(obj, container);
            IsInjected = true;
        }
    }
}