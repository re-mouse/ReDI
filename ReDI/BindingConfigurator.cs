using System;
using System.Collections.Generic;

namespace ReDI
{
    internal class BindingInfo
    {
        public bool AlwaysNewInstance { get; set; }
        public HashSet<Type> AssociatedInterfaces { get; } = new HashSet<Type>();
        public Type InstanceType { get; }
        public object? Instance { get; set; }
        public bool CreateOnBuild { get; set; }
        public bool IsDisposable { get; set; }

        public BindingInfo(Type instanceType)
        {
            InstanceType = instanceType; 
        }
    }
    
    public class BindingConfigurator
    {
        private readonly BindingInfo _binding;

        internal BindingConfigurator(BindingInfo binding)
        {
            _binding = binding;
        }

        public BindingConfigurator ImplementingInterfaces()
        {
            _binding.AssociatedInterfaces.UnionWith(_binding.InstanceType.GetInterfaces());
            return this;
        }
        
        public BindingConfigurator AsDisposable()
        {
            _binding.IsDisposable = true;
            return this;
        }

        public BindingConfigurator As<TInterface>()
        {
            _binding.AssociatedInterfaces.Add(typeof(TInterface));
            return this;
        }

        public BindingConfigurator FromInstance(object instance)
        {
            _binding.Instance = instance;
            return this;
        }

        public BindingConfigurator CreateOnContainerBuild()
        {
            _binding.CreateOnBuild = true;
            return this;
        }
    }
}