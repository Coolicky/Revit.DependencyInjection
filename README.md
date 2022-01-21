
# Revit Dependency Injection

This repository borrows heavily from [Onbox framework](https://github.com/engthiago/Onboxframework) to create dependency inject with Revit Plugins. Instead of using custom container created by Onbox Team this is using Unity Container.

I've removed some libraries and utilities (like their mvc like packages). Packed all of it into a single dll for simplicity. 

## Installation 
The package can be downloaded from [Nuget](https://www.nuget.org/packages/Revit.DependencyInjection.Unity/)

### Application
Make sure that the application class that would normally inherit from IExternalApplication inherits from Revit App

```c#
[ContainerProvider("{{GUID}}")] // This attribute is crucial
public class App : RevitApp
{
	public override void OnCreateRibbon(IRibbonManager  ribbonManager)
	{
		// Here you can create Ribbon tabs, panels and buttons
	}
	public override Result OnStartup(IUnityContainer container,  UIControlledApplication application)
	{
		// Add Container registrations here
    
		return Result.Succeeded;
	}
	public override Result OnShutdown(IUnityContainer container, UIControlledApplication application)
	{
		// The container will be disposed automatically
    
		return Result.Succeeded;
	}
}
```
### Command
Make sure that the command class that would normally inherit from IExternalCommand inherits RevitAppCommand\<App\> where "App" refers to the application class
```c#
[Transaction(TransactionMode.Manual)] // Important to every command
public class SampleCommand : RevitAppCommand<App>
{
	public override Result Execute(IUnityContainer container, ExternalCommandData commandData, 
                                 ref string message, ElementSet elements)
	{
		// Your logic here
	}
}
```
## Container Registration
Registration follows standard [Unity procedures](http://unitycontainer.org/tutorials/registration/registration.html)  below just the basics from their documentation.

### Instance Registration
```c#
var instance = new Service();
container.RegisterInstance(instance);

// Named Registration
container.RegisterInstance("UniqueName", instance);

// Mapped to type
container.RegisterInstance<IService>(instance);
container.RegisterInstance<IService>("UniqueName", instance);
or
container.RegisterInstance(typeof(IService), instance)
container.RegisterInstance(typeof(IService), "UniqueName", instance)
```
### Lifetime
```c#
// Per container (Default)
container.RegisterInstance("UniqueName", instance, InstanceLifetime.PerContainer);
container.RegisterInstance<IService>("UniqueName", instance, InstanceLifetime.PerContainer);

// Singleton
container.RegisterInstance("UniqueName", instance, InstanceLifetime.Singleton);
container.RegisterInstance<IService>("UniqueName", instance, InstanceLifetime.Singleton);

// External (Unity will not control the instance lifetime)
container.RegisterInstance<IService>("Some Name", instance, InstanceLifetime.External);
```

### Factory Registration
```c#
container.RegisterFactory<IService>(f => new Service());
```

### Type Registration
```c#
// Standard
container.RegisterType<IService, Service>();

// Named
container.RegisterType<IService, MailService>("Email");

// Lifetime
container.RegisterType<IService, MailService>("Email", TypeLifetime.Singleton);
```

You can either register required services directly in the OnStartup method

Or create extension methods 

```c#
public static class SamplePipeline
{
	public static IUnityContainer RegisterSampleServices(this IUnityContainer container)
	{
		container.RegisterType<IService, Service>();
		return container;
	}
}

public class App : RevitApp
{
	[...]
	public override Result OnStartup(IUnityContainer container,  UIControlledApplication application)
	{
		// Direct
		container.RegisterType<IService, Service>();
		// Extension
		container.RegisterSampleServices();
		...
		return Result.Succeeded;
	}
	[...]
}
```

### Resolving
For more detailed explanation refer to [Unity Documentation](http://unitycontainer.org/tutorials/registration/Type/Constructor/param_none.html)

Generally speaking you can get instance of a class/service by
```c#
var instance = Container.Resolve<Service>();
```
Unity framework should automatically resolve services required by a constructor of a class 
-   `static`  constructors are not supported
-   `private`  and  `protected`  constructors are not accessible
-   Constructors with  `ref`  and  `out`  parameters are not supported

Constructors are selected as below:
-   If present, use registered  [Injection Constructor](http://unitycontainer.org/api/Unity.Injection.InjectionConstructor.html)
-   If present, annotated with an attribute
-   Automatically select constructor
    -   Get all accessible constructors
    -   Process constructors in ascending order from most complex to the default
        -   Filter out  [restricted](http://unitycontainer.org/tutorials/registration/Type/Constructor/constructor.html#restrictions)  constructors
        -   Loop through parameters and check if
            -   Is primitive
                -   Is registered with the container
                -   Has  _default_  value
            -   Is resolvable type
            -   Is registered with container
        -   Select the first constructor the container can create

Sample
```c#
// Default
public class Service
{
	private  readonly  IDependency _dependency;
	
	public Service(IDependency dependency)
	{
		_dependency = dependency;
	}
}

// Constructor Annotation
public class Service
{
	private  readonly  IDependency _dependency;
	
	[InjectionConstructor]
	public Service(IDependency dependency)
	{
		_dependency = dependency;
	}
	
	public Service()
	{
	}
}

// Or Registering a specific constructor
Container.RegisterType<Service>(Invoke.Constructor());
```

