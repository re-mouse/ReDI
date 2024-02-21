using ReDI;

namespace ReDI.Tests
{
    public class TestModule : Module
    {
        public override void BindDependencies(TypeManager typeBinder)
        {
            typeBinder.AddSingleton<IService, Service>().AsDisposable();
        }

        public override void BindModuleDependencies(ModuleManager moduleBinder)
        {
        }
    }
}