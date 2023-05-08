# SessionContainer
Simple session container implementation for ASP .NET Core

### Configuring project

Firstly, need to add register few services in ```Program.cs```

```csharp
builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSessionMappings(Assembly.GetExecutingAssembly()); 

var app = builder.Build();
app.UseSession();
```

### Usage

Declare class that derives from ```SessionContainer``` with some property, for example:

```csharp
public class NumbersContainer : SessionContainer
{
    public List<int> Numbers { get; set; }

    public NumbersContainer(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
    {
    }
}
```

Class ```NumbersContainer``` will be automatically registered as transient, now application is ready to use it as service.
Object state is set up based on session contents on constructing object.
```Save()``` method saves the current state of the object to session.

```csharp
app.MapGet("/", (NumbersContainer container) =>
{
    if (container.Numbers == null)
    {
        container.Numbers = new List<int>() { 1 };
        container.Save();

        return container.Numbers;
    }

    container.Numbers.Add(container.Numbers.Last() + 1);
    container.Save();

    return container.Numbers;
});
```

### Result
As expected, with every request we get new number in list.
![2023-05-08 14-29-21 (1)](https://user-images.githubusercontent.com/105856864/236851741-48a1efe8-9aaa-4811-a8f0-e7b0e9c30b00.gif)