using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using SharedLibrary;

namespace PluginA;
public class Freebet : IPlugin
{
    private readonly IFreebetService _service;
    private readonly ILogger<Freebet> _logger;

    public Freebet(IFreebetService service, ILogger<Freebet> logger)
    {
        this._service = service;
        this._logger = logger;
    }

    public void DoSomething()
    {
        this._service.Print("Freebet plugin", this._logger);
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
    public void Print(string text, ILogger logger)
    {
        logger.LogInformation(text);
    }
}