using System;
using System.Collections.Concurrent;
using System.Threading;
using Todo.Contracts.Services.UI;

namespace Todo.UI;

public class OutputWriter : IOutputWriter
{
    private Thread _writingThread;
    private BlockingCollection<string> _pipe;

    public OutputWriter()
    {
        _pipe = new BlockingCollection<string>();
        _writingThread = new Thread(ConsumingThread);
        _writingThread.Start();
    }
    
    public void WriteLine() => WriteLine("");

    public void WriteLine(object obj) => WriteLine(obj.ToString() ?? "");

    public void WriteLine(string message) => _pipe.Add(message);
    
    public IOutputWriterDisposableHandle CreateDisposableHandle()
    {
        return new OutputWriterDisposableHandle(this);
    }

    public void JoinWritingThread()
    {
        _pipe.CompleteAdding();
        _writingThread.Join();
    }

    private void ConsumingThread()
    {
        while (_pipe.TryTake(out var str, -1))
        {
            Console.WriteLine(str);
        }
    }
}
