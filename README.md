# SessionContainer
Simple session container implementation for ASP .NET Core

### Configuring project

Firstly, need to register few services in ```Program.cs```

```csharp
builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSessionMappings(); 

var app = builder.Build();
app.UseSession();
```

### Usage

Declare class that derives from ```SessionContainer``` with some property, for example:

```csharp
public class NumbersContainer : SessionContainer
{
    public List<int> Numbers { get; set; } = new List<int>() { };

    public NumbersContainer(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
    {
    }
}
```

Class ```NumbersContainer``` will be automatically registered as scoped, now application is ready to use ```NumbersContainer``` as service.
Object state is set up based on session contents on constructing object.
```Save()``` method saves the current state of the object to session.

```csharp
app.MapGet("/", (NumbersContainer container) =>
{
    container.Numbers.Add(container.Numbers.LastOrDefault() + 1);

    container.Save();

    return container.Numbers;
});
```

### Result
As expected, with every request we get new number in list.
![2023-05-08 14-29-21 (1)](https://user-images.githubusercontent.com/105856864/236851741-48a1efe8-9aaa-4811-a8f0-e7b0e9c30b00.gif)
