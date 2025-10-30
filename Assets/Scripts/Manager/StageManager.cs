using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    None,
    Ready,
    Play,
    Dead,
    Stop,
}

public class StageManager : MonoBehaviour
{
    private GameManager _gameManager;
    private Dictionary<int, StageController> _stages = new();
    private StageController _currentStage = null;


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
}
