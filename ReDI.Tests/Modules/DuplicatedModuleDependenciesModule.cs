namespace ReDI.Tests
{
    public class DuplicatedModuleDependenciesModule : Module
    {
        public override void BindDependencies(TypeManager typeBinder)
        {
                
        }

        public override void BindModuleDependencies(ModuleManager moduleBinder)
        {
            moduleBinder.RegisterModule<TestModule>();
            moduleBinder.RegisterModule<TestModule>();
        }
    }
}