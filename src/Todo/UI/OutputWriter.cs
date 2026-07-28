using System;
using System.Collections.Concurrent;
using System.Threading;
using Todo.Contracts.Services.UI;

namespace Todo.UI;

public class OutputWriter : IOutputWriter
{
    private bool _initialised;
    private Thread? _writingThread;
    private BlockingCollection<string>? _pipe;

    public void WriteLine() => WriteLine("");

    public void WriteLine(object obj) => WriteLine(obj.ToString() ?? "");

    public void WriteLine(string message)
    {
        CheckInitialised();
        _pipe!.Add(message);
    }

    private void CheckInitialised()
    {
        if (!_initialised) throw new InvalidOperationException("Output writer is not initialised");
    }
    
    public IOutputWriterDisposableHandle CreateDisposableHandle()
    {
        _pipe = new BlockingCollection<string>();
        _writingThread = new Thread(ConsumingThread);
        _writingThread.Start();
        _initialised = true;
        
        return new OutputWriterDisposableHandle(this);
    }

    public void JoinWritingThread()
    {
        if (!_initialised) throw new InvalidOperationException("Output writer is not initialised");

        _pipe!.CompleteAdding();
        _writingThread!.Join();
    }

    private void ConsumingThread()
    {
        while (_pipe!.TryTake(out var str, -1))
        {
            Console.WriteLine(str);
        }
    }
}