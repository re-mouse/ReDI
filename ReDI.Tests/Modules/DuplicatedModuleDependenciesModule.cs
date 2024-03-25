namespace ReDI.Tests
{
    public class DuplicatedModuleDependenciesModule : IModule
    {
        public void BindDependencies(TypeManager typeBinder)
        {
        }

        public void BindModuleDependencies(ModuleManager moduleBinder)
        {
            moduleBinder.RegisterModule<TestModule>();
            moduleBinder.RegisterModule<TestModule>();
        }
    }
}