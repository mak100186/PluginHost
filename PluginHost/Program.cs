using System.Reflection;
using System.Runtime.Loader;

using PluginHost;

using SharedLibrary;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var location = new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName!;


AssemblyLoadContext.Default.Resolving += (assemblyLoadContext, assemblyName) =>
{
    var fullyQualifiedPath = Path.Combine(location, $"{assemblyName.Name}.dll");

    return assemblyLoadContext.LoadFromAssemblyPath(fullyQualifiedPath);
};

builder.Services.AddControllers();

InstallPlugin("C:\\Users\\muhkha\\source\\repos\\PluginHost\\PluginHost\\PluginA\\bin\\Debug\\net6.0\\PluginA.dll", builder.Configuration);

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using var scope = app.Services.CreateScope();
scope.ServiceProvider.GetRequiredService<IPlugin>()?.DoSomething(); //this is the runner

app.Run();


void InstallPlugin(string pluginPath, IConfiguration config)
{
    var pluginAssembly = Assembly.LoadFrom(pluginPath);
    var pluginRegistrantTypeName = pluginAssembly.GetTypes()
        .Single(t => t.GetInterfaces().Any(i => i.Name == "IRegistrant")).FullName;

    var pluginRegistrant = pluginAssembly.CreateInstance<IRegistrant>(pluginRegistrantTypeName!);

    pluginRegistrant.Register(builder.Services, config); // create services the host doesn't know about

    // a plugin can contribute more than one class
    foreach (var pluginType in pluginAssembly.GetTypes().Where(t => t.GetInterfaces().Any(i => i.Name == nameof(IPlugin))))
    {
        var pluginTypeCtor = pluginType.GetConstructors().Single(); // exactly one ctor per plugin class
        var pluginTypeCtorParamInfos = pluginTypeCtor
            .GetParameters()
            .OrderBy(pi => pi.Position);
        //builder.Services.AddScoped<shared_types.IPluginMultiply>(sp =>
        //{
        //    var pluginCtorParamValues = pluginTypeCtorParamInfos.Select(p => sp.GetService(p.ParameterType)!) ?? new List<Type>();
        //    var result = pluginAssembly.CreateInstance<shared_types.IPluginMultiply>(pluginType.FullName!, pluginCtorParamValues)!;
        //    return result!;
        //});
    }
}