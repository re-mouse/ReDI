using System;
using System.Collections.Generic;

namespace ReDI
{
    public class TypeManager
    {
        private readonly List<BindingInfo> _bindings = new List<BindingInfo>();
        
        public BindingConfigurator<I> AddSingleton<I, T>() where T : class, I
        {
            var binding = new BindingInfo(typeof(T))
            {
                AssociatedInterfaces = { typeof(I) },
                AlwaysNewInstance = false
            };
            
            _bindings.Add(binding);
            
            return new BindingConfigurator<I>(binding);
        }
        
        public BindingConfigurator<I> AddTransient<I, T>() where T : class, I
        {
            var binding = new BindingInfo(typeof(T))
            {
                AssociatedInterfaces = { typeof(I) },
                AlwaysNewInstance = true
            };
            
            _bindings.Add(binding);
            
            return new BindingConfigurator<I>(binding);
        }
        
        public BindingConfigurator<T> AddSingleton<T>() where T : class
        {
            var binding = new BindingInfo(typeof(T))
            {
                AssociatedInterfaces = { typeof(T) },
                AlwaysNewInstance = false
            };
            
            _bindings.Add(binding);
            
            return new BindingConfigurator<T>(binding);
        }
        
        public BindingConfigurator<T> AddTransient<T>() where T : class
        {
            var binding = new BindingInfo(typeof(T))
            {
                AssociatedInterfaces = { typeof(T) },
                AlwaysNewInstance = true
            };
            
            _bindings.Add(binding);
            
            return new BindingConfigurator<T>(binding);
        }
        
        internal IEnumerable<BindingInfo> GetBindings() => _bindings.AsReadOnly();
    }
}