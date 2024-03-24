using NUnit.Framework;

namespace ReDI.Tests;

[TestFixture]
public class GenericTests
{
    [Test]
    public void TestGenericResolving()
    {
        var builder = new ContainerBuilder();
        builder.AddModule<TestGenericModule>();
        var container = builder.Build();
        var service = container.Resolve<TestGeneric<ServiceABC>>();
        Assert.IsNotNull(service);
        Assert.IsInstanceOf<TestGeneric<ServiceABC>>(service);
        
        Assert.IsNotNull(service._dependency);
        Assert.IsInstanceOf<ServiceABC>(service._dependency);
    }
}

public class TestGeneric<T>
{
    public readonly T _dependency;

    public TestGeneric(T dependency)
    {
        _dependency = dependency; 
    }
}

public class TestGenericModule : Module
{
    public override void BindDependencies(TypeManager typeBinder)
    {
        typeBinder.AddSingleton(typeof(TestGeneric<>));
    }

    public override void BindModuleDependencies(ModuleManager moduleBinder)
    {
        moduleBinder.RegisterModule<ABCModule>();
    }
}