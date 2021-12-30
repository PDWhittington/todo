using System;

namespace Todo.Contracts.Services;

public interface IDateHelper
{
    DateOnly ConvertToDateOnly(DateTime dateTime);
}