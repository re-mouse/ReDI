namespace ReDI.Tests
{
    public class ABCModule : IModule
    {
        public void BindDependencies(TypeManager typeBinder)
        {
            typeBinder.AddSingleton<ServiceA>();
            typeBinder.AddSingleton<ServiceB>();
            typeBinder.AddSingleton<ServiceC>();
            typeBinder.AddTransient<ServiceABC>();
        }

        public void BindModuleDependencies(ModuleManager moduleBinder)
        {
        
        }
    }
}