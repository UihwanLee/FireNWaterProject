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

    private GameObject _ember;
    private GameObject _wade;
    private EmberController _emberController;
    private WadeController _wadeController;

    public void Init(
        GameObject ember,
        GameObject wade,
        EmberController emberController,
        WadeController wadeController)
    {
        _ember = ember;
        _wade = wade;
        _emberController = emberController;
        _wadeController = wadeController;
    }

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

    private void SetSpawnPoint()
    {
        // 스폰 장소 지정
        _ember.transform.position = _emberSpwanPoint.position;
        _wade.transform.position = _wadeSpwanPoint.position;
    }

    public void OnStart()
    {
        SetSpawnPoint();
    }

    public void OnPause()
    {
        _emberController.Pause();
        _wadeController.Pause();
    }

    public void OnResume()
    {
        _emberController.CancelPause();
        _wadeController.CancelPause();
    }

    public void OnExit()
    {
        Logger.NotImpl();
    }

    public void GameOver()
    {
        Logger.NotImpl();
    }

    public void OnClear()
    {
        Logger.NotImpl();
    }

    public void CheckScore()
    {
        Logger.NotImpl();
    }
}
