using System;

namespace Todo.Contracts.Services.Dates.Naming;

public interface IDateFormatter
{
    string GetMarkdownHeader(DateOnly dateOnly);

    string GetHtmlTitle(DateOnly dateOnly);
}
