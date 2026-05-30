using System;
using System.Collections.Generic;
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
   public ScoreInfo [] GetScoresForDateInterval(DateOnly fromExclusive, DateOnly toInclusive)
   {
      var scoresFromFiles = YieldScoresForDateInterval(fromExclusive, toInclusive)
         .Where(x => x.Total() != 0)
         .ToDictionary(x => x.FilePath.Date, x => x);

      var list = new List<ScoreInfo>();

      for (var date = fromExclusive.AddDays(1); date <= toInclusive; date = date.AddDays(1))
      {
         if (scoresFromFiles.TryGetValue(date, out var score))
         {
            list.Add(score);
         }
         else
         {
            list.Add(EmptyScoreInfo.Of(date));
         }
      }
      
      return list.ToArray();
   }
   
   private IEnumerable<FilePathScoreInfo> YieldScoresForDateInterval(DateOnly fromExclusive, DateOnly toInclusive)
   {
      var files = fileListCreator.GetFiles<DayListFilePathInfo>(
         OutputFolderEnum.MainFolder | OutputFolderEnum.ArchiveFolder,
         ListFileTypeEnum.DayList);

      var filesFiltered = files
         .Where(info => fromExclusive < info.Date && info.Date <= toInclusive);

      return YieldScoresFor(filesFiltered);
   } 
   
   private IEnumerable<FilePathScoreInfo> YieldScoresFor(IEnumerable<DayListFilePathInfo> filePathInfos)
   {
      var scoreCategories = configurationProvider.ConfigInfo.Configuration.ScoreCategories;

      var fileIterationMethod = configurationProvider.ConfigInfo.Configuration.FileIterationMethod;
      
      var infosWithIterationMode = fileIterationMethod switch
         {
            IterationMethodEnum.Parallel => filePathInfos.AsParallel(),
            IterationMethodEnum.Series => filePathInfos,
            _ => throw new Exception("Unknown file iteration method")
         };

      return infosWithIterationMode
         .Select(x => GetScoreInfo(x, scoreCategories))
         .OrderBy(x => x.FilePath.Date);
   }

   private FilePathScoreInfo GetScoreInfo(DayListFilePathInfo filePathInfo, ScoreCategory[] scoreCategories)
   {
      var todoFile = markdownFileReader.ReadMarkdownFile(filePathInfo);
      var markdownHeadingStack = new MarkdownHeadingStack();
      
      var scoreDictionary = new Dictionary<ScoreCategory, int>();
      
      foreach (var currentLine in todoFile.MarkdownLines)
      {
         if (currentLine.LineType == MarkdownLineTypeEnum.Heading)
         {
            markdownHeadingStack.UpdateStack(currentLine);
            continue;
         }

         if (!ContainsTokenScore(currentLine.Line, out var tokenScore)) continue;

         var scoreCategory = GetCategoryFromStack(markdownHeadingStack, scoreCategories);

         if (!scoreDictionary.TryAdd(scoreCategory, tokenScore))
         {
            scoreDictionary[scoreCategory] += tokenScore;
         }
      }
      
      return FilePathScoreInfo.Of(filePathInfo, scoreDictionary);
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
   
   private static ScoreCategory GetCategoryFromStack(MarkdownHeadingStack stack, 
      ScoreCategory [] scoreCategories)
   {
      foreach (var category in scoreCategories.Where(x => !x.IsDefaultCategory))
      {
         var isWithinSection = stack
            .Select(x => x.HeadingTitle)
            .Contains(category.Name, StringComparer.CurrentCultureIgnoreCase); 
            
         // ReSharper disable once InvertIf
         if (isWithinSection)
         {
            return category;
         }
      }
   
      return scoreCategories.First(x => x.IsDefaultCategory);
   }
}