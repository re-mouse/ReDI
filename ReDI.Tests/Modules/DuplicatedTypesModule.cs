using ReDI;

namespace ReDI.Tests
{
    public class DuplicatedTypesModule : Module
    {
        public override void BindDependencies(TypeManager typeBinder)
        {
            typeBinder.AddSingleton<IService, Service>().AsDisposable();
            typeBinder.AddTransient<IService, Service>().AsDisposable();
        }

        public override void BindModuleDependencies(ModuleManager moduleBinder)
        {
                
        }
    }
}