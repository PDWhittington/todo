using System;
using System.Reflection;
using System.Text;
using Todo.Contracts.Services.AssemblyOperations;

namespace Todo.AssemblyOperations;

public class ManifestStreamProvider : IManifestStreamProvider
{
    public string GetStringFromManifest(string manifestName)
    {
        var manifestStream = Assembly
            .GetExecutingAssembly()
            .GetManifestResourceStream(manifestName);

        if (manifestStream == null) throw new Exception(
            $"Manifest with name {manifestName} not found in assembly");

        var buffer = new byte[manifestStream.Length];

        manifestStream.Read(buffer, 0, buffer.Length);

        var text = Encoding.UTF8.GetString(buffer);
        return text;
    }
}
