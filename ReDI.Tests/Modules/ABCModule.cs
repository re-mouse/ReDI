namespace ReDI.Tests
{
    public class ABCModule : Module
    {
        public override void BindDependencies(TypeManager typeBinder)
        {
            typeBinder.AddSingleton<ServiceA>();
            typeBinder.AddSingleton<ServiceB>();
            typeBinder.AddSingleton<ServiceC>();
            typeBinder.AddTransient<ServiceABC>();
        }

        public override void BindModuleDependencies(ModuleManager moduleBinder)
        {
        
        }
    }
}