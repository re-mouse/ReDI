using System;
using System.Collections.Generic;

namespace ReDI
{
    public class TypeManager
    {
        private readonly List<BindingInfo> _bindings = new List<BindingInfo>();
        
        public BindingConfigurator AddSingleton<I, T>() where T : class, I
        {
            var binding = new BindingInfo(typeof(T))
            {
                AssociatedInterfaces = { typeof(I) },
                AlwaysNewInstance = false
            };
            
            _bindings.Add(binding);
            
            return new BindingConfigurator(binding);
        }
        
        public BindingConfigurator AddTransient<I, T>() where T : class, I
        {
            var binding = new BindingInfo(typeof(T))
            {
                AssociatedInterfaces = { typeof(I) },
                AlwaysNewInstance = true
            };
            
            _bindings.Add(binding);
            
            return new BindingConfigurator(binding);
        }
        
        public BindingConfigurator AddSingleton<T>() where T : class
        {
            var binding = new BindingInfo(typeof(T))
            {
                AssociatedInterfaces = { typeof(T) },
                AlwaysNewInstance = false
            };
            
            _bindings.Add(binding);
            
            return new BindingConfigurator(binding);
        }
        
        public BindingConfigurator AddTransient<T>() where T : class
        {
            var binding = new BindingInfo(typeof(T))
            {
                AssociatedInterfaces = { typeof(T) },
                AlwaysNewInstance = true
            };
            
            _bindings.Add(binding);
            
            return new BindingConfigurator(binding);
        }
        
        public BindingConfigurator AddSingleton(Type interfaceType, Type instanceType)
        {
            var binding = new BindingInfo(instanceType)
            {
                AssociatedInterfaces = { interfaceType },
                AlwaysNewInstance = false
            };
            
            _bindings.Add(binding);
            
            return new BindingConfigurator(binding);
        }
        
        public BindingConfigurator AddTransient(Type interfaceType, Type instanceType)
        {
            var binding = new BindingInfo(instanceType)
            {
                AssociatedInterfaces = { interfaceType },
                AlwaysNewInstance = true
            };
            
            _bindings.Add(binding);
            
            return new BindingConfigurator(binding);
        }
        
        public BindingConfigurator AddSingleton(Type instanceType)
        {
            var binding = new BindingInfo(instanceType)
            {
                AssociatedInterfaces = { instanceType },
                AlwaysNewInstance = false
            };
            
            _bindings.Add(binding);
            
            return new BindingConfigurator(binding);
        }
        
        public BindingConfigurator AddTransient(Type instanceType)
        {
            var binding = new BindingInfo(instanceType)
            {
                AssociatedInterfaces = { instanceType },
                AlwaysNewInstance = true
            };
            
            _bindings.Add(binding);
            
            return new BindingConfigurator(binding);
        }
        
        internal IEnumerable<BindingInfo> GetBindings() => _bindings.AsReadOnly();
    }
}