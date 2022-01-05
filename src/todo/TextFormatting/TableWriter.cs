using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Todo.Contracts.Data.HelpMessages;
using Todo.Contracts.Services.TextFormatting;

namespace Todo.TextFormatting;

public class TableWriter : ITableWriter
{
    struct ColumnWidths
    {
        public int WordColumnWidth;
        public int MessageColumnWidth;
    }

    private int FullWidth = 80;

    public void OutputTable(IEnumerable<CommandHelpMessage> commandHelpMessages)
    {
        var commandHelpMessagesArr = commandHelpMessages.ToArray();

        var columnWidths = GetColumnWidths(commandHelpMessagesArr);

        OutputHorizontalLine(RowType.Top, columnWidths);

        for (var i = 0; i < commandHelpMessagesArr.Length; i++)
        {
            var commandHelpMessage = commandHelpMessagesArr[i];

            var helpWordLines = commandHelpMessage.HelpWords;
            var commandDescriptionLines = WrapText(commandHelpMessage.CommandDescription, columnWidths.MessageColumnWidth).ToArray();

            var maxLines = Math.Max(helpWordLines.Length, commandDescriptionLines.Length);

            for (var j = 0; j < maxLines; j++)
            {
                OutputRow(helpWordLines, commandDescriptionLines, j, columnWidths);
            }

            if (i < commandHelpMessagesArr.Length - 1) OutputHorizontalLine(RowType.Middle, columnWidths);
        }

        OutputHorizontalLine(RowType.Bottom, columnWidths);
    }

    private void OutputRow(string[] helpWordLines, string[] commandDescriptionLines, int index, ColumnWidths columnWidths)
    {
        var sb = new StringBuilder()
            .Append('\u2502');

        var helpWord = index >= 0 && index < helpWordLines.Length ? helpWordLines[index] : "";

        var helpWordPadded = helpWord.PadRight(columnWidths.WordColumnWidth);

        sb.Append(helpWordPadded).Append('\u2502');

        var description = index >= 0 && index < commandDescriptionLines.Length ? commandDescriptionLines[index] : "";

        var descriptionPadded = description.PadRight(columnWidths.MessageColumnWidth);

        sb.Append(descriptionPadded).Append('\u2502');

        Console.WriteLine(sb);
    }

    enum RowType
    {
        Top,
        Middle,
        Bottom
    }


    private void OutputHorizontalLine(RowType rowType, ColumnWidths columnWidths)
    {
        var line = rowType switch
        {
            RowType.Top => "\u250C" + new string('\u2500', columnWidths.WordColumnWidth) + "\u252C" + new string('\u2500', columnWidths.MessageColumnWidth) + "\u2510",
            RowType.Middle => "\u251C" + new string('\u2500', columnWidths.WordColumnWidth) + "\u253C" + new string('\u2500', columnWidths.MessageColumnWidth) + "\u2524",
            RowType.Bottom => "\u2514"+ new string('\u2500', columnWidths.WordColumnWidth) + "\u2534" + new string('\u2500', columnWidths.MessageColumnWidth) + "\u2518",
            _ => throw new ArgumentOutOfRangeException(nameof(rowType), rowType, null)
        };

        Console.WriteLine(line);
    }

    private IEnumerable<string> WrapText(string [] lines, int columnWidth)
    {

        IEnumerable<string[]> GetLines(string[] words)
        {
            var list = new List<string>();

            int currentLineLength = 0;

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

        for (int i = 0; i < lines.Length; i++)
        {
            var wordsInLine = lines[i].Split(' ');

            var outputLines = GetLines(wordsInLine)
                .Select(outputWord => string.Join(' ', outputWord));

            foreach (var outputLine in outputLines) yield return outputLine;

            if (i < lines.Length - 1) yield return "";
        }
    }

    private ColumnWidths GetColumnWidths(CommandHelpMessage [] rows)
    {
        var wordColumnWidth = rows
            .SelectMany(x => x.HelpWords)
            .Max(word => word.Length);

        var messageColumnWidth = FullWidth - wordColumnWidth - 3;

        return new ColumnWidths
        {
            MessageColumnWidth = messageColumnWidth,
            WordColumnWidth = wordColumnWidth,
        };
    }
}
