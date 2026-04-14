using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.Execution;
using Todo.Contracts.Services.StateAndConfig;
using Todo.Contracts.Services.UI;

namespace Todo.Execution;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class WhichTodoCommandExecutor(
    IOutputWriter outputWriter,
    IBoilerPlateProvider boilerPlateProvider)
    : CommandExecutorBase<WhichTodoCommand>(outputWriter), IWhichTodoCommandExecutor
{
    public override void Execute(WhichTodoCommand command) =>
        OutputWriter.WriteLine(boilerPlateProvider.GetBoilerPlate());
}
