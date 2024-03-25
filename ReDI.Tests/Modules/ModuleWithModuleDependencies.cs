namespace ReDI.Tests
{
    public class ModuleWithModuleDependencies : Module
    {
        public override void BindDependencies(TypeManager typeBinder)
        {
        }

        public override void BindModuleDependencies(ModuleManager moduleBinder)
        {
            moduleBinder.RegisterModule<TestModule>();
        }
    }
}