namespace Todo.Contracts.Services.StateAndConfig
{
    public interface ICommandLineProvider
    {
        string GetCommandLineMinusAssemblyLocation();
    }
}
