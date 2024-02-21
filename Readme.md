ReDI: Reusable Dependency Injection (ReDI) Framework
Purpose
The ReDI framework aims to simplify dependency management within your projects. By encapsulating dependencies in modules, it allows you to create reusable libraries and easily set up test environments. With just a few lines of code, you can manage complex dependency graphs.

Quickstart
Install ReDI via NuGet:
Add the StackExchange.Redis package to your project using NuGet:
Install-Package StackExchange.Redis

Create Modules:
Define your modules by creating classes that inherit from Module. A common naming convention is {LibraryName}Module.
In each module, specify the types and their dependencies using the same syntax as Microsoft’s Dependency Injection (DI) framework.
Example Usage
Suppose we have the following scenario:

Modules:
FooBarModule: Contains types related to Foo and Bar.
ZuBarModule: Contains types related to Zu and Bar.
Dependencies:
Bar depends on IFoo.
ZuBarModule depends on FooBarModule.
Here’s how you can set it up:

```cs
using ReDI;

class Program
{
    static void Main()
    {
        var containerBuilder = new ContainerBuilder();
        containerBuilder.AddModule<ZuBarModule>(); // Register ZuBarModule
        var container = containerBuilder.Build();

        var zu = container.Resolve<Zu>();
        var bar = container.Resolve<Bar>();
        // Check if IFoo is injected into Bar and zu is resolved
        Console.WriteLine(zu != null); 
        Console.WriteLine(bar.foo != null); 
    }
}

public interface IFoo { }
public class Foo : IFoo { }
public class Zu { }

public class Bar
{
    [Inject] public IFoo foo; // Inject IFoo dependency
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
        // No additional module dependencies for FooBarModule
    }
}

public class ZuBarModule : Module
{
    public override void BindDependencies(TypeManager typeBinder)
    {
        typeBinder.AddSingleton<Zu>(); // Register Zu type
    }

    public override void BindModuleDependencies(ModuleManager moduleBinder)
    {
        moduleBinder.RegisterModule<FooBarModule>(); // Register FooBarModule dependency
    }
}
```

## Contributing
Feel free to contribute to the ReDI framework by submitting pull requests or reporting issues on the GitHub repository.

## License
This project is licensed under the MIT License.

For more detailed information, visit the documentation (in-progress).

Happy coding! 🚀