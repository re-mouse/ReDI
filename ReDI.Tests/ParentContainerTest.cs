using NUnit.Framework;

namespace ReDI.Tests;

[TestFixture]
public class ParentContainerTest
{
    [Test]
    public void TestParentContainerBuilding()
    {
        var containerBuilder = new ContainerBuilder();
        containerBuilder.AddModule<ABModule>();
        var container = containerBuilder.Build();
        Assert.IsNotNull(container);
        var containerWithParent = containerBuilder.Build(container);
        Assert.IsNotNull(containerWithParent);
        Assert.That(containerWithParent, Is.Not.SameAs(container));
    }
    
    [Test]
    public void TestParentContainerResolves()
    {
        var containerBuilder = new ContainerBuilder();
        containerBuilder.AddModule<ABModule>();
        var container = containerBuilder.Build();
        Assert.IsNotNull(container);
        var emptyContainerBuilder = new ContainerBuilder();
        var containerWithParent = emptyContainerBuilder.Build(container);
        Assert.IsNotNull(containerWithParent);
        Assert.That(containerWithParent, Is.Not.SameAs(container));

        var abc = container.Resolve<ServiceABC>();
        Assert.IsNotNull(abc);
        Assert.IsInstanceOf<ServiceABC>(abc);
        
        var abcFromParent = containerWithParent.Resolve<ServiceABC>();
        Assert.IsNotNull(abcFromParent);
        Assert.IsInstanceOf<ServiceABC>(abcFromParent);
    }
}