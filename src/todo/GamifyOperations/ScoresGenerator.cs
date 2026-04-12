using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Todo.Contracts.Data.Config;
using Todo.Contracts.Data.FileSystem;
using Todo.Contracts.Data.Markdown;
using Todo.Contracts.Data.Scoring;
using Todo.Contracts.Services.FileSystem;
using Todo.Contracts.Services.GamifyOperations;
using Todo.Contracts.Services.MarkdownOperations;
using Todo.Contracts.Services.StateAndConfig;
using Todo.MarkdownOperations;
using Todo.StringOperations;

namespace Todo.GamifyOperations;

public class ScoresGenerator(IConfigurationProvider configurationProvider, 
   IFileListCreator fileListCreator, IMarkdownFileReader markdownFileReader) : IScoresGenerator
{
   [Flags]
   private enum HeadingCategoryEnum
   {
      None = 0, //Must be kept as zero as this is used in bitwise operations to disapper.
      Done = 1,
      NotDone = 2,
      CarriedForward = 4
   }

   public ScoreInfo[] GetNonZeroScoresForDateInterval(DateOnly fromExclusive, DateOnly toInclusive)
   {
      return YieldScoresForDateInterval(fromExclusive, toInclusive)
         .Where(x => x.Total() != 0)
         .ToArray();  
   }

   public ScoreInfo[] GetScoresForDateInterval(DateOnly fromExclusive, DateOnly toInclusive)
   {
      return YieldScoresForDateInterval(fromExclusive, toInclusive)
         .ToArray();
   }
   
   private IEnumerable<ScoreInfo> YieldScoresForDateInterval(DateOnly fromExclusive, DateOnly toInclusive)
   {
      var files = fileListCreator.GetFiles<DayListFilePathInfo>(
         OutputFolderEnum.MainFolder | OutputFolderEnum.ArchiveFolder,
         ListFileTypeEnum.DayList);

      var filesFiltered = files
         .Where(info => fromExclusive < info.Date && info.Date <= toInclusive)
         .OrderBy(info => info.Date);

      return YieldScoresFor(filesFiltered);
   } 
   
   private IEnumerable<ScoreInfo> YieldScoresFor(IEnumerable<DayListFilePathInfo> filePathInfos)
   {
      var scoreCategories = configurationProvider.ConfigInfo.Configuration.ScoreCategories;
      
      return configurationProvider.ConfigInfo.Configuration.FileIterationMethod switch
      {
         IterationMethodEnum.Series => filePathInfos
            .Select(x => GetScoreInfo(x, scoreCategories))
            .OrderBy(x => x.FilePath.Path),
         
         IterationMethodEnum.Parallel => filePathInfos
            .AsParallel()
            .Select(x => GetScoreInfo(x, scoreCategories))
            .OrderBy(x => x.FilePath.Path),

         _ => throw new ArgumentOutOfRangeException(
            $"{configurationProvider.ConfigInfo.Configuration.FileIterationMethod} " +
            $"should be either series or parallel.")
      };
   }

   private ScoreInfo GetScoreInfo(DayListFilePathInfo filePathInfo, ScoreCategory[] scoreCategories)
   {
      var todoFile = markdownFileReader.ReadMarkdownFile(filePathInfo);
      var markdownHeadingStack = new MarkdownHeadingStack();
      
      var scoreDictionary = new Dictionary<ScoreCategory, int>();
      
      for (var i = 0; i < todoFile.MarkdownLines.Length; i++) 
      {
         var currentLine = todoFile.MarkdownLines[i];

         if (currentLine.LineType == MarkdownLineTypeEnum.Heading)
         {
            markdownHeadingStack.UpdateStack(currentLine);
            continue;
         }

         if (!ContainsTokenScore(currentLine.Line, out var tokenScore)) continue;
         
         if (!TryGetCategoryFromStack(markdownHeadingStack, scoreCategories, out var scoreCategory)) continue;

         if (scoreDictionary.ContainsKey(scoreCategory!))
         {
            scoreDictionary[scoreCategory!] += tokenScore;
         }
         else
         {
            scoreDictionary[scoreCategory!] = tokenScore;
         }
      }
      
      return ScoreInfo.Of(filePathInfo, scoreDictionary);
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
   
   private static bool TryGetCategoryFromStack(MarkdownHeadingStack stack, 
      ScoreCategory [] scoreCategories, out ScoreCategory? scoreCategory)
   {
      foreach (var category in scoreCategories)
      {
         var isWithinSection = stack
            .Select(x => x.HeadingTitle)
            .Contains(category.Name, StringComparer.CurrentCultureIgnoreCase); 
            
         // ReSharper disable once InvertIf
         if (isWithinSection)
         {
            scoreCategory = category;
            return true;
         }
      }
   
      scoreCategory = null;
      return false;
   }
}