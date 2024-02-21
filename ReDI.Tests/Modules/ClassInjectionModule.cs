namespace ReDI.Tests
{
    public class ClassInjectionModule : Module
    {
        public override void BindDependencies(TypeManager typeBinder)
        {
            typeBinder.AddSingleton<ServiceWithInjectProperties>().ImplementingInterfaces();
            typeBinder.AddSingleton<ServiceWithInjectMethods>().ImplementingInterfaces();
            typeBinder.AddSingleton<ServiceWithInjectConstructor>().ImplementingInterfaces();
            typeBinder.AddSingleton<ServiceWithInjectFields>().ImplementingInterfaces();
        }

        public override void BindModuleDependencies(ModuleManager moduleBinder)
        {
            
        }
    }
}