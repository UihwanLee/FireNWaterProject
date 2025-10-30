public enum GameState
{
    None,
    Ready,
    Play,
    Dead,
    Stop,
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
    public int ElementCount { get; set; }

    public Stage(int id, float limitTime, int elementCount)
    {
        StageId = id;
        LimitTime = limitTime;
        ElementCount = elementCount;
    }
}

[System.Serializable]
public class StageClearInfo
{
    public int StageId { get; set; }
    public StageScore StageScore { get; set; }
    public float ClearTime { get; set; }

    public StageClearInfo(Stage stage)
    {
        StageId = stage.StageId;
        StageScore = StageScore.None;
        ClearTime = 0f;
    }
}