namespace ReDI.Tests
{
    public class CircularDependencyModule : IModule
    {
        public void BindDependencies(TypeManager typeBinder)
        {
            typeBinder.AddSingleton<CircularServiceA>();
            typeBinder.AddSingleton<CircularServiceB>();
        }

        public void BindModuleDependencies(ModuleManager moduleBinder)
        {
        }
    }
}