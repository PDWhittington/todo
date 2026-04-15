using System;
using Todo.Contracts.Data.Scoring;

namespace Todo.Contracts.Services.GamifyOperations;

public interface IScoresGenerator
{
    // ReSharper disable once UnusedMember.Global
    ScoreInfo[] GetScoresForDateInterval(DateOnly fromExclusive, DateOnly toInclusive);

    ScoreInfo[] GetNonZeroScoresForDateInterval(DateOnly fromExclusive, DateOnly toInclusive);
}