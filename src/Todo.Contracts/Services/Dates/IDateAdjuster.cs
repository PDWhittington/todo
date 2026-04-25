using System;

namespace Todo.Contracts.Services.Dates;

public interface IDateAdjuster
{
    DateOnly GetTodayWithMidnightAdjusted();
}