using NUnit.Framework;
using System;

namespace ReDI.Tests
{
    [TestFixture]
    public class ContainerTests
    {
        [Test]
        public void TestRegisterAndResolve()
        {
            var builder = new ContainerBuilder();
            builder.AddModule<TestModule>();
            var container = builder.Build();
            var service = container.Resolve<IService>();
            Assert.IsNotNull(service);
            Assert.IsInstanceOf<Service>(service);
        }
        
        [Test]
        public void TestCircularDependencyThrowError()
        {
            var builder = new ContainerBuilder();
            builder.AddModule<CircularDependencyModule>();
            var container = builder.Build();
            Assert.Throws<CircularDependencyFoundException>(() => container.Resolve<CircularServiceA>());
        }
        
        [Test]
        public void TestModuleDependencies()
        {
            var builder = new ContainerBuilder();
            builder.AddModule<ModuleWithModuleDependencies>();
            var container = builder.Build();
            var service = container.Resolve<IService>();
            Assert.IsNotNull(service);
            Assert.IsInstanceOf<Service>(service);
        }
        
        [Test]
        public void TestModuleWidthDuplicatedDependencies()
        {
            var builder = new ContainerBuilder();
            builder.AddModule<DuplicatedModuleDependenciesModule>();
            var container = builder.Build();
            var service = container.Resolve<IService>();
            Assert.IsNotNull(service);
            Assert.IsInstanceOf<Service>(service);
        }


        [Test]
        public void TestResolveNotRegisteredReturnsNull()
        {
            var builder = new ContainerBuilder();
            var container = builder.Build();
            var service = container.Resolve<IService>();
            Assert.IsNull(service);
        }

        [Test]
        public void TestContainerDisposeDisposesService()
        {
            var builder = new ContainerBuilder();
            builder.AddModule<TestModule>();
            var container = builder.Build();
            var service = container.Resolve<IService>() as Service;
            Assert.IsFalse(service.IsDisposed);
            container.Dispose();
            Assert.IsTrue(service.IsDisposed);
        }
        
        [Test]
        public void TestModuleDuplicate()
        {
            var builder = new ContainerBuilder();
            builder.AddModule<TestModule>();
            builder.AddModule<TestModule>();
            var container = builder.Build();
            var service = container.Resolve<IService>() as Service;
            var service2 = container.Resolve<IService>() as Service;
            Assert.IsFalse(service.IsDisposed);
            container.Dispose();
            Assert.IsTrue(service.IsDisposed);
            
            Assert.AreEqual(service, service2);
        }
        
        [Test]
        public void TestModuleWithDuplicateType()
        {
            var builder = new ContainerBuilder();
            builder.AddModule<DuplicatedTypesModule>();
            var container = builder.Build();
            var service = container.Resolve<IService>() as Service;
            Assert.IsFalse(service.IsDisposed);
            container.Dispose();
            Assert.IsTrue(service.IsDisposed);
        }

        [Test]
        public void TestDoubleDisposeThrowsException()
        {
            var builder = new ContainerBuilder();
            var container = builder.Build();
            container.Dispose();
            Assert.Throws<ObjectDisposedException>(() => container.Dispose());
        }
    }
}