using System.Reflection;

namespace Todo.Contracts.Data.FileSystem;

public struct ManifestInfo
{
    // ReSharper disable once MemberCanBePrivate.Global
    public string AssemblyName { get; }

    public string FileName { get; }

    public string FullName { get; }

    private ManifestInfo(string assemblyName, string fileName)
    {
        AssemblyName = assemblyName;
        FileName = fileName;
        FullName = $"{AssemblyName}.{FileName}";
    }

    // ReSharper disable once MemberCanBePrivate.Global
    public static ManifestInfo Of(string assemblyName, string fileName)
        => new(assemblyName, fileName);

    public static ManifestInfo Of(string fileName)
    {
        var assemblyName = Assembly.GetExecutingAssembly().GetName().Name!;
        var namespaceName = $"{assemblyName[..1].ToUpper()}{assemblyName[1..]}";

        return Of(namespaceName, fileName);
    }
}
