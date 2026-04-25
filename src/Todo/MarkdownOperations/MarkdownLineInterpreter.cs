using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Markdig.Helpers;
using Todo.Contracts.Data.FileSystem;
using Todo.Contracts.Data.Markdown;
using Todo.Contracts.Services.MarkdownOperations;

namespace Todo.MarkdownOperations;

public class MarkdownLineInterpreter : IMarkdownLineInterpreter
{
    public MarkdownLineInfo [] CreateMarkdownLine(FilePathInfo filePathInfo, string [] lines)
    {
        var list = new List<MarkdownLineInfo>();

        for (var i = 0; i < lines.Length; i++)
        {
            var currentLine = lines[i];

            var markdownType = GetMarkdownType(currentLine);

            var headingLevel = markdownType == MarkdownLineTypeEnum.Heading
                ? CountHashesAtFront(currentLine)
                : -1;

            var markdownLine = MarkdownLineInfo.Of(filePathInfo, markdownType, 
                currentLine, i,  headingLevel);
            
            list.Add(markdownLine);
        }
        
        return list.ToArray();
    }

    private static int CountHashesAtFront(string line)
    {
        var value = 0;
        var hashesStarted = false;

        foreach (var ch in line)
        {
            if (ch == '#')
            {
                hashesStarted = true;
                value++;
                continue;
            }

            if (ch.IsWhitespace() && !hashesStarted) continue;

            break;
        }

        return value;
    }

    [SuppressMessage("ReSharper", "ConvertIfStatementToReturnStatement")]
    private static MarkdownLineTypeEnum GetMarkdownType(string line)
    {
        var trimmed = line.Trim();

        if (trimmed.Length == 0) return MarkdownLineTypeEnum.EmptyLine;
        if (trimmed.StartsWith('#')) return MarkdownLineTypeEnum.Heading;
        if (trimmed.StartsWith('*')) return MarkdownLineTypeEnum.BulletPoint;
        
        return MarkdownLineTypeEnum.NormalText;
    }
}