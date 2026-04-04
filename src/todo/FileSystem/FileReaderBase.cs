using System;
using System.IO;
using System.Linq;

namespace Todo.FileSystem;

public abstract class FileReaderBase
{
    protected static string[] GetFileText(string path)
    {
        if (!File.Exists(path)) throw new Exception($"{path} not found");

        var allText = File.ReadAllText(path);
        
        //Split assuming \n and then trim out any \r. This should work on all systems
        //even with files created elsewhere.
        var lines = allText.Split('\n');

        var linesTrimmedForCarriageReturn = lines.Select(x => x.Trim('\r'));
        return linesTrimmedForCarriageReturn.ToArray();
    }
}
