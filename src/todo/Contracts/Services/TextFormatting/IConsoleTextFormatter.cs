using System.Collections.Generic;
using Todo.Contracts.Data.HelpMessages;

namespace Todo.Contracts.Services.TextFormatting;

public interface IConsoleTextFormatter
{
    string CreateTable(IEnumerable<CommandHelpMessage> rows);

    IEnumerable<string> WrapText(IReadOnlyList<string> lines, int columnWidth);
}
