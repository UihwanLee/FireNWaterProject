using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    private GameManager _gameManager;
    private Dictionary<int, StageController> _stages = new();
    private StageController _currentStage;


    private GameState _currentGameState = GameState.None;
    public GameState CurrentGameState => _currentGameState;

    public void Init(GameManager gameManager)
    {
        _gameManager = gameManager;

        var stages = GetComponentsInChildren<StageController>(true);    // 비활성화된 object도 탐색

        _stages.Clear();

        foreach (var stage in stages)
        {
            int id = stage.StageId;

            if (_stages.ContainsKey(id))
            {
                Debug.Log($"[StageManager.Init] {id} 번째 스테이지 중복 할당");
                continue;
            }

            _stages[id] = stage;                                        // dict 채우기

            stage.gameObject.SetActive(false);                          // 모두 비활성화 하기
        }

        Debug.Log($"[StageManager.Init] 등록된 Stage 수: {_stages.Count}");
    }

    public void SelectStage(int id)
    {
        if (!_stages.TryGetValue(id, out var stage))
        {
            Logger.Log($"{id} 번째 스테이지 존재하지 않음");
            return;
        }

        _currentStage = _stages[id];
        _currentStage.gameObject.SetActive(true);   // 활성화
        StartStage();                               // 자동 시작
    }

    private void StartStage()
    {
        _currentGameState = GameState.Play;
        _currentStage.StartStage();
    }

    public void PauseStage()
    {
        _currentGameState = GameState.Stop;
        _currentStage.PauseStage();
    }

    public void GameOver()
    {
        _currentGameState = GameState.Dead;
        _currentStage.GameOver();
    }

    public void RestartStage()
    {
        _currentStage.ResetStageInfo();
        _currentStage.StartStage();
    }

    public void ExitStage()
    {
        _currentGameState = GameState.None;
        _currentStage.ResetStageInfo();
        _currentStage.ExitStage();
        _currentStage.gameObject.SetActive(false);  // 비활성화
    }

    public void ClearStage()
    {
        _currentStage.ClearStage();
        _currentStage.CheckScore();
        _currentStage.gameObject.SetActive(false);  // 비활성화
        _currentGameState = GameState.None;
    }
}
