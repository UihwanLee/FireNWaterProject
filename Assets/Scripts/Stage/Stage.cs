public enum GameState
{
    None,
    Start,
    Play,
    Pause,
    Dead,
    Clear,
    End,
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
    public int ElementCount { get; private set; }

    public Stage(int id, float limitTime, int elementCount)
    {
        StageId = id;
        LimitTime = limitTime;
        ElementCount = elementCount;
    }

    public override string ToString()
    {
        return "{ " + $"id: {StageId}, limit time: {LimitTime}, element count: {ElementCount}" + " }";
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

    public override string ToString()
    {
        return "{ " + $"id: {StageId}, stage score: {StageScore}, clear time: {ClearTime}" + " }";
    }
}