using System.Reflection;

namespace PluginHost;

public static class AssemblyExtensions
{
    #region create instance from typeName

    public static object CreateInstance(this Assembly assembly, string typeName, params object[] parmArray)
    {
        if (parmArray.Length > 0)
        {
            var culture = System.Threading.Thread.CurrentThread.CurrentCulture;
            var activationAttrs = new object[] { };
            return assembly.CreateInstance(typeName, false, BindingFlags.CreateInstance, null, parmArray, culture, activationAttrs)!;
        }
        else
        {
            return assembly.CreateInstance(typeName)!;
        }
    }

    public static object CreateInstance(this Assembly assembly, string typeName, IEnumerable<object> parmList) => CreateInstance(assembly, typeName, parmList);

    public static T CreateInstance<T>(this Assembly assembly, string typeName, params object[] parmArray) => (T)CreateInstance(assembly, typeName, parmArray);

    public static T CreateInstance<T>(this Assembly assembly, string typeName, IEnumerable<object> parmList) => (T)CreateInstance(assembly, typeName, parmList.ToArray());

    #endregion



    #region create instance from type

    public static object? CreateInstance(this Type type, params object[] parmArray)
    {
        if (parmArray.Length > 0)
        {
            var culture = System.Threading.Thread.CurrentThread.CurrentCulture;
            var activationAttrs = new object[] { };
            return type.Assembly.CreateInstance(type.Name, false, BindingFlags.CreateInstance, null, parmArray, culture, activationAttrs);
        }
        else
        {
            return type.Assembly.CreateInstance(type.FullName!);
        }
    }

    public static object CreateInstance(this Type type, IEnumerable<object> parmList) => CreateInstance(type, parmList.ToArray())!;

    #endregion


    public static string GetVersionName(this Assembly assembly)
    {
        var x = assembly.GetName();
        return $"{x.Name}.{x.Version?.Major ?? 0}.{x.Version?.Minor ?? 0}";
    }

    public static bool Contains(this Assembly assy, string typeName)
    {
        return assy.GetExportedTypes().Any(t => t.FullName == typeName);
    }

}