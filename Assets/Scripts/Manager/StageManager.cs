using System;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [Header("캐릭터")]
    [SerializeField] private GameObject _ember;
    [SerializeField] private GameObject _wade;

    private EmberController _emberController;
    private WadeController _wadeController;

    // 스테이지 정보
    private Dictionary<int, StageController> _stages = new();
    private StageController _currentStage;

    // 게임 상태 정보
    private GameState _currentGameState = GameState.None;
    public GameState CurrentGameState => _currentGameState;
    public event Action<GameState> OnGameStateChanged;

    // 측정할 정보
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
        }
    }
    private bool _checkTimeLimit = false;
    public event Action OnFailedToClearWithinTimeLimit;

    private void Awake()
    {
        if (_ember == null)
        {
            // 엠버 프리팹 원격으로 가져오기
            Logger.Log("엠버 프리팹 가져오기");
        }

        if (_wade == null)
        {
            // 웨이드 프리팹 원격으로 가져오기
            Logger.Log("웨이드 프리팹 가져오기");
        }

        if (!_ember.TryGetComponent<EmberController>(out _emberController))
        {
            Logger.Log("Ember Controller 가져오기");
            _emberController = FindObjectOfType<EmberController>();
        }

        if (!_wade.TryGetComponent<WadeController>(out _wadeController))
        {
            Logger.Log("Wade Controller 가져오기");
            _wadeController = FindObjectOfType<WadeController>();
        }

        Logger.Log("엠버, 웨이드 초기화 및 비활성화 완료");
    }

    // 초기화
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
    }

    private void OnEnable()
    {
        Logger.Log("Stage Manager 엘리게이트 추가");
        OnGameStateChanged += HandleStateChanged;
        _emberController.OnPlayerDied += HandlePlayerDeath;
        _wadeController.OnPlayerDied += HandlePlayerDeath;
    }

    private void Update()
    {
        if (CurrentGameState == GameState.Play)
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
        OnGameStateChanged -= HandleStateChanged;
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

    // 전이 가능한 상태 지정
    private readonly Dictionary<GameState, GameState[]> _allowedTransitions = new()
    {
        { GameState.None,  new[] { GameState.Start } },
        { GameState.Start, new[] { GameState.Play } },
        { GameState.Play,  new[] { GameState.Pause, GameState.Clear, GameState.Dead } },
        { GameState.Pause,  new[] { GameState.Start, GameState.Resume, GameState.Exit } },
        { GameState.Resume, new[] { GameState.Play } },
        { GameState.Dead,  new[] { GameState.Start, GameState.Exit } },
        { GameState.Clear, new[] { GameState.Exit, GameState.Next } },
        { GameState.Exit,   new[] { GameState.None } },
        { GameState.Next,   new[] { GameState.Start } }
    };

    public void ChangeGameState(GameState gameState)
    {
        // 동일한 상태일 경우 스킵
        if (CurrentGameState == gameState) return;

        // FSM 유효한지 확인
        if (!_allowedTransitions.TryGetValue(CurrentGameState, out var allowedStates) ||
            Array.IndexOf(allowedStates, gameState) == -1)
        {
            Logger.Log($"상태 변경 불가: {CurrentGameState} → {gameState}");
            return;
        }

        if (gameState == GameState.Start)
        {
            ResetStageInfo();
            Logger.Log("스테이지 정보 초기화");
        }

        _currentGameState = gameState;
        Logger.Log($"상태 변경: {_currentGameState}");
        OnGameStateChanged?.Invoke(_currentGameState);
    }

    /// <summary>
    /// 게임 상태에 따라 다른 로직을 처리하기 위한 메서드
    /// 유효성 검사를 위해 GameManager의 ChangeGameState 사용을 권장
    /// </summary>
    /// <param name="state"></param>
    private void HandleStateChanged(GameState state)
    {
        if (_currentStage == null)
        {
            Logger.LogWarning($"현재 저장된 stage 없음");
        }

        switch (state)
        {
            case GameState.Start:                   // 카운트 다운, 로딩 등
                HandleStageStart();
                break;
            case GameState.Play:                    // 실제 플레이(조작, 점수/시간 측정)
                Logger.Log("플레이 중");
                break;
            case GameState.Pause:                    // 조작 불가, 시간 멈춤
                HandlePause();
                break;
            case GameState.Resume:
                HandleResume();
                break;
            case GameState.Dead:                    // 실패, 재시작 대기
                GameOver();
                break;
            case GameState.Clear:                   // 성공, 점수 계산
                HandleStageClear();
                break;
            case GameState.Exit:                     // 맵으로 나가기
                HandleStageExit();
                break;
            case GameState.Next:                    // 다음 스테이지
                HandleStageNext();
                break;
            default:
                break;
        }
    }

    #region 스테이지 내부 로직
    private void HandleStageStart()
    {
        _currentStage.ExecuteStageStart();
        ChangeGameState(GameState.Play);
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

    public void HandleStageExit()
    {
        _currentStage.ExecuteExit();
        _currentStage.gameObject.SetActive(false);  // 비활성화
        _currentStage = null;
        ChangeGameState(GameState.None);
    }

    public void HandleStageClear()
    {
        _currentStage.ExecuteClear();
    }

    /// <summary>
    /// 다음 스테이지 이동
    /// stage 예외처리는 SelectStage에서 진행
    /// </summary>
    public void HandleStageNext()
    {
        int id = _currentStage.StageId;
        SelectStage(id + 1);
    }
    #endregion

    private void HandlePlayerDeath()
    {
        ChangeGameState(GameState.Dead);
    }

    private void ResetStageInfo()
    {
        Timer = 0f;
        _currentStage.RevivePlayer();
    }
}
