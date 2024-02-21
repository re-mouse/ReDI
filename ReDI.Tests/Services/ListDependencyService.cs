namespace ReDI.Tests;

public class ListDependencyService : IValidateInjected
{
    [Inject] public List<ITwoServicesDependency> dependencies { get; set; }

    public bool Validate()
    {
        return dependencies.Count == 2;
    }
}

public class TwoServicesDependencyModule : Module
{
    public override void BindDependencies(TypeManager typeBinder)
    {
        typeBinder.AddSingleton<ITwoServicesDependency, ServiceAa>();
        typeBinder.AddSingleton<ITwoServicesDependency, ServiceBb>();
        typeBinder.AddSingleton<ListDependencyService>();
    }

    public override void BindModuleDependencies(ModuleManager moduleBinder)
    {
        
    }
}

public class ServiceAa : ITwoServicesDependency
{
    
}

public class ServiceBb : ITwoServicesDependency
{
    
}

public interface ITwoServicesDependency
{
}