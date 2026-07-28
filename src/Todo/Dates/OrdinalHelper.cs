using System;
using Todo.Contracts.Services.Dates;

namespace Todo.Dates;

public class OrdinalHelper : IOrdinalHelper
{
    public string GetOrdinal(int num)
    {
        if (num is < 1 or > 31) throw new ArgumentException("Out of range", nameof(num));

        return num switch
        {
            1 => "st",
            2 => "nd",
            3 => "rd",
            21 => "st",
            22 => "nd",
            23 => "rd",
            31 => "st",
            _ => "th"
        };
    }
}