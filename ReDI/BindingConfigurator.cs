using System;
using System.Collections.Generic;

namespace ReDI
{
    internal class BindingInfo
    {
        public bool alwaysNewInstance;
        public readonly HashSet<Type> associatedInterfaces = new HashSet<Type>();
        public Type boundType;
        public object instance;
        public bool createOnBuild;
        public bool isDisposable;
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
            _binding.associatedInterfaces.UnionWith(_binding.boundType.GetInterfaces());
            return this;
        }
        
        public BindingConfigurator<T> AsDisposable()
        {
            
            _binding.isDisposable = true;
            return this;
        }

        public BindingConfigurator<T> As<TInterface>()
        {
            _binding.associatedInterfaces.Add(typeof(TInterface));
            return this;
        }

        public BindingConfigurator<T> FromInstance(T instance)
        {
            _binding.instance = instance;
            return this;
        }

        public BindingConfigurator<T> CreateOnContainerBuild()
        {
            _binding.createOnBuild = true;
            return this;
        }
    }
}