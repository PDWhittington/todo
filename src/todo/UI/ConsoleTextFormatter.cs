using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Todo.Contracts.Data.HelpMessages;
using Todo.Contracts.Services.StateAndConfig;
using Todo.Contracts.Services.UI;

namespace Todo.UI;

public class ConsoleTextFormatter : IConsoleTextFormatter
{
    private struct ColumnWidths
    {
        public int WordColumnWidth;
        public int MessageColumnWidth;
    }

    private readonly IConfigurationProvider _configurationProvider;

    public ConsoleTextFormatter(IConfigurationProvider configurationProvider)
    {
        _configurationProvider = configurationProvider;
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
            var commandDescriptionLines = WrapText(commandHelpMessage.CommandDescription, columnWidths.MessageColumnWidth).ToArray();

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

    private static string CreateRow(IReadOnlyList<string> helpWordLines, IReadOnlyList<string> commandDescriptionLines, int index, ColumnWidths columnWidths)
    {
        var sb = new StringBuilder()
            .Append('\u2502');

        var helpWord = index >= 0 && index < helpWordLines.Count ? helpWordLines[index] : "";

        var helpWordPadded = helpWord.PadRight(columnWidths.WordColumnWidth);

        sb.Append(helpWordPadded).Append('\u2502');

        var description = index >= 0 && index < commandDescriptionLines.Count ? commandDescriptionLines[index] : "";

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

    private static string HorizontalLine(RowType rowType, ColumnWidths columnWidths)
        => rowType switch
        {
            RowType.Top => "\u250C" + new string('\u2500', columnWidths.WordColumnWidth) + "\u252C" + new string('\u2500', columnWidths.MessageColumnWidth) + "\u2510",
            RowType.Middle => "\u251C" + new string('\u2500', columnWidths.WordColumnWidth) + "\u253C" + new string('\u2500', columnWidths.MessageColumnWidth) + "\u2524",
            RowType.Bottom => "\u2514"+ new string('\u2500', columnWidths.WordColumnWidth) + "\u2534" + new string('\u2500', columnWidths.MessageColumnWidth) + "\u2518",
            _ => throw new ArgumentOutOfRangeException(nameof(rowType), rowType, null)
        };

    public IEnumerable<string> WrapText(IEnumerable<string> lines, int columnWidth)
    {
        IEnumerable<string[]> GetLines(IEnumerable<string> words)
        {
            var list = new List<string>();

            var currentLineLength = 0;

            foreach (var word in words)
            {
                if (currentLineLength + word.Length > columnWidth - 1)
                {
                    yield return list.ToArray();
                    list.Clear();
                    currentLineLength = 0;
                }

                if (currentLineLength != 0) currentLineLength++; // Space

                list.Add(word);
                currentLineLength += word.Length;
            }

            yield return list.ToArray();
        }

        foreach (var line in lines)
        {
            var wordsInLine = line
                .Split(' ')
                .Select(x => x.Replace("\t", "   "));

            var outputLines = GetLines(wordsInLine)
                .Select(ol => string.Join(' ', ol));

            foreach (var outputLine in outputLines) yield return outputLine;
        }
    }

    private ColumnWidths GetColumnWidths(IEnumerable<CommandHelpMessage> rows)
    {
        var wordColumnWidth = rows
            .SelectMany(x => x.HelpWords)
            .Max(word => word.Length);

        var messageColumnWidth = _configurationProvider.Config.ConsoleWidth - wordColumnWidth - 3;

        return new ColumnWidths
        {
            MessageColumnWidth = messageColumnWidth,
            WordColumnWidth = wordColumnWidth
        };
    }
}
