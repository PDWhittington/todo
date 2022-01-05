using System;
using System.Linq;
using System.Text;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Data.HelpMessages;
using Todo.Contracts.Services.CommandFactories;
using Todo.Contracts.Services.Execution;
using Todo.Contracts.Services.TextFormatting;

namespace Todo.Execution;

public class ShowHelpExecutor : IShowHelpExecutor
{
    private readonly ICommandFactorySet _commandFactorySet;
    private readonly ITableWriter _tableWriter;

    public ShowHelpExecutor(ICommandFactorySet commandFactorySet, ITableWriter tableWriter)
    {
        _commandFactorySet = commandFactorySet;
        _tableWriter = tableWriter;
    }

    public void Execute(ShowHelpCommand command)
    {
        var commandHelpMessages = _commandFactorySet
            .GetAllCommandFactories()
            .Select(cf => new CommandHelpMessage(cf.CommandWords.ToArray(), cf.HelpText));

        var sb = new StringBuilder()
            .AppendLine(_tableWriter.CreateTable(commandHelpMessages))
            .AppendLine()
            .AppendLine(GetNotes());

        Console.WriteLine(sb);
    }

    private static string GetNotes() => _notes
        .Replace("->", "\u2192");

    private const string _notes =
@"Notes: 

createorshow is the default command. This means it can be accessed simply by typing anything that can be parsed as a date after the word todo.

Valid date formats:-

    ""y"", ""yesterday"" -> yesterday 
    [empty string], ""."", ""today"" -> today
    ""tm"", ""tomorrow"" -> tomorrow
        
    [day] -> maps to the day/month/year which is nearest in time to today
    [day]/[month] -> maps to the day/month which is nearest in time to today
    +[daycount] -> positive offset a number of days from today
    -[daycount] -> negative offset a number of days from today

[Commit Message] -> In the Commit and Sync commands, the commit message is optional. If none is supplied, then 
a standard message detailing date and time of the commit will be used. 
";

}
