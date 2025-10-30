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
    public int StageNumber { get; private set; }
    public float LimitTime { get; private set; }
    public int ElementCount { get; set; }

    public Stage(int number, float limitTime, int elementCount)
    {
        StageNumber = number;
        LimitTime = limitTime;
        ElementCount = elementCount;
    }
}

[System.Serializable]
public class StageClearInfo
{
    public int StageNumber { get; set; }
    public StageScore StageScore { get; set; }
    public float ClearTime { get; set; }

    public StageClearInfo(Stage stage)
    {
        StageNumber = stage.StageNumber;
        StageScore = StageScore.None;
        ClearTime = 0f;
    }
}