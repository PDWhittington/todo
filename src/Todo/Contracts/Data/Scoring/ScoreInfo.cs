using System;
using Todo.Contracts.Data.Config;

namespace Todo.Contracts.Data.Scoring;

public abstract record ScoreInfo
{
    public abstract DateOnly Date { get; }

    public abstract int Total();

    public abstract int GetScore(ScoreCategory category);

    public abstract bool TryGetScore(ScoreCategory category, out int score);
}

