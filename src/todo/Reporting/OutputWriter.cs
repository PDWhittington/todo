using System;
using Todo.Contracts.Services.Reporting;

namespace Todo.Reporting;

public class OutputWriter : IOutputWriter
{
    public void WriteLine() => WriteLine("");

    public void WriteLine(object obj) => WriteLine(obj.ToString() ?? "");

    public void WriteLine(string message) => Console.WriteLine(message);
}
