using Todo.Contracts.Data.HelpMessages;

namespace Todo.Contracts.Services.UI;

public interface IConsoleTextFormatter
{
    string CreateTable(IEnumerable<CommandHelpMessage> rows);

    IEnumerable<string> WrapText(IEnumerable<string> lines, int columnWidth);

    string FormatAsUnderlined(string text);

    string FormatAsBold(string text);

    string FormatWithForegroundColour(string text, ConsoleColor foregroundColour);

    string FormatWithBackgroundColour(string text, ConsoleColor backgroundColour);
}
