using System.Diagnostics.CodeAnalysis;
using Todo.Contracts.Data.Commands;
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
      
      var scoreInfos = scoresGenerator.GetScoresForDateInterval(start, now);

      var scoreCategories = configurationProvider.ConfigInfo.Configuration.ScoreCategories;
      
      foreach (var scoreInfo in scoreInfos)
      {
         OutputWriter.WriteLine(scoreInfo.Date);
         
         foreach (var scoreCategory in scoreCategories)
         {
            scoreInfo.TryGetScore(scoreCategory, out var score); //zero if not found
            
            OutputWriter.WriteLine($"{scoreCategory.Name}: {score}");
         }
         
         OutputWriter.WriteLine($"Total: {scoreInfo.Total()}");
         OutputWriter.WriteLine();
      }
   }
}
