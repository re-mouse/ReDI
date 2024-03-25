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
    }
    
    [Test]
    public void TestGenericResolvingWithDependency()
    {
        var builder = new ContainerBuilder();
        builder.AddModule<TestGenericModule>();
        var container = builder.Build();
        var service = container.Resolve<TestGenericDependency<ServiceABC>>();
        Assert.IsNotNull(service);
        Assert.IsInstanceOf<TestGenericDependency<ServiceABC>>(service);
        
        Assert.IsNotNull(service._dependency);
        Assert.IsInstanceOf<ServiceABC>(service._dependency);
    }
    
    [Test]
    public void TestGenericResolvingWithMethodInjection()
    {
        var builder = new ContainerBuilder();
        builder.AddModule<TestGenericModule>();
        var container = builder.Build();
        var service = container.Resolve<TestGenericMethodInjection<ServiceABC>>();
        Assert.IsNotNull(service);
        Assert.IsInstanceOf<TestGenericMethodInjection<ServiceABC>>(service);
        
        Assert.IsNotNull(service._dependency);
        Assert.IsInstanceOf<ServiceABC>(service._dependency);
    }
}

public class TestGenericMethodInjection<T>
{
    public T _dependency;

    [Inject]
    public void Inject(T dependency)
    {
        _dependency = dependency;
    }
}

public class TestGeneric<T>
{
}

public class TestGenericDependency<T>
{
    public readonly T _dependency;

    public TestGenericDependency(T dependency)
    {
        _dependency = dependency; 
    }
}

public class TestGenericModule : Module
{
    public override void BindDependencies(TypeManager typeBinder)
    {
        typeBinder.AddSingleton(typeof(TestGenericDependency<>));
        typeBinder.AddSingleton(typeof(TestGeneric<>));
        typeBinder.AddSingleton(typeof(TestGenericMethodInjection<>));
    }

    public override void BindModuleDependencies(ModuleManager moduleBinder)
    {
        moduleBinder.RegisterModule<ABCModule>();
    }
}