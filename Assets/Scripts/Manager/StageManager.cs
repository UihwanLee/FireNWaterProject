using System;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private StageUI _stageUI;

    [Header("캐릭터")]
    [SerializeField] private GameObject _ember;
    [SerializeField] private GameObject _wade;

    private EmberController _emberController;
    private WadeController _wadeController;

    // 스테이지 정보
    private Dictionary<int, StageController> _stages = new();
    private StageController _currentStage;
    private readonly Dictionary<GameState, Action> _stateHandlers = new();

    // 게임 상태 정보
    private GameState _currentGameState = GameState.None;

    // 타이머
    private float _timer = 0f;
    public float Timer
    {
        get => _timer;
        private set
        {
            if (!_checkTimeLimit && _currentStage.LimitTime < value)
            {
                OnFailedToClearWithinTimeLimit?.Invoke();
                _checkTimeLimit = true;
            }
            _timer = value;
            _stageUI.UpdateTime(value);
        }
    }

    // 플래그 및 트리거
    private bool _checkTimeLimit = false;
    public event Action OnFailedToClearWithinTimeLimit;
    public event Action OnClearStage;
    public event Action OnStartStage;

    #region Awake State Manager
    private void Awake()
    {
        InitCharacters();
        InitStateHandlers();
    }

    private T GetOrFindController<T>(GameObject obj, string name) where T : Component
    {
        if (obj == null)
        {
            Logger.Log($"{name} 프리팹 비어 있음");
        }

        if (!obj.TryGetComponent(out T controller))
        {
            Logger.Log($"{name} Controller 가져오기");
            controller = FindObjectOfType<T>(true);
        }

        return controller;
    }

    private void InitCharacters()
    {
        _emberController = GetOrFindController<EmberController>(_ember, "엠버");
        _wadeController = GetOrFindController<WadeController>(_wade, "웨이드");

        Logger.Log("엠버, 웨이드 초기화 및 비활성화 완료");
    }

    private void InitStateHandlers()
    {
        _stateHandlers.Clear();
        _stateHandlers[GameState.Start] = HandleStageStart;
        _stateHandlers[GameState.Play] = () => Logger.Log("플레이 중");
        _stateHandlers[GameState.Pause] = HandlePause;
        _stateHandlers[GameState.Resume] = HandleResume;
        _stateHandlers[GameState.Dead] = GameOver;
        _stateHandlers[GameState.Clear] = HandleStageClear;
        _stateHandlers[GameState.Exit] = HandleStageExit;
        _stateHandlers[GameState.Next] = HandleStageNext;
    }
    #endregion

    public void Init()
    {
        var stages = GetComponentsInChildren<StageController>(true);    // 비활성화된 object도 탐색

        _stages.Clear();
        foreach (var stage in stages)
        {
            int id = stage.StageId;

            if (_stages.ContainsKey(id))
            {
                Debug.LogError($"[StageManager.Init] {id} 번째 스테이지 중복 할당");
                continue;
            }

            _stages[id] = stage;                                        // dict 채우기
            stage.gameObject.SetActive(false);                          // 모두 비활성화 하기
        }
        Debug.Log($"[StageManager.Init] 등록된 Stage 수: {_stages.Count}");

        _stageUI.ShowStageMapUI();
    }

    private void OnEnable()
    {
        Logger.Log("Stage Manager 엘리게이트 추가");
        _emberController.OnPlayerDied += HandlePlayerDeath;
        _wadeController.OnPlayerDied += HandlePlayerDeath;
    }

    private void Update()
    {
        if (_currentGameState == GameState.Play)
        {
            // 타이머 돌아가는 로직 작성
            Timer += Time.deltaTime;
        }
        //Logger.Log($"시간: {Timer.ToString()}");
        if (Input.GetKeyDown(KeyCode.Alpha2)) ChangeGameState(GameState.Start);
        if (Input.GetKeyDown(KeyCode.Alpha3)) ChangeGameState(GameState.Pause);
        if (Input.GetKeyDown(KeyCode.Alpha4)) ChangeGameState(GameState.Resume);
        if (Input.GetKeyDown(KeyCode.Alpha5)) ChangeGameState(GameState.Dead);
        if (Input.GetKeyDown(KeyCode.Alpha7)) ChangeGameState(GameState.Exit);
        if (Input.GetKeyDown(KeyCode.Alpha8)) ChangeGameState(GameState.Clear);
        if (Input.GetKeyDown(KeyCode.Alpha9)) ChangeGameState(GameState.Next);
    }

    private void OnDisable()
    {
        Logger.Log("Stage Manager 엘리게이트 제거");
        _emberController.OnPlayerDied -= HandlePlayerDeath;
        _wadeController.OnPlayerDied -= HandlePlayerDeath;
    }

    // 스테이지 선택
    public void SelectStage(int id)
    {
        if (!_stages.TryGetValue(id, out var stage))
        {
            Logger.LogWarning($"{id} 번째 스테이지 존재하지 않음");
            return;
        }

        if (_currentStage != null)
        {
            _currentStage.gameObject.SetActive(false);          // 이전 스테이지 확인
        }

        _currentStage = _stages[id];
        _currentStage.gameObject.SetActive(true);               // 활성화
        _currentStage.Init(_ember, _wade, _emberController, _wadeController);
        Logger.Log($"{id} 번째 스테이지 활성화");

        ChangeGameState(GameState.Start);                       // 자동 시작
    }

    #region 스테이지 상태 변경
    /// <summary>
    /// GameState FSM -> Figma FSM 참고
    /// </summary>
    private readonly Dictionary<GameState, GameState[]> _allowedTransitions = new()
    {
        { GameState.None,  new[] { GameState.Start } },
        { GameState.Start, new[] { GameState.Play } },
        { GameState.Play,  new[] { GameState.Pause, GameState.Clear, GameState.Dead } },
        { GameState.Pause,  new[] { GameState.Start, GameState.Resume, GameState.Exit } },
        { GameState.Resume, new[] { GameState.Play } },
        { GameState.Dead,  new[] { GameState.Start, GameState.Exit } },
        { GameState.Clear, new[] { GameState.Exit, GameState.Next, GameState.Start } },
        { GameState.Exit,   new[] { GameState.None } },
        { GameState.Next,   new[] { GameState.Start } }
    };

    public void ChangeGameState(GameState gameState)
    {
        if (!CanTransitionTo(gameState)) return;

        _currentGameState = gameState;
        Logger.Log($"상태 변경: {_currentGameState}");
        HandleStateChanged(gameState);

        if (gameState == GameState.Start)
        {
            ResetStage();
            ChangeGameState(GameState.Play);
        }
    }

    /// <summary>
    /// 변경 가능한 상태인지 검증
    /// </summary>
    /// <param name="gameState"></param>
    /// <returns></returns>
    private bool CanTransitionTo(GameState gameState)
    {
        // 동일한 상태일 경우 스킵
        if (_currentGameState == gameState) return false;

        // FSM 유효한지 확인
        if (!_allowedTransitions.TryGetValue(_currentGameState, out var allowedStates) ||
            Array.IndexOf(allowedStates, gameState) == -1)
        {
            Logger.Log($"상태 변경 불가: {_currentGameState} → {gameState}");
            return false;
        }

        return true;
    }

    /// <summary>
    /// 게임 상태에 따라 다른 로직을 처리하기 위한 메서드
    /// 유효성 검사를 위해 ChangeGameState 사용을 권장
    /// </summary>
    /// <param name="state"></param>
    private void HandleStateChanged(GameState state)
    {
        if (_currentStage == null)
        {
            Logger.LogWarning($"현재 저장된 stage 없음");
            return;
        }

        if (_stateHandlers.TryGetValue(state, out var handler))
        {
            handler?.Invoke();
        }
        else
        {
            Logger.Log($"{state} 할당 함수 없음");
        }
    }
    #endregion

    #region 스테이지 내부 로직
    private void HandleStageStart()
    {
        _currentStage.ExecuteStageStart();
        _stageUI.ShowTimerUI();
    }

    private void HandlePause()
    {
        _currentStage.ExecutePause();
    }

    private void HandleResume()
    {
        _currentStage.ExecuteResume();
        ChangeGameState(GameState.Play);
    }

    private void GameOver()
    {
        _currentStage.GameOver();
    }

    private void HandleStageExit()
    {
        _stageUI.ShowStageMapUI();
        _stageUI.CloseTimeUI();

        _currentStage.ExecuteExit();
        _currentStage.gameObject.SetActive(false);  // 비활성화
        _currentStage = null;
        ChangeGameState(GameState.None);
    }

    private void HandleStageClear()
    {
        _currentStage.ExecuteClear();
        OnClearStage?.Invoke();
    }

    /// <summary>
    /// 다음 스테이지 이동
    /// stage 예외처리는 SelectStage에서 진행
    /// </summary>
    private void HandleStageNext()
    {
        int id = _currentStage.StageId;
        SelectStage(id + 1);
    }
    #endregion

    public bool HandleCheckGemCount(int currentGemCount)
    {
        return _currentStage.CheckGemCount(currentGemCount);
    }

    private void ResetStage()
    {
        OnStartStage?.Invoke();
        ResetTimer();
        _currentStage.RevivePlayer();
        _currentStage.ResetJemState();
        Logger.Log("스테이지 정보 초기화");
    }

    private void ResetTimer()
    {
        Logger.Log("시간 초기화");
        Timer = 0f;
        _checkTimeLimit = false;
    }

    private void HandlePlayerDeath()
    {
        ChangeGameState(GameState.Dead);
    }

    public StageClearInfo GetStageClearInfo()
    {
        return _currentStage.GetStageClearInfo(Timer);
    }
}
