namespace ReDI.Tests
{
    public class ModuleWithModuleDependencies : IModule
    {
        public void BindDependencies(TypeManager typeBinder)
        {
        }

        public void BindModuleDependencies(ModuleManager moduleBinder)
        {
            moduleBinder.RegisterModule<TestModule>();
        }
    }
}