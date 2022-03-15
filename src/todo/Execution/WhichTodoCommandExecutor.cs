using System.Diagnostics.CodeAnalysis;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.Execution;
using Todo.Contracts.Services.StateAndConfig;
using Todo.Contracts.Services.UI;

namespace Todo.Execution;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class WhichTodoCommandExecutor : CommandExecutorBase<WhichTodoCommand>, IWhichTodoCommandExecutor
{
    private readonly IAssemblyInformationProvider _assemblyInformationProvider;

    public WhichTodoCommandExecutor(IAssemblyInformationProvider assemblyInformationProvider,
        IOutputWriter outputWriter)
        : base(outputWriter)
    {
        _assemblyInformationProvider = assemblyInformationProvider;
    }

    public override void Execute(WhichTodoCommand command)
    {
        OutputWriter.WriteLine($"Assembly location: {_assemblyInformationProvider.AssemblyLocation()}");
    }
}
