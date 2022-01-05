using System.Collections.Generic;
using Todo.Contracts.Data.HelpMessages;

namespace Todo.Contracts.Services.TextFormatting;

public interface ITableWriter
{
    void OutputTable(IEnumerable<CommandHelpMessage> rows);
}
