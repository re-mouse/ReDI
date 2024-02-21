using System;
using System.Collections.Generic;
using System.Linq;

namespace ReDI
{
    public class ModuleManager
    {
        private class ModuleBinding
        {
            public Type type;
            public Module? instance;
            public Module? defaultInstance;
        }
        
        private readonly Dictionary<Type, ModuleBinding> _modules = new Dictionary<Type, ModuleBinding>();

        public void RegisterModule<TModule>() where TModule : Module, new()
        {
            var moduleType = typeof(TModule);
            if (!_modules.ContainsKey(moduleType))
            {
                var module = new TModule();
                var moduleBind = new ModuleBinding() { type = moduleType, defaultInstance = module };
                _modules[moduleType] = moduleBind;
                
                module.BindModuleDependencies(this);
            }
        }
        
        public void RegisterModule<TModule>(TModule module) where TModule : Module
        {
            var moduleType = typeof(TModule);
            
            if (!_modules.ContainsKey(moduleType))
            {
                var moduleBind = new ModuleBinding() { type = moduleType, instance = module };
                _modules[moduleType] = moduleBind;
                
                module.BindModuleDependencies(this);
            }
            else
            {
                var moduleBind = _modules[moduleType];
                if (moduleBind.instance != null)
                    throw new ModuleInstanceAlreadyRegisteredException(module);

                moduleBind.instance = module;
                module.BindModuleDependencies(this);
            }
        }

        internal IEnumerable<Module> GetRegisteredModules() => 
            _modules.Values.Select(bind => bind.instance ?? bind.defaultInstance ?? throw new ModuleNotAssignedException(bind.type));
    }
}