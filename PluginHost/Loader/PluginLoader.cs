using PluginHost.Loader;

using SharedLibrary;

public class PluginLoader : IPluginLoader
{
	protected readonly List<IPlugin> _plugins;

	public PluginLoader(IEnumerable<IPlugin> plugins)
	{
		_plugins = plugins.ToList();
	}

	public async Task StartPlugins()
    {
        foreach (var pluginContext in _plugins)
        {
            pluginContext.DoSomething();
        }

        await Task.CompletedTask;
    }
}