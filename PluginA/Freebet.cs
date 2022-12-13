using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using SharedLibrary;

namespace PluginA;
public class Freebet : IPlugin
{
    private readonly IFreebetService _service;
    public Freebet(IFreebetService service)
    {
        this._service = service;
    }

    public void DoSomething()
    {
        this._service.Print("Freebet plugin");
    }
}

public class Registrant : IRegistrant
{
    public IServiceCollection Register(IServiceCollection services, IConfiguration config)
    {
        services.AddSingleton<IPlugin, Freebet>();
        services.AddSingleton<IFreebetService, FreebetService>();

        return services;
    }
}

public class FreebetService : IFreebetService
{
    public void Print(string text)
    {
        Console.WriteLine(text);
    }
}