using Microsoft.Extensions.Logging;

namespace PluginA;

public interface IFreebetService
{
    void Print(string text, ILogger logger);
}