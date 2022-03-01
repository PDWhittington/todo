namespace Todo.Contracts.Services.Reporting;

public interface IOutputWriter
{
    void WriteLine();

    void WriteLine(object obj);

    void WriteLine(string message);
}
