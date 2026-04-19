using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using Markdig.Helpers;
using Todo.Contracts.Data.FileSystem;
using Todo.Contracts.Data.Markdown;
using Todo.Contracts.Services.MarkdownOperations;

namespace Todo.MarkdownOperations;

public class MarkdownLineInterpreter : IMarkdownLineInterpreter
{
    public MarkdownLineInfo[] CreateMarkdownLine(FilePathInfo filePathInfo, byte[] bytes)
    {
        var lines = CreateLines(bytes).ToArray();
        return CreateMarkdownLine(filePathInfo, lines);
    }

    private IEnumerable<string> CreateLines(byte[] bytes)
    {
        var previousIndex = 0;
        
        for (int i = 0; i < bytes.Length; i++)
        {
            var b = bytes[i];

            if (b == (byte)'\n')
            {
                yield return Encoding.UTF8.GetString(bytes, previousIndex, i - previousIndex);
                previousIndex = i;
            }
        }
        
        yield return Encoding.UTF8.GetString(bytes, previousIndex, bytes.Length - previousIndex);
        
    }
    
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