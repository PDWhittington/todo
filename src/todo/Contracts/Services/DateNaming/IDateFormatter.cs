using System;

namespace Todo.Contracts.Services.DateNaming;

public interface IDateFormatter
{
    string GetMarkdownHeader(DateOnly dateOnly);

    string GetHtmlTitle(DateOnly dateOnly);
}
