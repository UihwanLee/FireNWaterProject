using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance => _instance;

    // 추후 매니저 추가 예정
    [Header("Managers")]
    [SerializeField] private ScoreManager _scoreManager;

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


    public void StartGame()
    {
        // 스테이지 맵으로 전환하기
        // if 튜토리얼을 하지 않았다면 튜토리얼 스테이지로 아니라면 맵으로
        ChangeGameState(GameState.Start);
    }

    // 전이 가능한 상태 지정
    private readonly Dictionary<GameState, GameState[]> _allowedTransitions = new()
    {
        { GameState.None,  new[] { GameState.Ready } },
        { GameState.Ready, new[] { GameState.Start, GameState.None } },
        { GameState.Start, new[] { GameState.Play, GameState.Stop } },
        { GameState.Play,  new[] { GameState.Stop, GameState.Clear, GameState.Dead } },
        { GameState.Stop,  new[] { GameState.Play, GameState.End } },
        { GameState.Dead,  new[] { GameState.End } },
        { GameState.Clear, new[] { GameState.End } },
        { GameState.End,   new[] { GameState.None, GameState.Ready } }
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
        OnGameStateChanged?.Invoke(_currentGameState);
        Logger.Log($"상태 변경: {_currentGameState}");
    }
}
