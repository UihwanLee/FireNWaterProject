using UnityEngine;

public class StageController : MonoBehaviour
{
    [Header("캐릭터 스폰 포인트")]
    [SerializeField] private Transform _emberSpwanPoint;
    [SerializeField] private Transform _wadeSpwanPoint;

    [Header("스테이지 정보")]
    [SerializeField] private int _stageId;
    [SerializeField] private int _elementCount;
    [SerializeField] private float _limitTime;

    public int StageId => _stageId;
    public int ElementCount => _elementCount;
    public float LimitTime => _limitTime;

    private Stage _stage;
    private StageClearInfo _stageClearInfo;

    private void OnEnable()
    {
        if (_emberSpwanPoint == null)
        {
            _emberSpwanPoint.position = Vector3.zero;
        }

        if (_wadeSpwanPoint == null)
        {
            _wadeSpwanPoint.position = Vector3.zero;
        }

        _stage = new(_stageId, _limitTime, _elementCount);
        _stageClearInfo = new(_stage);
        Logger.Log($"stage: {_stage}\n stage info: {_stageClearInfo}");
    }

    public void SetSpawnPoint(GameObject ember, GameObject wade)
    {
        // 스폰 장소 지정
        ember.transform.position = _emberSpwanPoint.position;
        wade.transform.position = _wadeSpwanPoint.position;
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
