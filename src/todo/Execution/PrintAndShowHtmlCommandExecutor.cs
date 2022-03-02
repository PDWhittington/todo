using System.Diagnostics.CodeAnalysis;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.Execution;
using Todo.Contracts.Services.Reporting;

namespace Todo.Execution;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class PrintAndShowHtmlCommandExecutor
    : CommandExecutorBase<PrintAndShowHtmlCommand>, IPrintAndShowHtmlCommandExecutor
{
    private readonly IPrintHtmlCommandExecutor _printHtmlCommandExecutor;
    private readonly IShowHtmlCommandExecutor _showHtmlCommandExecutor;

    public PrintAndShowHtmlCommandExecutor(IOutputWriter outputWriter,
        IPrintHtmlCommandExecutor printHtmlCommandExecutor, IShowHtmlCommandExecutor showHtmlCommandExecutor)
        : base(outputWriter)
    {
        _printHtmlCommandExecutor = printHtmlCommandExecutor;
        _showHtmlCommandExecutor = showHtmlCommandExecutor;
    }

    public override void Execute(PrintAndShowHtmlCommand command)
    {
        _printHtmlCommandExecutor.Execute(PrintHtmlCommand.Of(command.Date));
        _showHtmlCommandExecutor.Execute(ShowHtmlCommand.Of(command.Date));
    }
}
