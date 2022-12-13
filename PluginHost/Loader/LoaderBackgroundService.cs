namespace PluginHost.Loader;

public class LoaderBackgroundService : BackgroundService
{
    private readonly IPluginLoader _pluginLoader;

    public LoaderBackgroundService(IPluginLoader pluginLoader)
    {
        _pluginLoader = pluginLoader;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            if (!stoppingToken.IsCancellationRequested)
            {
                await _pluginLoader.StartPlugins();

                Environment.Exit(0);
            }
        }
        catch (Exception ex)
        {
            var innerException = ex.InnerException;
            while (innerException != null)
            {
                innerException = innerException.InnerException;
            }
        }
    }
}
