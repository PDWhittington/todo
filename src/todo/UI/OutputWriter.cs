using System;
using Todo.Contracts.Services.UI;

namespace Todo.UI;

public class OutputWriter : IOutputWriter
{
    public void WriteLine() => WriteLine("");

    public void WriteLine(object obj) => WriteLine(obj.ToString() ?? "");

    public void WriteLine(string message) => Console.WriteLine(message);
}
