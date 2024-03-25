namespace ReDI.Tests
{
    public class ABModule : IModule
    {
        public void BindDependencies(TypeManager typeBinder)
        {
            typeBinder.AddSingleton<ServiceAB>();
        }

        public void BindModuleDependencies(ModuleManager moduleBinder)
        {
            moduleBinder.RegisterModule<ABCModule>();
        }
    }
}