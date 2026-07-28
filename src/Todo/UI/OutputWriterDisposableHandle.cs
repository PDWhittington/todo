using System;
using Todo.Contracts.Services.UI;

namespace Todo.UI;

public class OutputWriterDisposableHandle(IOutputWriter outputWriter) 
    : IOutputWriterDisposableHandle
{
    public void Dispose()
    {
        GC.SuppressFinalize(this);
        outputWriter.JoinWritingThread();
    }
}