using System;
using Todo.Contracts.Services.DateNaming;

namespace Todo.DateNaming;

public class DateNamer : IDateNamer
{
    private readonly IChristmasNewYearDateNamer _christmasNewYearDateNamer;
    private readonly IEasterDateNamer _easterDateNamer;
    private readonly ISaintsDayDateNamer _saintsDayDateNamer;

    public DateNamer(IChristmasNewYearDateNamer christmasNewYearDateNamer,
        IEasterDateNamer easterDateNamer, ISaintsDayDateNamer saintsDayDateNamer)
    {
        _christmasNewYearDateNamer = christmasNewYearDateNamer;
        _easterDateNamer = easterDateNamer;
        _saintsDayDateNamer = saintsDayDateNamer;
    }
    
    public bool TryGetName(DateOnly date, out string name)
    {
        //Order important -- Easter should trump a saint's day
        
        if (_christmasNewYearDateNamer.TryGetName(date, out name)) return true;
        if (_easterDateNamer.TryGetName(date, out name)) return true;
        if (_saintsDayDateNamer.TryGetName(date, out name)) return true;

        name = null;
        return false; 
    }
}