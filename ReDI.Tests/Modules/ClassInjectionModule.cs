namespace ReDI.Tests
{
    public class ClassInjectionModule : IModule
    {
        public void BindDependencies(TypeManager typeBinder)
        {
            typeBinder.AddSingleton<ServiceWithInjectProperties>().ImplementingInterfaces();
            typeBinder.AddSingleton<ServiceWithInjectMethods>().ImplementingInterfaces();
            typeBinder.AddSingleton<ServiceWithInjectConstructor>().ImplementingInterfaces();
            typeBinder.AddSingleton<ServiceWithInjectFields>().ImplementingInterfaces();
        }

        public void BindModuleDependencies(ModuleManager moduleBinder)
        {
            
        }
    }
}