using UnityEngine;


public class StageController : MonoBehaviour
{
    [SerializeField] private Transform _spwanPoint;

    [Header("스테이지 정보")]
    [SerializeField] private int _stageId;
    [SerializeField] private int _elementCount;
    [SerializeField] private float _limitTime;

    public int StageId => _stageId;
    public int ElementCount => _elementCount;
    public float LimitTime => _limitTime;

    private Stage _stage;
    private StageClearInfo _stageClearInfo;

    private void Awake()
    {
        if (_spwanPoint == null)
        {
            transform.position = Vector3.zero;
        }

        _stage = new(_stageId, _limitTime, _elementCount);
        _stageClearInfo = new(_stage);
        Logger.Log($"stage: {_stage.ToString()}\n stage info: {_stageClearInfo.ToString()}");
    }

    public void StartStage()
    {
        Logger.NotImpl();
    }

    public void PauseStage()
    {
        Logger.NotImpl();
    }

    public void ExitStage()
    {
        Logger.NotImpl();
    }

    public void GameOver()
    {
        Logger.NotImpl();
    }

    public void ClearStage()
    {
        Logger.NotImpl();
    }

    public void CheckScore()
    {
        Logger.NotImpl();
    }
}
