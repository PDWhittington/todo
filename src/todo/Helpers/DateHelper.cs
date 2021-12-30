using System;
using System.Runtime.Serialization;
using Todo.Contracts.Services;

namespace Todo.Helpers;

public class DateHelper : IDateHelper
{
    public DateOnly ConvertToDateOnly(DateTime dateTime)
        => new DateOnly(dateTime.Year, dateTime.Month, dateTime.Day);
}