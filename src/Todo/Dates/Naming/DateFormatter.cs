using System;
using Todo.Contracts.Services.Dates;
using Todo.Contracts.Services.Dates.Naming;
using Todo.Contracts.Services.StateAndConfig;

namespace Todo.Dates.Naming;

public class DateFormatter(
    IConfigurationProvider configurationProvider,
    ISpecialDateNamer specialDateNamer,
    IOrdinalHelper ordinalHelper
) : IDateFormatter
{
    public string GetMarkdownHeader(DateOnly dateOnly) =>
        GetDateFormatted(dateOnly, "<sup>", "</sup>");

    public string GetHtmlTitle(DateOnly dateOnly) => GetDateFormatted(dateOnly, "", "");

    private string GetDateFormatted(
        DateOnly dateOnly,
        string superscriptPre,
        string superscriptPost
    ) =>
        configurationProvider.ConfigInfo.Configuration.UseNamesForDays
        && //Check if UseNamesForDays is turned on
        specialDateNamer.TryGetSpecialName(dateOnly, out var dateName) // Check if current day is a special day
            ? $"{dateName}, {dateOnly.Year}"
            : $"{dateOnly:dddd d}{superscriptPre}{ordinalHelper.GetOrdinal(dateOnly.Day)}"
                + $"{superscriptPost} {dateOnly:MMMM}, {dateOnly:yyyy}";
}