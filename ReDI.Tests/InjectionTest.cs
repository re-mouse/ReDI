using NUnit.Framework;

namespace ReDI.Tests
{
    [TestFixture]
    public class InjectionTest
    {
        [Test]
        public void TestPropertiesInjection()
        {
            var builder = new ContainerBuilder();
            builder.AddModule<ClassInjectionModule>();
            var container = builder.Build();
            
            var service = container.Resolve<ServiceWithInjectProperties>();
            Assert.IsNotNull(service);
            Assert.IsInstanceOf<ServiceWithInjectProperties>(service);
            Assert.IsTrue(service.Validate());
        }
        
        [Test]
        public void TestConstructorInjection()
        {
            var builder = new ContainerBuilder();
            builder.AddModule<ClassInjectionModule>();
            var container = builder.Build();
            
            var service = container.Resolve<ServiceWithInjectConstructor>();
            Assert.IsNotNull(service);
            Assert.IsInstanceOf<ServiceWithInjectConstructor>(service);
            Assert.IsTrue(service.Validate());
        }
        
        [Test]
        public void TestFieldsInjection()
        {
            var builder = new ContainerBuilder();
            builder.AddModule<ClassInjectionModule>();
            var container = builder.Build();
            
            var service = container.Resolve<ServiceWithInjectFields>();
            Assert.IsNotNull(service);
            Assert.IsInstanceOf<ServiceWithInjectFields>(service);
            Assert.IsTrue(service.Validate());
        }
        
        [Test]
        public void TestMethodInjection()
        {
            var builder = new ContainerBuilder();
            builder.AddModule<ClassInjectionModule>();
            var container = builder.Build();
            
            var service = container.Resolve<ServiceWithInjectMethods>();
            Assert.IsNotNull(service);
            Assert.IsInstanceOf<ServiceWithInjectMethods>(service);
            Assert.IsTrue(service.Validate());
        }
    }
}