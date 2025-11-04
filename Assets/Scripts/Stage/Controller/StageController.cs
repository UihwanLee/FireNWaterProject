using System;
using UnityEngine;

public class StageController : MonoBehaviour
{
    [Header("캐릭터 스폰 포인트")]
    [SerializeField] private Transform _emberSpwanPoint;
    [SerializeField] private Transform _wadeSpwanPoint;

    [Header("스테이지 정보")]
    [SerializeField] private int _stageId;
    [SerializeField] private int _gemCount;
    [SerializeField] private float _limitTime;

    [Header("젬 오브젝트")]
    [SerializeField] private GameObject _gems;
    private BaseGem[] _baseGems;
    public event Action OnResetJem;

    private IObstacle[] _obstacles;

    public int StageId => _stageId;
    public int GemCount => _gemCount;
    public float LimitTime => _limitTime;

    private Stage _stage;
    private StageClearInfo _stageClearInfo;

    private GameObject _ember;
    private GameObject _wade;
    public GameObject Ember => _ember;
    public GameObject Wade => _wade;
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

    private void Awake()
    {
        _obstacles = GetComponentsInChildren<IObstacle>(true);
        _baseGems = _gems.GetComponentsInChildren<BaseGem>(true);

        Logger.Log("젬 초기화 이벤트 구독하기");
        foreach (var baseGem in _baseGems)
        {
            OnResetJem += baseGem.ResetObject;
        }
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

        _stage = new(_stageId, _limitTime, _gemCount);
        _stageClearInfo = new(_stage);
        Logger.Log($"stage: {_stage}\n stage info: {_stageClearInfo}");
    }

    private void OnDestroy()
    {
        Logger.Log("젬 초기화 이벤트 구독 취소하기");
        foreach (var baseGem in _baseGems)
        {
            OnResetJem -= baseGem.ResetObject;
        }
    }

    #region 플레이어 상태 변경
    private void SetPlayerActive(bool active)
    {
        Logger.Log($"Ember, Wade {(active ? "활성화" : "비활성화")}");
        _ember.SetActive(active);
        _wade.SetActive(active);
    }

    private void SetSpawnPoint()
    {
        // 스폰 장소 지정
        _ember.transform.position = _emberSpwanPoint.position;
        _wade.transform.position = _wadeSpwanPoint.position;
    }

    public void RevivePlayer()
    {
        _emberController.Revive();
        _wadeController.Revive();
    }

    private void PausePlayers()
    {
        _emberController.Pause();
        _wadeController.Pause();
    }
    #endregion

    #region 게임 로직 작성
    public void ExecuteStageStart()
    {
        SetPlayerActive(true);
        SetSpawnPoint();
    }

    public void ExecutePause()
    {
        PausePlayers();
    }

    public void ExecuteResume()
    {
        _emberController.CancelPause();
        _wadeController.CancelPause();
    }

    public void ExecuteExit()
    {
        SetPlayerActive(false);
    }

    public void GameOver()
    {
        Logger.NotImpl();
    }

    public void ExecuteClear()
    {
        PausePlayers();
    }
    #endregion

    public bool CheckGemCount(int currentGemCount)
    {
        Logger.Log($"스테이지 총 젬 개수: {_gemCount} 현재 획득 젬 개수: {currentGemCount}");
        if (_gemCount == currentGemCount) return true;
        return false;
    }

    public StageClearInfo GetStageClearInfo(float clearTime)
    {
        _stageClearInfo.ClearTime = clearTime;
        return _stageClearInfo;
    }

    public void ResetJemState()
    {
        Logger.Log("젬 상태 초기화");
        OnResetJem?.Invoke();
    }

    public void ReseetObstacles()
    {
        foreach (var obstacle in _obstacles)
        {
            obstacle.Init();
        }
    }
}
