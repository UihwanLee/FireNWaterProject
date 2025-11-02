public enum GameState
{
    None,
    Start,
    Play,
    Pause,
    Dead,
    Clear,
    Exit,
    Next,
    Resume,
}

public enum StageScore
{
    None, // Default
    A, // 조건 전부 통과
    B, // 조건 2개만 통과
    C, // 조건 1개만 통과
}

[System.Serializable]
public class Stage
{
    public int StageId { get; private set; }
    public float LimitTime { get; private set; }
    public int GemCount { get; private set; }

    public Stage(int id, float limitTime, int gemCount)
    {
        StageId = id;
        LimitTime = limitTime;
        GemCount = gemCount;
    }

    public override string ToString()
    {
        return "{ " + $"id: {StageId}, limit time: {LimitTime}, gem count: {GemCount}" + " }";
    }
}

[System.Serializable]
public class StageClearInfo
{
    public int StageId { get; private set; }
    public StageScore StageScore { get; set; }
    public float ClearTime { get; set; }

    public StageClearInfo(Stage stage)
    {
        StageId = stage.StageId;
        StageScore = StageScore.None;
        ClearTime = 0f;
    }

    public StageClearInfo(int stageId)
    {
        StageId = stageId;
        StageScore = StageScore.None;
        ClearTime = 0f;
    }

    public override string ToString()
    {
        return "{ " + $"id: {StageId}, stage score: {StageScore}, clear time: {ClearTime}" + " }";
    }
}