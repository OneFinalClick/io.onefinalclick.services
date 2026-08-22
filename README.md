Installation: `"io.finalclick.services": "https://github.com/FinalClick/io.finalclick.services.git?path=/Assets/Package",`

# FinalClick – Services
**Package:** `io.finalclick.services`

A simple lightweight **service locator** and **dependency injection** system for Unity.  
Use it to replace hard-referenced singletons with decoupled, testable services.

### Example
Instead of this:
```csharp
GameManager gameManager = GameManager.Instance;
```

Write this:
```csharp
GameManager gameManager = ApplicationServices.Get<GameManager>();
```

or this:
```csharp
GameManager gameManager = gameObject.GetService<GameManager>();
```

or, within other services, auto injection like this:

```csharp
[InjectService]
GameManager _gamemanager;
```

---

## Overview

`FinalClick.Services` allows you to register and resolve services both at the Project/Application scope or at an Individual Scene scope. Services can be pure C# classes or MonoBehaviours if you require references to assets or scene objects.

---

## Features

- 🔍 **Automatic Service Discovery** via `ApplicationService` and `MonoBehaviourService` attributes.
- 🧩 ***Automatic Dependency Injection** via the `InjectService` property attribute.
- 💡 **Application Scoped Services** via `ApplicationServices`.
- 🎬 **Scene Scoped Services** via `SceneServices`.
- ⚙️ **Lifecycle Hooks** via the `IService` interface. (`OnServiceStart`, `OnServiceUpdate`, `OnServiceStop`)
- 🚀 **Disable Domain Reload Support**: Automaticly start and stops services during scene load and unload, and when entering or exiting Play Mode.

> *Automatic Dependency Injection can only be used within service
---

## 🔍 Getting Services

### Inside a Service

```
[InjectService] 
            │
            └─ SceneServices.Get<T>(scene)
                │
                └─ ApplicationServices.Get<T>()
```

Within a Service you can use the `[InjectService]` attribute on a property or field.

```csharp
[MonoBehaviourService]
public class ExampleComponent : MonoBehavour
{
    [InjectService] 
    private IMyService MyService { get; } = null;
    
    // ...
}
```

The service will be set before the services start

> Note, the `InjectService` attribute can only be used if the **MonoBehaviour** or csharp **class** is a registered service.

### Outside a Service

> Note, there are corresponding `TryGet` functions for the following `Get` functions

There are 3 ways to get services if the requester is not a service.

```
GameObject.GetService<T>()
                │
                └─ SceneServices.Get<T>(scene)
                    │
                    └─ ApplicationServices.Get<T>()
```

### From GameObject

```csharp
ITimeService timeService = gameObject.GetService<ITimeService>();
```

This method requires access to a GameObject instance. It will first attempt to get the service from the Scene Scope of the scene that `gameObject` is in. If not found, it will then fallback to find a service in the Application Scope.

### From Scene 

```csharp
ITimeService timeService = SceneServices.Get<ITimeService>(scene);
```

This will require a reference to a scene, such as from `gameObject.scene`. It will search for the service in the Scene Scope. If not found, it will then fallback to find a service in the Application Scope

### From Application

```csharp
ITimeService timeService = ApplicationServices.Get<ITimeService>(scene);
```

This can be called anywhere and requires no references to any Unity Object

## ⚙️ Application Service Registration

### Registering Pure C# Services

#### Automatically 
To automatically create an instance of a service and register it as an application service, use the `[ApplicationService]` attribute.

```csharp
[ApplicationService]
public class MyService
{
    //...
}
```
#### Manually

If you need complex constructions, such as none default constructors, you can create a function to create the application service:

```csharp
public static void RegisterApplicationServices( ServicesCollectionBuilder builder)
{
    builder.Register<MyService>();
}
```
and then register that function with ApplicationServices:

```csharp
[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
private static void RegisterApplicationServices()
{ 
    ApplicationServices.AddApplicationServicesBuilderFunction(RegisterApplicationServices);
}
```

### Registering MonoBehaviour Services

> Using MonoBehaviours allows you to reference Assets/Unity Objects easily via the inspector. 

#### Assign a prefab in the **Project Settings**
The settings are under `FinalClick > Services`

<img width="1232" height="428" alt="image" src="https://github.com/user-attachments/assets/6e833d4b-2ef5-4faf-aa01-dcd7e5488244" />

This prefab will be automatically instantiated into the first scene that's loaded.

#### Registering MonoBehaviour Services

Any **root* MonoBehaviours on the Application Services Prefab with the attribute `[MonoBehaviourService]`, or a `IServiceRegisterer` implementation, can be used to register application services.

```csharp
[MonoBehaviourService]
public class MyService : MonoBehavour
{
    ///
}
```

or

```csharp
public class MyService : MonoBehavour, IServiceRegisterer
{
    public void RegisterServices(ServiceCollectionBuilder builder)
    {
        builder.Register<MyService>(this);
    }
}
```

> Note, at build time this prefab is instantiated into the first scene. This causes it to be unpacked and baked at build time so there is no instantiation costs in builds.

---

## ⚙️ Scene Service Registration

Scene services can be pure csharp classes or `MonoBehaviours`. However, they can **only** be registered via MonoBehaviours. MonoBehaviourServices, or registerers, must be on a **root** GameObject in the scene to be found.

1. Any **root** MonoBehaviour components of a type with the attribute `[MonoBehaviourService]` will be registered.
2. Any **root** MonoBehaviour components that implement `IServiceRegisterer` will have their `IServiceRegisterer.RegisterServices` function invoked during scene service registration.

---

## Service Lifecycle (`IService`)

For services that need structured startup, update, and shutdown behaviour, implement the `IService` interface:

```csharp
namespace FinalClick.Services
{
    public interface IService
    {
        void OnServiceStart();
        void OnServiceUpdate();
        void OnServiceStop();
    }
}
```

| Method             | Application Service Called When | Scene Service Called When |
|---------------------|-------------------|---------------------------|
| `OnServiceStart()`  | On Application startup | On Scene Loaded           |
| `OnServiceUpdate()` | Once per frame    | Once per frame            |
| `OnServiceStop()`   | On Application shutdown | On Scene Unloaded         |

---

## Lifecycle Overview

| Event                             | Action                                             |
|-----------------------------------|---------------------------------------------------|
| Entering Play Mode                | Application services are registered and started.  |
| Loading a Scene                   | Scene services are registered and started.        |
| Unloading a Scene                 | Scene services are stopped.                       |
| Exiting Play Mode / Application Quit | Application and scene services are stopped.      |

---
