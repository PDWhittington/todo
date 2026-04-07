using System;

namespace Todo.Contracts.Services.Dates;

public interface IDateAccessor
{
    DateTime GetNow();
}