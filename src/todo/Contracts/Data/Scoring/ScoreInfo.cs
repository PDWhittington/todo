using Todo.Contracts.Data.FileSystem;

namespace Todo.Contracts.Data.Scoring;

public class ScoreInfo
{
    public FilePathInfo FilePath { get; }
    public int ScoreNotDone { get; }
    public int ScoreDone { get; }

    private ScoreInfo(FilePathInfo filePathInfo, int scoreNotDone, int scoreDone)
    {
        FilePath = filePathInfo;
        ScoreNotDone = scoreNotDone;
        ScoreDone = scoreDone;
    }
    
    public static ScoreInfo Of(FilePathInfo filePathInfo, int scoreNotDone, int scoreDone) =>
        new(filePathInfo, scoreNotDone, scoreDone);
}