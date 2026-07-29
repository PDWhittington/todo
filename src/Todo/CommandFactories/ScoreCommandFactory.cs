using System.Diagnostics.CodeAnalysis;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.StateAndConfig;
using Todo.Contracts.Services.UI;

namespace Todo.CommandFactories;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class ScoreCommandFactory(IConfigurationProvider configurationProvider, 
    IConsoleTextFormatter consoleTextFormatter, IOutputWriter outputWriter) 
   : CommandFactoryBase<ScoreCommand>(configurationProvider, consoleTextFormatter, outputWriter, Words)
{
   private static readonly string[] Words = ["score", "gamify"];

   public override ScoreCommand? TryGetCommand(string commandLine)
      => IsThisCommand(commandLine, out _)
         ? new ScoreCommand()
         : null;

   public override bool IsDefaultCommandFactory => false;

   protected override string [] HelpText { get; } =
   [
      "Allows the user to 'gamify' the todo lists and display a score for how much has been achieved" +
      "per day or per week."
   ];

   protected override string Usage => "score";
}
