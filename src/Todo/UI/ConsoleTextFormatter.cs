using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Todo.Contracts.Data.HelpMessages;
using Todo.Contracts.Services.StateAndConfig;
using Todo.Contracts.Services.UI;

namespace Todo.UI;

public class ConsoleTextFormatter(IConfigurationProvider configurationProvider)
    : IConsoleTextFormatter
{
    private struct ColumnWidths
    {
        public int WordColumnWidth;
        public int MessageColumnWidth;
    }

    public string CreateTable(IEnumerable<CommandHelpMessage> commandHelpMessages)
    {
        var commandHelpMessagesArr = commandHelpMessages.ToArray();

        var columnWidths = GetColumnWidths(commandHelpMessagesArr);

        var sb = new StringBuilder();

        sb.AppendLine(HorizontalLine(RowType.Top, columnWidths));

        for (var i = 0; i < commandHelpMessagesArr.Length; i++)
        {
            var commandHelpMessage = commandHelpMessagesArr[i];

            var helpWordLines = commandHelpMessage.HelpWords;
            var commandDescriptionLines = WrapText(
                    commandHelpMessage.CommandDescription,
                    columnWidths.MessageColumnWidth
                )
                .ToArray();

            var maxLines = Math.Max(helpWordLines.Length, commandDescriptionLines.Length);

            for (var j = 0; j < maxLines; j++)
            {
                sb.AppendLine(CreateRow(helpWordLines, commandDescriptionLines, j, columnWidths));
            }

            if (i < commandHelpMessagesArr.Length - 1)
            {
                sb.AppendLine(HorizontalLine(RowType.Middle, columnWidths));
            }
        }

        sb.AppendLine(HorizontalLine(RowType.Bottom, columnWidths));

        return sb.ToString();
    }

    private static string CreateRow(
        string[] helpWordLines,
        string[] commandDescriptionLines,
        int index,
        ColumnWidths columnWidths
    )
    {
        var sb = new StringBuilder().Append('\u2502');

        var helpWord = index >= 0 && index < helpWordLines.Length ? helpWordLines[index] : "";

        var helpWordPadded = helpWord.PadRight(columnWidths.WordColumnWidth);

        sb.Append(helpWordPadded).Append('\u2502');

        var description =
            index >= 0 && index < commandDescriptionLines.Length
                ? commandDescriptionLines[index]
                : "";

        var descriptionPadded = description.PadRight(columnWidths.MessageColumnWidth);

        sb.Append(descriptionPadded).Append('\u2502');

        return sb.ToString();
    }

    private enum RowType
    {
        Top,
        Middle,
        Bottom
    }

    private static string HorizontalLine(RowType rowType, ColumnWidths columnWidths) =>
        rowType switch
        {
            RowType.Top => "\u250C"
                + new string('\u2500', columnWidths.WordColumnWidth)
                + "\u252C"
                + new string('\u2500', columnWidths.MessageColumnWidth)
                + "\u2510",
            RowType.Middle => "\u251C"
                + new string('\u2500', columnWidths.WordColumnWidth)
                + "\u253C"
                + new string('\u2500', columnWidths.MessageColumnWidth)
                + "\u2524",
            RowType.Bottom => "\u2514"
                + new string('\u2500', columnWidths.WordColumnWidth)
                + "\u2534"
                + new string('\u2500', columnWidths.MessageColumnWidth)
                + "\u2518",
            _ => throw new ArgumentOutOfRangeException(nameof(rowType), rowType, null)
        };

    public IEnumerable<string> WrapText(IEnumerable<string> lines, int columnWidth)
    {
        foreach (var line in lines)
        {
            var wordsInLine = line.Split(' ').Select(x => x.Replace("\t", "   "));

            var outputLines = GetLines(wordsInLine).Select(ol => string.Join(' ', ol));

            foreach (var outputLine in outputLines)
                yield return outputLine;
        }

        yield break;

        IEnumerable<string[]> GetLines(IEnumerable<string> words)
        {
            var list = new List<string>();

            var currentLineLength = 0;

            foreach (var word in words)
            {
                if (currentLineLength + word.Length > columnWidth - 1)
                {
                    yield return [.. list];
                    list.Clear();
                    currentLineLength = 0;
                }

                if (currentLineLength != 0)
                    currentLineLength++; // Space

                list.Add(word);
                currentLineLength += word.Length;
            }

            yield return [.. list];
        }
    }

    private ColumnWidths GetColumnWidths(IEnumerable<CommandHelpMessage> rows)
    {
        var wordColumnWidth = rows.SelectMany(x => x.HelpWords).Max(word => word.Length);

        var messageColumnWidth =
            configurationProvider.ConfigInfo.Configuration.ConsoleWidth - wordColumnWidth - 3;

        return new ColumnWidths
        {
            MessageColumnWidth = messageColumnWidth,
            WordColumnWidth = wordColumnWidth
        };
    }

    public string FormatAsUnderlined(string text) =>
        Console.IsOutputRedirected ? text : $"\e[4m{text}\e[24m";

    public string FormatAsBold(string text) =>
        Console.IsOutputRedirected ? text : $"\e[1m{text}\e[22m";

    public string FormatAsColour(string text, ConsoleColor foregroundColour, ConsoleColor backgroundColour) =>
        Console.IsOutputRedirected ? text : $"\e[{Foreground(foregroundColour)};{Background(backgroundColour)}m{text}\e[39;49m";

    public string FormatWithForegroundColour(string text, ConsoleColor foregroundColour) =>
        Console.IsOutputRedirected ? text : $"\e[{Foreground(foregroundColour)}m{text}\e[39m";

    public string FormatWithBackgroundColour(string text, ConsoleColor backgroundColour) =>
        Console.IsOutputRedirected ? text : $"\e[{Background(backgroundColour)}m{text}\e[49m";

    public static int Foreground(ConsoleColor color) => color switch
    {
        ConsoleColor.Black       => 30,
        ConsoleColor.DarkRed     => 31,
        ConsoleColor.DarkGreen   => 32,
        ConsoleColor.DarkYellow  => 33,
        ConsoleColor.DarkBlue    => 34,
        ConsoleColor.DarkMagenta => 35,
        ConsoleColor.DarkCyan    => 36,
        ConsoleColor.Gray        => 37,
        ConsoleColor.DarkGray    => 90,
        ConsoleColor.Red         => 91,
        ConsoleColor.Green       => 92,
        ConsoleColor.Yellow      => 93,
        ConsoleColor.Blue        => 94,
        ConsoleColor.Magenta     => 95,
        ConsoleColor.Cyan        => 96,
        ConsoleColor.White       => 97,
        _ => 39 // default
    };

    public static int Background(ConsoleColor color) => 
        Foreground(color) + 10;
}
