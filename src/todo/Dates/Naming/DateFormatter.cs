using System;
using Todo.Contracts.Services.Dates;
using Todo.Contracts.Services.Dates.Naming;
using Todo.Contracts.Services.StateAndConfig;

namespace Todo.Dates.Naming;

public class DateFormatter : IDateFormatter
{
    private readonly IConfigurationProvider _configurationProvider;
    private readonly ISpecialDateNamer _specialDateNamer;
    private readonly IOrdinalHelper _ordinalHelper;

    public DateFormatter(IConfigurationProvider configurationProvider, 
        ISpecialDateNamer specialDateNamer, IOrdinalHelper ordinalHelper)
    {
        _configurationProvider = configurationProvider;
        _specialDateNamer = specialDateNamer;
        _ordinalHelper = ordinalHelper;
    }

    public string GetMarkdownHeader(DateOnly dateOnly) => GetDateFormatted(dateOnly, "<sup>", "</sup>");

    public string GetHtmlTitle(DateOnly dateOnly) => GetDateFormatted(dateOnly, "", "");

    private string GetDateFormatted(DateOnly dateOnly, string superscriptPre, string superscriptPost)
        => _configurationProvider.ConfigInfo.Configuration.UseNamesForDays && //Check if UseNamesForDays is turned on
           _specialDateNamer.TryGetSpecialName(dateOnly, out var dateName) // Check if current day is a special day
            ? $"{dateName}, {dateOnly.Year}" 
            : $"{dateOnly:dddd d}{superscriptPre}{_ordinalHelper.GetOrdinal(dateOnly.Day)}" +
              $"{superscriptPost} {dateOnly:MMMM}, {dateOnly:yyyy}";
}
