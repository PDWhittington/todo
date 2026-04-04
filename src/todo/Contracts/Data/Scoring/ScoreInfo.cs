using Todo.Contracts.Data.FileSystem;

namespace Todo.Contracts.Data.Scoring;

public class ScoreInfo
{
    public FilePathInfo FilePath { get; }
    public int ScoreNotDone { get; }
    public int ScoreDone { get; }
    public int CarriedForward { get; }
    public int Outstanding { get; }

    public int Total() => ScoreNotDone + ScoreDone + CarriedForward + Outstanding;
    
    private ScoreInfo(FilePathInfo filePathInfo, int scoreNotDone, 
        int scoreDone, int carriedForward, int outstanding)
    {
        FilePath = filePathInfo;
        ScoreNotDone = scoreNotDone;
        ScoreDone = scoreDone;
        CarriedForward = carriedForward;
        Outstanding = outstanding;
    }
    
    public static ScoreInfo Of(FilePathInfo filePathInfo, int scoreNotDone, 
        int scoreDone, int carriedForward, int outstanding) =>
        new(filePathInfo, scoreNotDone, scoreDone, carriedForward, outstanding);
}