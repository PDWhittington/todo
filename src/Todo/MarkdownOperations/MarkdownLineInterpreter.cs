using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using Markdig.Helpers;
using Todo.Contracts.Data.FileSystem;
using Todo.Contracts.Data.Markdown;
using Todo.Contracts.Data.Memory;
using Todo.Contracts.Services.MarkdownOperations;
using Todo.StringOperations;

namespace Todo.MarkdownOperations;

public class MarkdownLineInterpreter : IMarkdownLineInterpreter
{

    public MarkdownLineInfo[] CreateMarkdownLine(FilePathInfo filePathInfo, byte[] bytes)
    {
        var lines = CreateLines(bytes).ToArray();
        return CreateMarkdownLine(filePathInfo, bytes);
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
    
    private IEnumerable<ByteArraySpan> CreateByteArraySpansByLine(byte[] bytes)
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
    
    // private IEnumerable<ReadOnlySpan<byte>> CreateSpans(byte[] bytes)
    // {
    //     var previousIndex = 0;
    //     
    //     for (var i = 0; i < bytes.Length; i++)
    //     {
    //         var b = bytes[i];
    //
    //         if (b != (byte)'\n') continue;
    //         
    //         yield return new ReadOnlySpan<byte>(bytes, previousIndex, i - previousIndex);
    //         previousIndex = i;
    //     }
    //     
    //     yield return new ReadOnlySpan<byte>(bytes, previousIndex, bytes.Length - previousIndex);
    //     
    // }

    public MarkdownLineInfo[] CreateMarkdownLine(FilePathInfo filePathInfo, IEnumerable<ByteArraySpan> lines)
    {
        var list = new List<MarkdownLineInfo>();

        int i = 0;
        foreach (var currentLine in lines)
        {
            var markdownType = GetMarkdownType(currentLine);

            var headingLevel = markdownType == MarkdownLineTypeEnum.Heading
                ? CountHashesAtFront(currentLine)
                : -1;

            var markdownLine = MarkdownLineInfo.Of(markdownType, 
                currentLine, i,  headingLevel);
            
            list.Add(markdownLine);
            i++;
        }
        
        return list.ToArray();
    }
    
    // public MarkdownLineInfo [] CreateMarkdownLine(FilePathInfo filePathInfo, string [] lines)
    // {
    //     var list = new List<MarkdownLineInfo>();
    //
    //     for (var i = 0; i < lines.Length; i++)
    //     {
    //         var currentLine = lines[i];
    //
    //         var markdownType = GetMarkdownType(currentLine);
    //
    //         var headingLevel = markdownType == MarkdownLineTypeEnum.Heading
    //             ? CountHashesAtFront(currentLine)
    //             : -1;
    //
    //         var markdownLine = MarkdownLineInfo.Of(markdownType,
    //             currentLine, i,  headingLevel);
    //         
    //         list.Add(markdownLine);
    //     }
    //     
    //     return list.ToArray();
    // }

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

    private static int CountHashesAtFront(ByteArraySpan line)
    {
        var value = 0;
        var hashesStarted = false;

        for(var i = 0; i < line.Length; i++)
        {
            var b = line.GetByte(i);
            
            if (b == (byte)'#')
            {
                hashesStarted = true;
                value++;
                continue;
            }

            if (b.IsWhitespace() && !hashesStarted) continue;

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
    
    private static MarkdownLineTypeEnum GetMarkdownType(ByteArraySpan line)
    {   
        for(var i = 0; i < line.Length; i++)
        {   
            var b = line.GetByte(i);
            if (b.IsWhitespace()) continue;

            return b switch
            {
                (byte)'#' => MarkdownLineTypeEnum.Heading,
                (byte)'*' => MarkdownLineTypeEnum.BulletPoint,
                _ => MarkdownLineTypeEnum.NormalText
            };
        }

        return MarkdownLineTypeEnum.EmptyLine;
    }
}