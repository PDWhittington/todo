namespace Todo.Contracts.Services.UI;

public interface IOutputWriter
{
    void WriteLine();

    void WriteLine(object obj);

    void WriteLine(string message);

    IOutputWriterDisposableHandle CreateDisposableHandle();
    
    void JoinWritingThread();
}
