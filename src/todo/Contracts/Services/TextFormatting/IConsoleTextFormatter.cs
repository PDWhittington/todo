using System.Collections.Generic;
using Todo.Contracts.Data.HelpMessages;

namespace Todo.Contracts.Services.TextFormatting;

public interface IConsoleTextFormatter
{
    string CreateTable(IEnumerable<CommandHelpMessage> rows);
}
