using Todo.Contracts.Data.Config;
using Todo.Contracts.Data.FileSystem;

namespace Todo.Contracts.Data.Scoring;

public record FilePathScoreInfo : ScoreInfo
{
    public DayListFilePathInfo FilePath { get; }

    public override DateOnly Date => FilePath.Date;

    private readonly Dictionary<ScoreCategory, int> _scores = new();

    public override int Total() => _scores.Sum(x => x.Value);

    public override bool TryGetScore(ScoreCategory category, out int score) =>
        _scores.TryGetValue(category, out score);

    private FilePathScoreInfo(
        DayListFilePathInfo filePathInfo,
        IEnumerable<KeyValuePair<ScoreCategory, int>> scores
    )
    {
        FilePath = filePathInfo;
        _scores = scores.ToDictionary(x => x.Key, x => x.Value);
    }

    public static FilePathScoreInfo Of(
        DayListFilePathInfo filePathInfo,
        IEnumerable<KeyValuePair<ScoreCategory, int>> scores
    ) => new(filePathInfo, scores);
}

