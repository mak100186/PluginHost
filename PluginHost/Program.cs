using System.Reflection;
using System.Runtime.Loader;

using PluginHost;
using PluginHost.Loader;

using SharedLibrary;

var builder = WebApplication.CreateBuilder(args);
builder.Host.ConfigureSerilog();

// Add services to the container.
var location = new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName!;

AssemblyLoadContext.Default.Resolving += (assemblyLoadContext, assemblyName) =>
{
    var fullyQualifiedPath = Path.Combine(location, $"{assemblyName.Name}.dll");
    try
    {
        var something = assemblyLoadContext.LoadFromAssemblyPath(fullyQualifiedPath);

        return something;
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
        return null;
    }    
};

builder.Services.AddControllers();

builder.Services.AddSingleton<IPluginLoader, PluginLoader>();
builder.Services.AddHostedService<LoaderBackgroundService>();

InstallPlugin("C:\\Users\\muhkha\\source\\repos\\PluginHost\\PluginHost\\PluginA\\bin\\Debug\\net6.0\\PluginA.dll", builder.Configuration);


//Start app
var app = builder.Build();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();


void InstallPlugin(string pluginPath, IConfiguration config)
{
    var pluginAssembly = Assembly.LoadFrom(pluginPath);
    var pluginRegistrantTypeName = pluginAssembly.GetTypes()
        .Single(t => t.GetInterfaces().Any(i => i.Name == "IRegistrant")).FullName;

    var pluginRegistrant = pluginAssembly.CreateInstance<IRegistrant>(pluginRegistrantTypeName!);

    pluginRegistrant.Register(builder.Services, config); // create services the host doesn't know about

    // a plugin can contribute more than one class
    //foreach (var pluginType in pluginAssembly.GetTypes().Where(t => t.GetInterfaces().Any(i => i.Name == nameof(IPlugin))))
    //{
    //    var pluginTypeCtor = pluginType.GetConstructors().Single(); // exactly one ctor per plugin class
    //    var pluginTypeCtorParamInfos = pluginTypeCtor
    //        .GetParameters()
    //        .OrderBy(pi => pi.Position);
    //}
}