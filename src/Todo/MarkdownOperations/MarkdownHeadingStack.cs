using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Todo.Contracts.Data.Markdown;
using Todo.Contracts.Data.Memory;
using Todo.Extensions;
using Todo.StringOperations;

namespace Todo.MarkdownOperations;

public class MarkdownHeadingStack : IEnumerable<MarkdownHeadingInfo>
{
    private readonly List<MarkdownHeadingInfo> _markdownHeadingStack = new(10);

    public override string ToString() => _markdownHeadingStack
        .Select(x => x.HeadingTitle.ToString())
        .StringJoin(", ");
    

    public void UpdateStack(MarkdownLineInfo markdownLineInfo)
    {
        if (markdownLineInfo.LineType != MarkdownLineTypeEnum.Heading) return;

        var headingLevel = markdownLineInfo.HeadingLevel;
        
        if (headingLevel <= 0) throw new ArgumentException(
            $"{nameof(markdownLineInfo)}.{nameof(markdownLineInfo.HeadingLevel)} must be positive", 
            nameof(markdownLineInfo));

        var headingTitle = markdownLineInfo.Line
            .TrimStart()
            .TrimStart(b => b == (byte)'#')
            .TrimStart()
            .TrimEnd();
        
        var markdownHeadingInfo = MarkdownHeadingInfo.Of(headingLevel, headingTitle);

        for (var i = _markdownHeadingStack.Count - 1; i >= 0; i--)
        {
            if (_markdownHeadingStack[i].HeadingLevel >= headingLevel)
            {
                _markdownHeadingStack.RemoveAt(i);
            }
            else break;
        }
        
        _markdownHeadingStack.Add(markdownHeadingInfo);
    }

    public IEnumerator<MarkdownHeadingInfo> GetEnumerator()
    {
        return _markdownHeadingStack.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _markdownHeadingStack.GetEnumerator();
    }
}