using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Data.FileSystem;
using Todo.Contracts.Services.Dates;
using Todo.Contracts.Services.Execution;
using Todo.Contracts.Services.GamifyOperations;
using Todo.Contracts.Services.StateAndConfig;
using Todo.Contracts.Services.UI;

namespace Todo.Execution;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class ScoreCommandExecutor(IOutputWriter outputWriter,
   IScoresGenerator scoresGenerator, IConfigurationProvider configurationProvider, IDateAdjuster dateAdjuster)
   : CommandExecutorBase<ScoreCommand>(outputWriter), IScoreCommandExecutor
{
   public override void Execute(ScoreCommand command)
   {
      var now = dateAdjuster.GetTodayWithMidnightAdjusted();
      var intervalDays = configurationProvider.ConfigInfo.Configuration.DefaultDayIntervalForGamify;
      var start = now.AddDays(-intervalDays);
      
      var scoreInfos = scoresGenerator.GetNonZeroScoresForDateInterval(start, now);

      foreach (var scoreInfo in scoreInfos)
      {
         OutputWriter.WriteLine(scoreInfo.FilePath.Path);
         OutputWriter.WriteLine($"Done: {scoreInfo.ScoreDone}");
         OutputWriter.WriteLine($"NotDone: {scoreInfo.ScoreNotDone}");
         OutputWriter.WriteLine($"Carried Forward: {scoreInfo.CarriedForward}");
         OutputWriter.WriteLine($"Outstanding: {scoreInfo.Outstanding}");
         OutputWriter.WriteLine($"Total: {scoreInfo.Total()}");
         OutputWriter.WriteLine();
      }
   }
}
