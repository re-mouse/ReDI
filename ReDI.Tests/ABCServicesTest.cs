using NUnit.Framework;

namespace ReDI.Tests
{
    [TestFixture]
    public class ABCServicesTest
    {
        [Test]
        public void TestRegisterAndResolve()
        {
            var builder = new ContainerBuilder();
            builder.AddModule<ABCModule>();
            var container = builder.Build();
            var serviceA = container.Resolve<ServiceA>();
            Assert.IsNotNull(serviceA);
            Assert.IsInstanceOf<ServiceA>(serviceA);
            
            var serviceABC = container.Resolve<ServiceABC>();
            Assert.IsNotNull(serviceABC);
            Assert.IsInstanceOf<ServiceABC>(serviceABC);
            Assert.IsTrue(serviceABC.Validate());
        }
        
        [Test]
        public void TestRegisterBCAndResolve()
        {
            var builder = new ContainerBuilder();
            builder.AddModule<BCModule>();
            var container = builder.Build();
            
            var serviceBC = container.Resolve<ServiceBC>();
            Assert.IsNotNull(serviceBC);
            Assert.IsInstanceOf<ServiceBC>(serviceBC);
            Assert.IsTrue(serviceBC.Validate());
            
            var serviceABC = container.Resolve<ServiceABC>();
            Assert.IsNotNull(serviceABC);
            Assert.IsInstanceOf<ServiceABC>(serviceABC);
            Assert.IsTrue(serviceABC.Validate());
        }
        
        [Test]
        public void TestRegisterABAndResolve()
        {
            var builder = new ContainerBuilder();
            builder.AddModule<ABModule>();
            var container = builder.Build();
            
            var serviceAB = container.Resolve<ServiceAB>();
            Assert.IsNotNull(serviceAB);
            Assert.IsInstanceOf<ServiceAB>(serviceAB);
            
            var serviceABC = container.Resolve<ServiceABC>();
            Assert.IsNotNull(serviceABC);
            Assert.IsInstanceOf<ServiceABC>(serviceABC);
            Assert.IsTrue(serviceABC.Validate());
        }
    }
}