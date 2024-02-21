using NUnit.Framework;

namespace ReDI.Tests
{
    [TestFixture]
    public class ListResolveTest
    {
        [Test]
        public void TestListResolve()
        {
            var builder = new ContainerBuilder();
            builder.AddModule<ClassInjectionModule>();
            var container = builder.Build();
            
            var serviceList = container.Resolve<List<IValidateInjected>>();
            Assert.IsNotNull(serviceList);
            Assert.IsInstanceOf<List<IValidateInjected>>(serviceList);
            Assert.IsTrue(serviceList.Count == 4);
            foreach (var service in serviceList)
                Assert.IsTrue(service.Validate());
        }
        
        [Test]
        public void TestListDependencyResolve()
        {
            var builder = new ContainerBuilder();
            builder.AddModule<TwoServicesDependencyModule>();
            var container = builder.Build();
            
            var service = container.Resolve<ListDependencyService>();
            Assert.IsNotNull(service);
            Assert.IsInstanceOf<ListDependencyService>(service);
            Assert.IsTrue(service.Validate());
        }
    }
}