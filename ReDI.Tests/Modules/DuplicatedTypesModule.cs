namespace ReDI.Tests
{
    public class DuplicatedTypesModule : IModule
    {
        public void BindDependencies(TypeManager typeBinder)
        {
            typeBinder.AddSingleton<IService, Service>().AsDisposable();
            typeBinder.AddTransient<IService, Service>().AsDisposable();
        }

        public void BindModuleDependencies(ModuleManager moduleBinder)
        {
        }
    }
}