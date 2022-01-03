using System;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.DateNaming;
using Todo.Contracts.Services.StateAndConfig;
using Todo.Contracts.Services.Template;

namespace Todo.Template;

public class SubstitutionMaker : ISubstitutionMaker
{
    private readonly IConfigurationProvider _configurationProvider;
    private readonly ISpecialDateNamer _specialDateNamer;

    public SubstitutionMaker(IConfigurationProvider configurationProvider, ISpecialDateNamer specialDateNamer)
    {
        _configurationProvider = configurationProvider;
        _specialDateNamer = specialDateNamer;
    }

    public string MakeSubstitutions(CreateOrShowCommand command, string template)
    {
        var outputText = template.Replace(
            "{date}", GetDateText(command.Date));

        return outputText;
    }

    private string GetDateText(DateOnly date)
    {
        if (_configurationProvider.Config.UseNamesForDays &&
            _specialDateNamer.TryGetSpecialName(date, out var dateName))
        {
            return $"{dateName}, {date.Year}";
        }

        return $"{date:dddd d}<sup>{GetOrdinal(date.Day)}</sup> {date:MMMM}, {date:yyyy}";
    }

    private string GetOrdinal(int num)
    {
        if (num < 1 || num > 31) throw new ArgumentException("Out of range", nameof(num));

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
