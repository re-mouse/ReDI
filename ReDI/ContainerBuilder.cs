namespace ReDI
{
    public class ContainerBuilder
    {
        private readonly ModuleManager _moduleManager = new ModuleManager();

        public void AddModule<TModule>() where TModule : IModule, new()
        {
            _moduleManager.RegisterModule<TModule>();
        }
        
        public void AddModule<TModule>(TModule module) where TModule : IModule
        {
            _moduleManager.RegisterModule(module);
        }

        public Container Build(Container parentContainer = null)
        {
            var typeManager = new TypeManager();
            var modules = _moduleManager.GetRegisteredModules();
            
            foreach (var module in modules)
            {
                module.BindDependencies(typeManager);
            }

            var bindings = typeManager.GetBindings();
            return new Container(bindings, parentContainer);
        }
    }
}