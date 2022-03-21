using System;
using Todo.Contracts.Services.Dates.Naming;
using Todo.Contracts.Services.StateAndConfig;

namespace Todo.Dates.Naming;

public class DateFormatter : IDateFormatter
{
    private readonly IConfigurationProvider _configurationProvider;
    private readonly ISpecialDateNamer _specialDateNamer;

    public DateFormatter(IConfigurationProvider configurationProvider, ISpecialDateNamer specialDateNamer)
    {
        _configurationProvider = configurationProvider;
        _specialDateNamer = specialDateNamer;
    }

    public string GetMarkdownHeader(DateOnly dateOnly) => GetDateFormatted(dateOnly, "<sup>", "</sup>");

    public string GetHtmlTitle(DateOnly dateOnly) => GetDateFormatted(dateOnly, "", "");

    private string GetDateFormatted(DateOnly dateOnly, string superscriptPre, string superscriptPost)
        => _configurationProvider.ConfigInfo.Configuration.UseNamesForDays && //Check if UseNamesForDays is turned on
           _specialDateNamer.TryGetSpecialName(dateOnly, out var dateName) // Check if current day is a special day
            ?
            $"{dateName}, {dateOnly.Year}" :
            $"{dateOnly:dddd d}{superscriptPre}{GetOrdinal(dateOnly.Day)}{superscriptPost} {dateOnly:MMMM}, {dateOnly:yyyy}";

    private static string GetOrdinal(int num)
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
