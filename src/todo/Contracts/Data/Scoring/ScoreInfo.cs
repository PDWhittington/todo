using Todo.Contracts.Data.FileSystem;

namespace Todo.Contracts.Data.Scoring;

public class ScoreInfo
{
    public FilePathInfo FilePath { get; }
    public int ScoreNotDone { get; }
    public int ScoreDone { get; }
    
    public int CarriedForward { get; }

    private ScoreInfo(FilePathInfo filePathInfo, int scoreNotDone, int scoreDone, int carriedForward)
    {
        FilePath = filePathInfo;
        ScoreNotDone = scoreNotDone;
        ScoreDone = scoreDone;
        CarriedForward = carriedForward;
    }
    
    public static ScoreInfo Of(FilePathInfo filePathInfo, int scoreNotDone, int scoreDone, int carriedForward) =>
        new(filePathInfo, scoreNotDone, scoreDone, carriedForward);
}