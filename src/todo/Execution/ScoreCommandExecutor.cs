using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Data.FileSystem;
using Todo.Contracts.Data.Markdown;
using Todo.Contracts.Data.Scoring;
using Todo.Contracts.Services.Execution;
using Todo.Contracts.Services.FileSystem;
using Todo.Contracts.Services.MarkdownOperations;
using Todo.Contracts.Services.StateAndConfig;
using Todo.Contracts.Services.UI;
using Todo.MarkdownOperations;

namespace Todo.Execution;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class ScoreCommandExecutor(IOutputWriter outputWriter, IFileListCreator fileListCreator,
   IConfigurationProvider configurationProvider, IMarkdownFileReader markdownFileReader)
   : CommandExecutorBase<ScoreCommand>(outputWriter), IScoreCommandExecutor
{
   private readonly IConfigurationProvider _configurationProvider = configurationProvider;

   public override void Execute(ScoreCommand command)
   {
      var files = fileListCreator.GetFiles(
         OutputFolderEnum.MainFolder | OutputFolderEnum.ArchiveFolder,
         ListFileTypeEnum.DayList);
      
      //_configurationProvider.ConfigInfo.Configuration
      
   }

   private IEnumerable<ScoreInfo> GetScoreInfos(FilePathInfo filePathInfo)
   {
      var todoFile = markdownFileReader.ReadMarkdownFile(filePathInfo);
      var markdownHeadingStack = new MarkdownHeadingStack();

      int doneScore = 0;
      int notDoneScore = 0;
      
      for (int i = 0; i < todoFile.MarkdownLines.Length; i++) 
      {
         var currentLine = todoFile.MarkdownLines[i];

         if (currentLine.LineType == MarkdownLineTypeEnum.Heading)
         {
            markdownHeadingStack.UpdateStack(currentLine);
            continue;
         }
         
         currentLine.Line.Split(' ')

         var isWithinDoneSection = markdownHeadingStack
            .Select(x => x.HeadingTitle)
            .Contains("DONE", StringComparer.CurrentCultureIgnoreCase); 

         if (isWithinDoneSection)
         {
            
         }

      }
      
      // todoFile.FileLines.Select(line => MarkdownLineInfo.)
      
   }

   private static bool ContainsTokenScore(string line, int out tokenScore)
   {
      
   }
}
