using System;

namespace Todo.Contracts.Services.Dates.Parsing;

public interface IDateParser
{
    bool TryGetDate(string? str, out DateOnly dateOnly);

}
