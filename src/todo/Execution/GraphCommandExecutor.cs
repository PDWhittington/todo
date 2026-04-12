using System.Diagnostics.CodeAnalysis;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.Dates;
using Todo.Contracts.Services.Execution;
using Todo.Contracts.Services.GamifyOperations;
using Todo.Contracts.Services.StateAndConfig;
using Todo.Contracts.Services.UI;

namespace Todo.Execution;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class GraphCommandExecutor(IOutputWriter outputWriter, IScoresGenerator scoresGenerator,
    IConfigurationProvider configurationProvider, IDateAdjuster dateAdjuster)
    : CommandExecutorBase<GraphCommand>(outputWriter), IGraphCommandExecutor
{
    public override void Execute(GraphCommand command)
    {
        var now = dateAdjuster.GetTodayWithMidnightAdjusted();
        var intervalDays = configurationProvider.ConfigInfo.Configuration.DefaultDayIntervalForGamify;
        var start = now.AddDays(-intervalDays);
      
        var scoreInfos = scoresGenerator.GetNonZeroScoresForDateInterval(start, now);
        
        OutputWriter.WriteLine("Graphing!");
    }
}
