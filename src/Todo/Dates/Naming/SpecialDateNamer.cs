using System;
using Todo.Contracts.Services.Dates.Naming;

namespace Todo.Dates.Naming;

public class SpecialDateNamer(
    IChristmasNewYearDateNamer christmasNewYearDateNamer,
    IEasterDateNamer easterDateNamer,
    ISaintsDayDateNamer saintsDayDateNamer
) : ISpecialDateNamer
{
    public bool TryGetSpecialName(DateOnly date, out string? name)
    {
        //Order important -- Easter should trump a saint's day

        if (christmasNewYearDateNamer.TryGetSpecialName(date, out name))
            return true;
        if (easterDateNamer.TryGetSpecialName(date, out name))
            return true;
        if (saintsDayDateNamer.TryGetSpecialName(date, out name))
            return true;

        name = null;
        return false;
    }
}
