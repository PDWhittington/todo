using System.Collections.Generic;
using Todo.Contracts.Data.HelpMessages;

namespace Todo.Contracts.Services.UI;

public interface IConsoleTextFormatter
{
    string CreateTable(IEnumerable<CommandHelpMessage> rows);

    IEnumerable<string> WrapText(IEnumerable<string> lines, int columnWidth);
}
