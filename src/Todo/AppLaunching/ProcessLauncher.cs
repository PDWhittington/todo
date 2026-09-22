using System.Diagnostics;
using Todo.Contracts.Services.AppLaunching;

namespace Todo.AppLaunching;

public class ProcessLauncher : IProcessLauncher
{
    public Process? Start(string fileName, string arguments)
        => Process.Start(fileName, arguments);
}
