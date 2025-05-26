using System;
using System.IO;
using System.Reflection;
using System.Text;
using Todo.Contracts.Services.AssemblyOperations;

namespace Todo.AssemblyOperations;

public class ManifestStreamProvider : IManifestStreamProvider
{
    public byte[] GetBytesFromManifest(string manifestName)
    {
        var manifestStream = Assembly
            .GetExecutingAssembly()
            .GetManifestResourceStream(manifestName);

        if (manifestStream is null) throw new Exception(
            $"Manifest with name {manifestName} not found in assembly");

        var buffer = new byte[manifestStream.Length];

        manifestStream.ReadExactly(buffer);

        return buffer;
    }

    public string GetStringFromManifest(string manifestName)
    {
        var buffer = GetBytesFromManifest(manifestName);

        var text = Encoding.UTF8.GetString(buffer);
        return text;
    }

    public void WriteStringFromManifestToFile(string manifestName, string path)
    {
        var buffer = GetBytesFromManifest(manifestName);

        using var file = new FileStream(path, FileMode.Create, FileAccess.Write);
        file.Write(buffer, 0, buffer.Length);
    }
}
