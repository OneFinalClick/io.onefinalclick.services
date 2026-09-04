# Services - [Full Documentation Here](https://docs.onelastclick.io/packages/services/start-here/getting-started/)

A simple lightweight **service locator** and **dependency injection** system for Unity.  
Use it to replace hard-referenced singletons with decoupled, testable services.

## Overview

`OneFinalClick.Services` allows you to register and resolve services both at the Project/Application scope or at an Individual Scene scope. Services can be pure C# classes or MonoBehaviours if you require references to assets or scene objects.

## Features

- 🔍 **Automatic Service Discovery** via `ApplicationService` and `MonoBehaviourService` attributes.
- 🧩 ***Automatic Dependency Injection** via the `InjectService` property attribute.
- 💡 **Application Scoped Services** via `ApplicationServices`.
- 🎬 **Scene Scoped Services** via `SceneServices`.
- ⚙️ **Lifecycle Hooks** via the `IService` interface. (`OnServiceStart`, `OnServiceUpdate`, `OnServiceStop`)
- 🚀 **Disable Domain Reload Support**: Automaticly start and stops services during scene load and unload, and when entering or exiting Play Mode.

> *Automatic Dependency Injection can only be used within service

### Example
Instead of this:

```csharp
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
}

GameManager gameManager = GameManager.Instance;
```

Just add the `ApplicationService` attribute:

```csharp
[ApplicationService]
public class GameManager
{

}
```

Then get:

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
