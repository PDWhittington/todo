using System.Collections.Generic;
using System.Linq;
using Todo.Contracts.Data.Config;
using Todo.Contracts.Data.FileSystem;

namespace Todo.Contracts.Data.Scoring;

public record ScoreInfo
{
    public DayListFilePathInfo FilePath { get; }
    
    private readonly Dictionary<ScoreCategory, int> _scores = new();

    public int Total() => _scores.Sum(x => x.Value);
    
    public int GetScore(ScoreCategory category) => _scores[category];
    
    public bool TryGetScore(ScoreCategory category, out int score) => _scores.TryGetValue(category, out score);

    public IEnumerable<KeyValuePair<ScoreCategory, int>> GetScores()
    {
        // ReSharper disable once ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator
        foreach (var pair in _scores) yield return pair;
    }
    
    private ScoreInfo(DayListFilePathInfo filePathInfo, IEnumerable<KeyValuePair<ScoreCategory, int>> scores)
    {
        FilePath = filePathInfo;
        _scores = scores.ToDictionary(
            x => x.Key, 
            x => x.Value);
    }
    
    public static ScoreInfo Of(DayListFilePathInfo filePathInfo, 
        IEnumerable<KeyValuePair<ScoreCategory, int>> scores) =>
        new(filePathInfo, scores);
}