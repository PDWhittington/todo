using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.Execution;
using Todo.Contracts.Services.UI;

namespace Todo.Execution;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class PrintAndShowHtmlCommandExecutor(
    IOutputWriter outputWriter,
    IPrintHtmlCommandExecutor printHtmlCommandExecutor,
    IShowHtmlCommandExecutor showHtmlCommandExecutor,
    ILogger<PrintAndShowHtmlCommandExecutor> logger
)
    : CommandExecutorBase<PrintAndShowHtmlCommand>(outputWriter, logger),
        IPrintAndShowHtmlCommandExecutor
{
    public override void Execute(PrintAndShowHtmlCommand command)
    {
        printHtmlCommandExecutor.Execute(PrintHtmlCommand.Of(command.Date));
        showHtmlCommandExecutor.Execute(ShowHtmlCommand.Of(command.Date));
    }
}
