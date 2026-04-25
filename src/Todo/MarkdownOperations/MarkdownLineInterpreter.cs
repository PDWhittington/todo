using System.Collections.Generic;
using System.Text;
using Todo.Contracts.Data.FileSystem;
using Todo.Contracts.Data.Markdown;
using Todo.Contracts.Data.Memory;
using Todo.Contracts.Services.MarkdownOperations;
using Todo.Contracts.StringOperations;
using Todo.StringOperations;

namespace Todo.MarkdownOperations;

public class MarkdownLineInterpreter : IMarkdownLineInterpreter
{

    public MarkdownLineInfo[] CreateMarkdownLines(UnmanagedByteArray file)
    {
        var lines = CreateByteArraySpansByLine(file);
        return CreateMarkdownLine(lines);
    }
    
    private static IEnumerable<ByteArraySpan> CreateByteArraySpansByLine(UnmanagedByteArray file)
    {
        var previousIndex = 0;
        
        for (var i = 0; i < file.Length; i++)
        {
            var b = file.GetByte(i);

            if (b != (byte)'\n') continue;
            
            yield return new ByteArraySpan(file.Start + previousIndex, i - previousIndex); 
            previousIndex = i;
        }
        
        yield return new ByteArraySpan(file.Start + previousIndex, file.Length - previousIndex);
    }

    private static MarkdownLineInfo[] CreateMarkdownLine(IEnumerable<ByteArraySpan> lines)
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