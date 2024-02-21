using System;
using System.Collections.Generic;

namespace ReDI
{
    internal class BindingInfo
    {
        public bool AlwaysNewInstance { get; set; }
        public HashSet<Type> AssociatedInterfaces { get; } = new HashSet<Type>();
        public Type BoundType { get; }
        public object? Instance { get; set; }
        public bool CreateOnBuild { get; set; }
        public bool IsDisposable { get; set; }

        public BindingInfo(Type boundType)
        {
            BoundType = boundType; 
        }
    }
    
    public class BindingConfigurator<T>
    {
        private readonly BindingInfo _binding;

        internal BindingConfigurator(BindingInfo binding)
        {
            _binding = binding;
        }

        public BindingConfigurator<T> ImplementingInterfaces()
        {
            _binding.AssociatedInterfaces.UnionWith(_binding.BoundType.GetInterfaces());
            return this;
        }
        
        public BindingConfigurator<T> AsDisposable()
        {
            _binding.IsDisposable = true;
            return this;
        }

        public BindingConfigurator<T> As<TInterface>()
        {
            _binding.AssociatedInterfaces.Add(typeof(TInterface));
            return this;
        }

        public BindingConfigurator<T> FromInstance(T instance)
        {
            _binding.Instance = instance;
            return this;
        }

        public BindingConfigurator<T> CreateOnContainerBuild()
        {
            _binding.CreateOnBuild = true;
            return this;
        }
    }
}