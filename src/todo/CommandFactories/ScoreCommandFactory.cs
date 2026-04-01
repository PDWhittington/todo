using System.Diagnostics.CodeAnalysis;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.UI;

namespace Todo.CommandFactories;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class ScoreCommandFactory(IOutputWriter outputWriter) 
   : CommandFactoryBase<ScoreCommand>(outputWriter, Words)
{
   private static readonly string[] Words = ["score", "gamify"];

   public override ScoreCommand? TryGetCommand(string commandLine)
   {
      if (!IsThisCommand(commandLine, out var restOfCommand)) return null;

      return new ScoreCommand();
   }

   public override bool IsDefaultCommandFactory => false;
   public override string [] HelpText { get; } = {
      "Allows the user to 'gamify' the todo lists and display a score for how much has been achieved" +
      "per day or per week.",
      "",
      "Usage: todo score"
   };
}
