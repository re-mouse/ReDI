# ReDI: Reusable Dependency Injection
![alt text](https://raw.githubusercontent.com/re-mouse/.blobs/0e42219d73f3d024a11d6475797543cad7c00db7/ReDI_icon.png)
[![MIT license](https://img.shields.io/badge/License-MIT-blue.svg)](https://lbesson.mit-license.org/)
[![NuGet stable version](https://badgen.net/nuget/v/ReDI)](https://www.nuget.org/packages/ReDI)

## Purpose
The ReDI framework aims to simplify dependency management within your projects. By encapsulating dependencies in modules, it allows you to create reusable libraries and easily set up test environments.

## Compatability
It targets the .net 2.1 standard and can be used in both .NET and Unity.

## Install
Add the ReDI package to your project using NuGet:
```
Install-Package ReDI
```

## Basic Usage

Let's define Foo class, that use IGreeter to print greeting, and concrete greeter.
```cs
public class Foo 
{
    private readonly IGreeter _greeter;

    public Foo(IGreeter greeter) { _greeter = greeter; }
    
    public void PrintGreeting()
    {
        Console.WriteLine(_greeter.GetGreeting());
    }
}

public interface IGreeter
{
    public string GetGreeting();
}

public class HelloWorldGreeter : IGreeter
{
    public string GetGreeting() => "Hello World";
}
```
Now our goal is to make them work together, so let's define module where we will pack them
```cs
public class FooModule : IModule
{
    public void BindDependencies(TypeManager typeBinder)
    {
        typeBinder.AddSingleton<Foo>();
        typeBinder.AddSingleton<IGreeter, HelloWorldGreeter>();
    }

    public void BindModuleDependencies(ModuleManager moduleBinder)
    {
    }
}
```
Great, now we know basics, we can create some module that we will reuse.

Let's define weather module, that provides weather service and use it later
```cs

public class WeatherModule : IModule
{
    public void BindDependencies(TypeManager typeBinder)
    {
        typeBinder.AddTransient<WeatherService>();
    }

    public void BindModuleDependencies(ModuleManager moduleBinder)
    {
    }
}

public class WeatherService
{
    public int GetWeatherTemperature() => 100; //it's hot
}
```

Now let's define class that use that service, and want to inject it.

Injecting attribute can be applied to properties, fields and methods. 
Calling order: Fields -> Properties -> Methods
```cs
public class Bar
{
    [Inject] private WeatherService _weatherService;

    public void PrintWeather()
    {
        Console.WriteLine($"Temperature is {_weatherService.GetWeatherTemperature()}");
    }
}

public class BarModule : IModule
{
    public void BindDependencies(TypeManager typeBinder)
    {
        var bar = new Bar();
        typeBinder.AddTransient<Bar>().ImplementingInterfaces().FromInstance(bar);
    }

    public void BindModuleDependencies(ModuleManager moduleBinder)
    {
        moduleBinder.RegisterModule<WeatherModule>();
    }
}

```

Now let's use everything in our program
```cs
var containerBuilder = new ContainerBuilder();
containerBuilder.AddModule<FooModule>();
containerBuilder.AddModule<BarModule>();
var container = containerBuilder.Build();

var foo = container.Resolve<Foo>();
var bar = container.Resolve<Bar>();

foo.PrintGreeting();
bar.PrintWeather();
```

Encapsulating dependencies making testing is much easier, and it allows you to write reusable libraries.

## Contributing
Feel free to contribute to the ReDI framework by submitting pull requests or reporting issues on the GitHub repository.

## License
This project is licensed under the MIT License.

For more detailed information, visit the documentation (in-progress).

Happy coding! 🚀
