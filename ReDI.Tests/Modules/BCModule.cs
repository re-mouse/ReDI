namespace ReDI.Tests
{
    public class BCModule : IModule
    {
        public void BindDependencies(TypeManager typeBinder)
        {
            typeBinder.AddSingleton<ServiceBC>();
        }

        public void BindModuleDependencies(ModuleManager moduleBinder)
        {
            moduleBinder.RegisterModule<ABCModule>();
        }
    }
}