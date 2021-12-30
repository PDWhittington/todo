using System;
using Todo.Contracts.Services;

namespace Todo.Helpers;

public class DateHelper : IDateHelper
{
    public DateOnly ConvertToDateOnly(DateTime dateTime) 
        => new(dateTime.Year, dateTime.Month, dateTime.Day);
}