using System;
using Todo.Contracts.Services.DateNaming;

namespace Todo.DateNaming;

public class SpecialDateNamer : ISpecialDateNamer
{
    private readonly IChristmasNewYearDateNamer _christmasNewYearDateNamer;
    private readonly IEasterDateNamer _easterDateNamer;
    private readonly ISaintsDayDateNamer _saintsDayDateNamer;

    public SpecialDateNamer(IChristmasNewYearDateNamer christmasNewYearDateNamer,
        IEasterDateNamer easterDateNamer, ISaintsDayDateNamer saintsDayDateNamer)
    {
        _christmasNewYearDateNamer = christmasNewYearDateNamer;
        _easterDateNamer = easterDateNamer;
        _saintsDayDateNamer = saintsDayDateNamer;
    }

    public bool TryGetSpecialName(DateOnly date, out string? name)
    {
        //Order important -- Easter should trump a saint's day

        if (_christmasNewYearDateNamer.TryGetSpecialName(date, out name)) return true;
        if (_easterDateNamer.TryGetSpecialName(date, out name)) return true;
        if (_saintsDayDateNamer.TryGetSpecialName(date, out name)) return true;

        name = null;
        return false;
    }
}
