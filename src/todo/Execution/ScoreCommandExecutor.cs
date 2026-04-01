using System.Diagnostics.CodeAnalysis;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.Execution;
using Todo.Contracts.Services.UI;

namespace Todo.Execution;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class ScoreCommandExecutor(IOutputWriter outputWriter)
   : CommandExecutorBase<ScoreCommand>(outputWriter), IScoreCommandExecutor
{
   public override void Execute(ScoreCommand command)
   {
      throw new System.NotImplementedException();
   }
}
