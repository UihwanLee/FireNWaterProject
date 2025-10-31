using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance => _instance;

    // 추후 매니저 추가 예정
    [Header("Managers")]
    [SerializeField] private ScoreManager _scoreManager;
    private StageManager _stageManager;

    private GameState _currentGameState = GameState.None;
    public GameState CurrentGameState => _currentGameState;
    public event Action<GameState> OnGameStateChanged;

    private void Awake()
    {
        // 씬 변경되더라도 유지되게 초기화 및 설정
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        if (_scoreManager == null)
        {
            _scoreManager = GetComponentInChildren<ScoreManager>();
        }
    }

    private void Start()
    {
        UpdateSceneToStage();
        _stageManager.SelectStage(1);
    }

    public void UpdateSceneToStage()
    {
        Scene activeScene = SceneManager.GetActiveScene();          // 현재 활성화된 씬
        GameObject[] roots = activeScene.GetRootGameObjects();      // 루트 오브젝트 모든 게임 오브젝트들 가져오기

        foreach (var root in roots)
        {
            _stageManager = root.GetComponentInChildren<StageManager>(true);
            if (_stageManager != null)
            {
                Logger.Log("Stage Manager 찾음");
                _stageManager.Init(this);
                return;
            }
            Logger.Log("Stage Manager 못 찾음");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2)) ChangeGameState(GameState.Start);
        if (Input.GetKeyDown(KeyCode.Alpha3)) ChangeGameState(GameState.Play);
        if (Input.GetKeyDown(KeyCode.Alpha4)) ChangeGameState(GameState.Stop);
        if (Input.GetKeyDown(KeyCode.Alpha5)) ChangeGameState(GameState.Dead);
        if (Input.GetKeyDown(KeyCode.Alpha6)) ChangeGameState(GameState.Clear);
        if (Input.GetKeyDown(KeyCode.Alpha7)) ChangeGameState(GameState.End);
    }

    public void StartGame()
    {
        // 스테이지 맵으로 전환하기
        // if 튜토리얼을 하지 않았다면 튜토리얼 스테이지로 아니라면 맵으로
        ChangeGameState(GameState.Start);
    }

    // 전이 가능한 상태 지정
    private readonly Dictionary<GameState, GameState[]> _allowedTransitions = new()
    {
        { GameState.None,  new[] { GameState.Start } },
        { GameState.Start, new[] { GameState.Play, GameState.Stop } },
        { GameState.Play,  new[] { GameState.Stop, GameState.Clear, GameState.Dead } },
        { GameState.Stop,  new[] { GameState.Start, GameState.Play, GameState.End } },
        { GameState.Dead,  new[] { GameState.Start, GameState.End } },
        { GameState.Clear, new[] { GameState.End, GameState.Next } },
        { GameState.End,   new[] { GameState.None } }
    };

    public void ChangeGameState(GameState gameState)
    {
        if (CurrentGameState == gameState) return;  // 동일한 상태일 경우 스킵

        if (!_allowedTransitions.TryGetValue(CurrentGameState, out var allowedStates) ||
            Array.IndexOf(allowedStates, gameState) == -1)
        {
            Logger.LogWarning($"상태 변경 불가: {CurrentGameState} → {gameState}");
            return;
        }

        _currentGameState = gameState;
        Logger.Log($"상태 변경: {_currentGameState}");
        OnGameStateChanged?.Invoke(_currentGameState);
    }
}
