using System.Diagnostics;

namespace Todo.Contracts.Services.AppLaunching;

public interface IProcessLauncher
{
    Process? Start(string fileName, string arguments);
}
