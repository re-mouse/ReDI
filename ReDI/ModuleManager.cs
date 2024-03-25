using System;
using System.Collections.Generic;
using System.Linq;

namespace ReDI
{
    public class ModuleManager
    {
        private class ModuleBinding
        {
            public Type Type { get; }
            public IModule? instance;
            public IModule? defaultInstance;

            public ModuleBinding(Type type)
            {
                Type = type;
            }
        }
        
        private readonly Dictionary<Type, ModuleBinding> _modules = new Dictionary<Type, ModuleBinding>();

        public void RegisterModule<TModule>() where TModule : IModule, new()
        {
            var moduleType = typeof(TModule);
            if (!_modules.ContainsKey(moduleType))
            {
                var module = new TModule();
                var moduleBind = new ModuleBinding(moduleType) { defaultInstance = module };
                _modules[moduleType] = moduleBind;
                
                module.BindModuleDependencies(this);
            }
        }
        
        public void RegisterModule<TModule>(TModule module) where TModule : IModule
        {
            var moduleType = typeof(TModule);
            
            if (!_modules.ContainsKey(moduleType))
            {
                var moduleBind = new ModuleBinding(moduleType) { instance = module };
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

        internal IEnumerable<IModule> GetRegisteredModules() => 
            _modules.Values.Select(bind => bind.instance ?? bind.defaultInstance ?? throw new ModuleNotAssignedException(bind.Type));
    }
}