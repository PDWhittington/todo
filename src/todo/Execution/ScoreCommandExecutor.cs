using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Data.Config;
using Todo.Contracts.Data.FileSystem;
using Todo.Contracts.Data.Markdown;
using Todo.Contracts.Data.Scoring;
using Todo.Contracts.Services.Execution;
using Todo.Contracts.Services.FileSystem;
using Todo.Contracts.Services.MarkdownOperations;
using Todo.Contracts.Services.StateAndConfig;
using Todo.Contracts.Services.UI;
using Todo.MarkdownOperations;
using Todo.StringOperations;

namespace Todo.Execution;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class ScoreCommandExecutor(IOutputWriter outputWriter, IFileListCreator fileListCreator,
   IConfigurationProvider configurationProvider, IMarkdownFileReader markdownFileReader)
   : CommandExecutorBase<ScoreCommand>(outputWriter), IScoreCommandExecutor
{
   [Flags]
   private enum HeadingCategoryEnum
   {
      None = 0, //Must be kept as zero as this is used in bitwise operations to disapper.
      Done = 1,
      NotDone = 2,
      CarriedForward = 4
   }

   public override void Execute(ScoreCommand command)
   {
      var files = fileListCreator.GetFiles<DayListFilePathInfo>(
         OutputFolderEnum.MainFolder | OutputFolderEnum.ArchiveFolder,
         ListFileTypeEnum.DayList);

      var scoreInfos = GetScoresForAllFiles(files);

      var scoresFilteredAndSorted = scoreInfos
         .Where(x => x.Total() != 0)
         .OrderBy(x => x.FilePath.Path);

      foreach (var scoreInfo in scoresFilteredAndSorted)
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

   private ScoreInfo[] GetScoresForAllFiles(IEnumerable<DayListFilePathInfo> filePathInfos)
   {
      return configurationProvider.ConfigInfo.Configuration.FileIterationMethod switch
      {
         IterationMethodEnum.Series => filePathInfos
            .Select(GetScoreInfo)
            .OrderBy(x => x.FilePath.Path)
            .ToArray(),
         
         IterationMethodEnum.Parallel => filePathInfos
            .AsParallel()
            .Select(GetScoreInfo)
            .OrderBy(x => x.FilePath.Path)
            .ToArray(),

         _ => throw new ArgumentOutOfRangeException(
            $"{configurationProvider.ConfigInfo.Configuration.FileIterationMethod} should be either series or parallel.")
      };
   }

   private ScoreInfo GetScoreInfo(DayListFilePathInfo filePathInfo)
   {
      var todoFile = markdownFileReader.ReadMarkdownFile(filePathInfo);
      var markdownHeadingStack = new MarkdownHeadingStack();

      var doneScore = 0;
      var notDoneScore = 0;
      var carriedForwardScore = 0;
      var outstandingScore = 0;
      
      for (var i = 0; i < todoFile.MarkdownLines.Length; i++) 
      {
         var currentLine = todoFile.MarkdownLines[i];

         if (currentLine.LineType == MarkdownLineTypeEnum.Heading)
         {
            markdownHeadingStack.UpdateStack(currentLine);
            continue;
         }

         if (!ContainsTokenScore(currentLine.Line, out var tokenScore)) continue;
         
         var currentHeading = GetHeadingCategoryFromStack(markdownHeadingStack, filePathInfo, i);
         
         switch (currentHeading)
         {
            case HeadingCategoryEnum.NotDone: notDoneScore += tokenScore; break;
            case HeadingCategoryEnum.Done: doneScore += tokenScore; break;
            case HeadingCategoryEnum.CarriedForward: carriedForwardScore += tokenScore; break;
            case HeadingCategoryEnum.None:
            default: 
               outstandingScore += tokenScore; break;
         }
      }
      
      return ScoreInfo.Of(filePathInfo, notDoneScore, doneScore, carriedForwardScore, outstandingScore);
   }

   private static bool ContainsTokenScore(string line, out int tokenScore)
   {
      var sections = GetSections(line);
      var totalScore = 0;
      var hasTokenScore = false;
      
      foreach (var section in sections)
      {
         if (!section.EndsWith('t') && !section.EndsWith('T')) continue;
         if (!section.TryIntParseAllButLast(out var score)) continue;
         if (score == 0) continue;
         
         hasTokenScore = true;
         totalScore += score;
      }
      
      tokenScore = totalScore;
      return hasTokenScore;
   }

   private static IEnumerable<CustomStringSection> GetSections(string line)
   {
      var previousCharWasAlphaNumeric = false;
      var currentSectionStart = 0;
      
      for (var i = 0; i < line.Length; i++)
      {
         var currentChar = line[i];
         var currentCharIsAlphaNumeric = char.IsLetterOrDigit(currentChar);

         switch (previousCharWasAlphaNumeric)
         {
            //Previous character was not alphanumeric and current character is alphanumeric
            //Start new section
            case false when currentCharIsAlphaNumeric:
               currentSectionStart = i;
               break;

            //Previous character was alphanumeric and current character is not alphanumeric
            //End section and yield it back
            case true when !currentCharIsAlphaNumeric:
               yield return CustomStringSection.Of(line, currentSectionStart, i - currentSectionStart);
               break;

            //TODO: this isn't quite right -- this should fire on the final char irrespective of the previous two conditions.
            default:
            {
               if (i == line.Length - 1 && currentCharIsAlphaNumeric)
               {
                  yield return CustomStringSection.Of(line, currentSectionStart, i - currentSectionStart + 1);
               }

               break;
            }
         }
         
         previousCharWasAlphaNumeric = currentCharIsAlphaNumeric;
      }
   }
   
   [SuppressMessage("ReSharper", "ConvertSwitchStatementToSwitchExpression")]
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
