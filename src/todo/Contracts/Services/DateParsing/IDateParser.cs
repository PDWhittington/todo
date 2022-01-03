using System;

namespace Todo.Contracts.Services.DateParsing;

public interface IDateParser
{
    bool TryGetDate(string str, out DateOnly? dateOnly);

}