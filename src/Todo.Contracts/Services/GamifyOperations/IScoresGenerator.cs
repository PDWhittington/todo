using Todo.Contracts.Data.Scoring;

namespace Todo.Contracts.Services.GamifyOperations;

public interface IScoresGenerator
{
    ScoreInfo[] GetScoresForDateInterval(DateOnly fromExclusive, DateOnly toInclusive);
}