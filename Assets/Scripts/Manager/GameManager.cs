using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance => _instance;
    public static readonly int STAGE_NUM = 5;

    // 추후 매니저 추가 예정
    [Header("Managers")]
    [SerializeField] private ScoreManager _scoreManager;
    private StageManager _stageManager;

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
        SceneManager.sceneLoaded += OnSceneLoaded;
        //SceneManager.LoadScene("StageScene");
    private void OnDisable()
    {
        Logger.Log("델리게이트 제거 (시간 제한, 스테이지 시작, 스테이지 클리어");
        _stageManager.OnFailedToClearWithinTimeLimit -= _scoreManager.HandleTimeLimitFailed;
        _stageManager.OnStartStage -= () =>
        {
            _scoreManager.ResetScoreFlags();
        };
        _stageManager.OnClearStage -= () =>
        {
            _scoreManager.CheckStageScore();
            _scoreManager.SaveStageClearInfo(_stageManager.GetStageClearInfo());
        };
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        Scene activeScene = SceneManager.GetActiveScene();          // 현재 활성화된 씬
        GameObject[] roots = activeScene.GetRootGameObjects();      // 루트 오브젝트 모든 게임 오브젝트들 가져오기

        foreach (var root in roots)
        {
            _stageManager = root.GetComponentInChildren<StageManager>(true);
            if (_stageManager != null)
            {
                Logger.Log("Stage Manager 찾음");
                _stageManager.Init();
                break;
            }
            Logger.Log("Stage Manager 못 찾음");
        }

        Logger.Log("델리게이트 추가 (시간 제한, 스테이지 시작, 스테이지 클리어");
        _stageManager.OnFailedToClearWithinTimeLimit += _scoreManager.HandleTimeLimitFailed;
        _stageManager.OnStartStage += () =>
        {
            _scoreManager.ResetScoreFlags();
        };
        _stageManager.OnClearStage += () =>
        {
            _scoreManager.CheckStageScore();
            _scoreManager.SaveStageClearInfo(_stageManager.GetStageClearInfo());
        };
        SelectStage(1);
    }

    #region Stage 상태 관리 메서드
    public void SelectStage(int id)
    {
        _stageManager.SelectStage(id);
        Logger.Log("젬 확인 델리게이트 추가");
        _scoreManager.OnCheckGemCount += _stageManager.HandleCheckGemCount;
    }

    public void StartStage()
    {
        _stageManager.ChangeGameState(GameState.Start);
    }

    public void PauseStage()
    {
        _stageManager.ChangeGameState(GameState.Pause);
    }

    public void ResumeStage()
    {
        _stageManager.ChangeGameState(GameState.Resume);
    }

    public void ClearStage()
    {
        _stageManager.ChangeGameState(GameState.Clear);
        _scoreManager.CheckStageScore();
    }

    public void StartNextStage()
    {
        _stageManager.ChangeGameState(GameState.Next);
        _scoreManager.ResetScoreFlags();
        Logger.Log("젬 확인 델리게이트 제거");
        _scoreManager.OnCheckGemCount -= _stageManager.HandleCheckGemCount;
    }

    public void RetryStage()
    {
        _stageManager.ChangeGameState(GameState.Start);
    }

    public void ExitStage()
    {
        _stageManager.ChangeGameState(GameState.Exit);
        Logger.Log("젬 확인 델리게이트 제거");
        _scoreManager.OnCheckGemCount -= _stageManager.HandleCheckGemCount;
    }
    #endregion

    #region 점수 관련 메서드
    public void AddGem()
    {
        _scoreManager.AddGem();
    }

    /// <summary>
    /// 현재 스테이지 클리어 점수 가져오기
    /// </summary>
    /// <returns></returns>
    public StageScore GetCurrentStageScore()
    {
        return _scoreManager.CurrentStageScore;
    }
    #endregion

    #region 데이터 저장/로드
    /// <summary>
    /// 스테이지 클리어 정보
    /// List의 index: stage id
    /// 클리어되지 않은 정보의 데이터 
    /// => (id: 스테이지 번호, clear time: 0, stage score: StageScore.None)
    /// </summary>
    /// <returns></returns>
    public List<StageClearInfo> GetStageClearData()
    {
        return _scoreManager.GetSaveData();
    }

    public void ResetData()
    {
        _scoreManager.ResetData();
    }
    #endregion
}
