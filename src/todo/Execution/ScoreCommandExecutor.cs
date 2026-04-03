using System;
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
   [Flags]
   private enum HeadingCategoryEnum
   {
      None = 0,
      Done = 1,
      NotDone = 2,
      CarriedForward = 4,
   }
   
   private readonly IConfigurationProvider _configurationProvider = configurationProvider;

   public override void Execute(ScoreCommand command)
   {
      var files = fileListCreator.GetFiles(
         OutputFolderEnum.MainFolder | OutputFolderEnum.ArchiveFolder,
         ListFileTypeEnum.DayList);
      
      //_configurationProvider.ConfigInfo.Configuration
      
   }

   private ScoreInfo GetScoreInfo(FilePathInfo filePathInfo)
   {
      var todoFile = markdownFileReader.ReadMarkdownFile(filePathInfo);
      var markdownHeadingStack = new MarkdownHeadingStack();

      int doneScore = 0;
      int notDoneScore = 0;
      int carriedForwardScore = 0;
      
      for (int i = 0; i < todoFile.MarkdownLines.Length; i++) 
      {
         var currentLine = todoFile.MarkdownLines[i];

         if (currentLine.LineType == MarkdownLineTypeEnum.Heading)
         {
            markdownHeadingStack.UpdateStack(currentLine);
            continue;
         }

         if (!ContainsTokenScore(currentLine.Line, out int tokenScore)) continue;
         
         var currentHeading = GetHeadingCategoryFromStack(markdownHeadingStack, filePathInfo, i);
         
         switch (currentHeading)
         {
            case HeadingCategoryEnum.Done: doneScore += tokenScore; break;
            case HeadingCategoryEnum.NotDone: notDoneScore += tokenScore; break;
            case HeadingCategoryEnum.CarriedForward: carriedForwardScore += tokenScore; break;
         }
      }
      
      return ScoreInfo.Of(filePathInfo, doneScore, notDoneScore, carriedForwardScore);
   }

   private static bool ContainsTokenScore(string line, out int tokenScore)
   {
      var sections = GetSections(line);

      foreach (var section in sections)
      {
         if (!section.EndsWith('t') &&  section.EndsWith('T')) continue;
         if (!section.Take(section.Length - 1).All(x => char.IsDigit(x))) continue;
         
         tokenScore = int.Parse(section.Substring(0, section.Length - 1));
         return true;
      }
      
      tokenScore = -1;
      return false;
   }

   private static IEnumerable<string> GetSections(string line)
   {
      var previousCharWasAlphaNumeric = false;
      int currentSectionStart = 0;
      
      for (int i = 0; i < line.Length; i++)
      {
         var currentChar = line[i];
         var currentCharIsAlphaNumeric = char.IsLetterOrDigit(currentChar);

         if (!previousCharWasAlphaNumeric && currentCharIsAlphaNumeric)
         {
            currentSectionStart = i;
         }
         else if (previousCharWasAlphaNumeric && !currentCharIsAlphaNumeric)
         {
            yield return line.Substring(currentSectionStart, i - currentSectionStart);
         }
         else if (i == line.Length - 1 && currentCharIsAlphaNumeric)
         {
            yield return line.Substring(currentSectionStart, i - currentSectionStart + 1);
         }
         
         previousCharWasAlphaNumeric = currentCharIsAlphaNumeric;
      }
      
   }
   
   private static HeadingCategoryEnum GetHeadingCategoryFromStack(MarkdownHeadingStack stack, 
      FilePathInfo filePathInfo, int lineNumber)
   {
      var isWithinDoneSection = stack
         .Select(x => x.HeadingTitle)
         .Contains("DONE", StringComparer.CurrentCultureIgnoreCase) 
            ? HeadingCategoryEnum.Done 
            : HeadingCategoryEnum.None; 
      
      var isWithinNotDoneSection = stack
         .Select(x => x.HeadingTitle)
         .Contains("NOT DONE", StringComparer.CurrentCultureIgnoreCase)
            ? HeadingCategoryEnum.NotDone
            : HeadingCategoryEnum.None;

      var isWithinCarriedForwardSection = stack
         .Select(x => x.HeadingTitle)
         .Contains("CARRIED FORWARD", StringComparer.CurrentCultureIgnoreCase)
            ? HeadingCategoryEnum.CarriedForward
            : HeadingCategoryEnum.None;

      var combinedSectionInfo = isWithinDoneSection 
                                | isWithinNotDoneSection | isWithinCarriedForwardSection;

      switch (combinedSectionInfo)
      {
         case HeadingCategoryEnum.None:
         case HeadingCategoryEnum.Done:
         case HeadingCategoryEnum.NotDone:
         case HeadingCategoryEnum.CarriedForward:
            
            return combinedSectionInfo;
            
         default:
            throw new Exception($"In {filePathInfo.Path} line number {lineNumber + 1} is ambiguous in which category " +
                                $"it belongs to ({combinedSectionInfo})");
      }
   }
}
