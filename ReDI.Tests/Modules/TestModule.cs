namespace ReDI.Tests
{
    public class TestModule : IModule
    {
        public void BindDependencies(TypeManager typeBinder)
        {
            typeBinder.AddSingleton<IService, Service>().AsDisposable();
        }

        public void BindModuleDependencies(ModuleManager moduleBinder)
        {
        }
    }
}