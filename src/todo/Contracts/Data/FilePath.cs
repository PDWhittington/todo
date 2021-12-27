using System;
using System.IO;
using System.Reflection;

namespace Todo.Contracts.Data;

public class FilePath
{
    public string Value { get; }

    public FilePath(string value)
    {
        Value = value;
    }

    public string GetRooted()
    {
        if (Path.IsPathRooted(Value)) return Value;
        
        var assemblyLocation = Assembly.GetEntryAssembly()?.Location ?? throw new Exception("Cannot find folder of the executing process");
        var assemblyFolder = Path.GetDirectoryName(assemblyLocation) ?? throw new Exception("Cannot get containing folder of executing process");;

        return Path.Combine(assemblyFolder, Value);
    }
}