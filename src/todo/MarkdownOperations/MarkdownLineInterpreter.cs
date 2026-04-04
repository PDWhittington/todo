using System.Collections.Generic;
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

        for (int i = 0; i < lines.Length; i++)
        {
            var currentLine = lines[i];

            var markdownType = GetMarkdownType(currentLine);

            int headingLevel = (markdownType == MarkdownLineTypeEnum.Heading)
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
        int value = 0;
        bool hashesStarted = false;

        for (int i = 0; i < line.Length; i++)
        {
            var ch = line[i];

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

    private static MarkdownLineTypeEnum GetMarkdownType(string line)
    {
        var trimmed = line.Trim();

        if (trimmed.Length == 0) return MarkdownLineTypeEnum.EmptyLine;
        if (trimmed.StartsWith('#')) return MarkdownLineTypeEnum.Heading;
        if (trimmed.StartsWith('*')) return MarkdownLineTypeEnum.BulletPoint;
        
        return MarkdownLineTypeEnum.NormalText;
    }
}