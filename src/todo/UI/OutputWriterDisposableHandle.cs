using Todo.Contracts.Services.UI;

namespace Todo.UI;

public class OutputWriterDisposableHandle : IOutputWriterDisposableHandle
{
    private readonly IOutputWriter _outputWriter;

    public OutputWriterDisposableHandle(IOutputWriter outputWriter)
    {
        _outputWriter = outputWriter;
    }


    public void Dispose()
    {
        _outputWriter.JoinWritingThread();
    }
}