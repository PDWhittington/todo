using System;
using System.Collections;
using System.Collections.Generic;
using Todo.Contracts.Data.Markdown;

namespace Todo.MarkdownOperations;

public class MarkdownHeadingStack : IEnumerable<MarkdownHeadingInfo>
{
    private readonly List<MarkdownHeadingInfo> markdownHeadingStack = new(10);
    
    public void UpdateStack(MarkdownLineInfo markdownLineInfo)
    {
        if (markdownLineInfo.LineType != MarkdownLineTypeEnum.Heading) return;

        var headingLevel = markdownLineInfo.HeadingLevel;
        
        if (headingLevel <= 0) throw new ArgumentException("Must be positive", nameof(markdownLineInfo.HeadingLevel));
        
        var headingTitle = markdownLineInfo.Line.Trim().TrimStart('#').TrimStart();
        
        var markdownHeadingInfo = MarkdownHeadingInfo.Of(headingLevel, headingTitle);

        for (int i = markdownHeadingStack.Count - 1; i >= 0; i--)
        {
            if (markdownHeadingStack[i].HeadingLevel >= headingLevel)
            {
                markdownHeadingStack.RemoveAt(i);
            }
            else break;
        }
    }

    public IEnumerator<MarkdownHeadingInfo> GetEnumerator()
    {
        return markdownHeadingStack.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return markdownHeadingStack.GetEnumerator();
    }
}