using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    private GameManager _gameManager;
    private Dictionary<int, StageController> _stages = new();
    private StageController _currentStage;

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

        _gameManager.OnGameStateChanged += HandleStateChanged;
        Debug.Log($"[StageManager.Init] 등록된 Stage 수: {_stages.Count}");
    }

    public void SelectStage(int id)
    {
        if (!_stages.TryGetValue(id, out var stage))
        {
            Logger.LogWarning($"{id} 번째 스테이지 존재하지 않음");
            return;
        }

        _currentStage = _stages[id];
        _currentStage.gameObject.SetActive(true);               // 활성화
        _gameManager.ChangeGameState(GameState.Start);          // 자동 시작
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
            case GameState.Ready:                   // 맵 로드
                _currentStage.ResetStageInfo();
                break;
            case GameState.Start:                   // 카운트 다운, 로딩 등
                StartStage();
                break;
            case GameState.Play:                    // 실제 플레이(조작, 점수/시간 측정)
                break;
            case GameState.Stop:                    // 조작 불가, 시간 멈춤, 메뉴 표시
                PauseStage();
                break;
            case GameState.Dead:                    // 실패, 재시작 대기
                GameOver();
                break;
            case GameState.Clear:                   // 성공, 점수 계산
                ClearStage();
                _currentStage.CheckScore();
                break;
            case GameState.End:                     // 맵으로 나가기
                ExitStage();
                break;
            case GameState.Next:                    // 다음 스테이지
                NextStage();
                break;
            case GameState.None:
                break;
            default:
                break;
        }
    }

    private void StartStage()
    {
        _currentStage.ResetStageInfo();
        _currentStage.StartStage();
    }

    public void PauseStage()
    {
        _currentStage.PauseStage();
    }

    public void GameOver()
    {
        _currentStage.GameOver();
    }

    public void ExitStage()
    {
        _currentStage.ResetStageInfo();
        _currentStage.ExitStage();
        _currentStage.gameObject.SetActive(false);  // 비활성화
        _currentStage = null;
    }

    public void ClearStage()
    {
        _currentStage.ClearStage();
        _currentStage.CheckScore();
        _currentStage.gameObject.SetActive(false);  // 비활성화
    }

    public void NextStage()
    {
        int id = _currentStage.StageId;
        ClearStage();
        SelectStage(id);
    }
}
