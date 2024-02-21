namespace ReDI.Tests
{
    public class CircularDependencyModule : Module
    {
        public override void BindDependencies(TypeManager typeBinder)
    {
        typeBinder.AddSingleton<CircularServiceA>();
        typeBinder.AddSingleton<CircularServiceB>();
    }

        public override void BindModuleDependencies(ModuleManager moduleBinder)
    {
        
    }
    }
}