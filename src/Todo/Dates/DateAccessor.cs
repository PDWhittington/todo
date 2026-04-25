using System;
using Todo.Contracts.Services.Dates;

namespace Todo.Dates;

public class DateAccessor : IDateAccessor
{
    public DateTime GetNow() => DateTime.Now;
}