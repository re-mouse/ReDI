using System;
using NUnit.Framework;

namespace ReDI.Tests
{
    [TestFixture]
    public class ExampleTest
    {
        [Test]
        public void Example()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.AddModule<ZuBarModule>();
            var container = containerBuilder.Build();
            var bar = container.Resolve<Bar>();
            Console.WriteLine(bar.foo != null);
        }
        internal class Bar
        {
            [Inject] public IFoo foo;
        }
        public class FooBarModule : Module
        {
            public override void BindDependencies(TypeManager typeBinder)
            {
                var bar = new Bar();
                typeBinder.AddSingleton<IFoo, Foo>().AsDisposable();
                typeBinder.AddTransient<Bar>().ImplementingInterfaces().FromInstance(bar);
            }

            public override void BindModuleDependencies(ModuleManager moduleBinder)
            {
            }
        }

        public class ZuBarModule : Module
        {
            public override void BindDependencies(TypeManager typeBinder)
            {
                typeBinder.AddSingleton<Zu>();
            }

            public override void BindModuleDependencies(ModuleManager moduleBinder)
            {
                moduleBinder.RegisterModule<FooBarModule>();
            }
        }
    }

    public class Zu { }
    public class Foo : IFoo { }
    internal interface IFoo { }
    internal class ZuBar { }
}