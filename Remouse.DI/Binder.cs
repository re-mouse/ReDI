using System;
using System.Collections.Generic;

namespace Remouse.DI
{
    public class ModuleManager
    {
        private readonly Dictionary<Type, Module> _modules = new Dictionary<Type, Module>();

        public void RegisterModule<TModule>() where TModule : Module, new()
        {
            var moduleType = typeof(TModule);
            if (!_modules.ContainsKey(moduleType))
            {
                var module = new TModule();
                module.BindModuleDependencies(this);
                _modules[moduleType] = module;
            }
        }
        
        public void RegisterModule<TModule>(TModule module) where TModule : Module
        {
            var moduleType = typeof(TModule);
            if (!_modules.ContainsKey(moduleType))
            {
                module.BindModuleDependencies(this);
                _modules[moduleType] = module;
            }
        }

        internal IEnumerable<Module> GetRegisteredModules() => _modules.Values;
    }

    public class TypeManager
    {
        private readonly List<BindingInfo> _bindings = new List<BindingInfo>();
        
        public BindingConfigurator<I> AddSingleton<I, T>() where T : class, I
        {
            var binding = new BindingInfo
            {
                boundType = typeof(T),
                associatedInterfaces = { typeof(I) },
                alwaysNewInstance = false
            };
            
            _bindings.Add(binding);
            
            return new BindingConfigurator<I>(binding);
        }
        
        public BindingConfigurator<I> AddTransient<I, T>() where T : class, I
        {
            var binding = new BindingInfo
            {
                boundType = typeof(T),
                associatedInterfaces = { typeof(I) },
                alwaysNewInstance = true
            };
            
            _bindings.Add(binding);
            
            return new BindingConfigurator<I>(binding);
        }
        
        public BindingConfigurator<T> AddSingleton<T>() where T : class
        {
            var binding = new BindingInfo
            {
                boundType = typeof(T),
                associatedInterfaces = { typeof(T) },
                alwaysNewInstance = false
            };
            
            _bindings.Add(binding);
            
            return new BindingConfigurator<T>(binding);
        }
        
        public BindingConfigurator<T> AddTransient<T>() where T : class
        {
            var binding = new BindingInfo
            {
                boundType = typeof(T),
                associatedInterfaces = { typeof(T) },
                alwaysNewInstance = true
            };
            
            _bindings.Add(binding);
            
            return new BindingConfigurator<T>(binding);
        }
        
        internal IEnumerable<BindingInfo> GetBindings() => _bindings.AsReadOnly();
    }
}