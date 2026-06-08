using Todo.Contracts.Data.Config;

namespace Todo.Contracts.Data.Scoring;

public record EmptyScoreInfo : ScoreInfo
{
    public override DateOnly Date { get; }

    public override int Total() => 0;

    public override bool TryGetScore(ScoreCategory category, out int score)
    {
        score = 0;
        return false;
    }

    private EmptyScoreInfo(DateOnly date)
    {
        Date = date;
    }

    public static EmptyScoreInfo Of(DateOnly date) => new(date);
}

