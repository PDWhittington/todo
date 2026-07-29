using Todo.Contracts.Services.UI;

namespace Todo.Contracts.Services;

public interface ITodoService
{
    IOutputWriter OutputWriter { get; }
    IConsoleTextFormatter ConsoleTextFormatter { get; }
    IOutputWriterDisposableHandle InitialiseService();
    void PerformTask();
}
