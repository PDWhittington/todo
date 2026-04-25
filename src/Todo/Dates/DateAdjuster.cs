using System;
using Todo.Contracts.Services.Dates;
using Todo.Contracts.Services.StateAndConfig;

namespace Todo.Dates;

public class DateAdjuster(IDateAccessor dateAccessor, 
    IConfigurationProvider configurationProvider, IDateHelper dateHelper)
    : IDateAdjuster
{
    public DateOnly GetTodayWithMidnightAdjusted()
    {
        var newDayThreshold = configurationProvider.ConfigInfo.Configuration.NewDayThreshold 
                              ?? new TimeSpan(0, 0, 0);

        var now = dateAccessor.GetNow();
        
        return now.TimeOfDay < newDayThreshold
            ? dateHelper.ConvertToDateOnly(now.AddDays(-1))
            : dateHelper.ConvertToDateOnly(now);
    }
}