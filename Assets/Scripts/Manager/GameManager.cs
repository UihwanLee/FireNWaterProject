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
    [SerializeField] private AudioManager _audioManager;
    private StageManager _stageManager;

    [Header("Setting Window")]
    public GameObject SettingWindow;
    [SerializeField] private UnityEngine.UI.Button _homeButton;
    [SerializeField] private UnityEngine.UI.Button _retryButton;

    public int MaxClearStageId => _scoreManager.MaxClearStageId;

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

        SettingWindow.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (SettingWindow.activeSelf)
                CloseSettingWindow();
            else
                OpenSettingWindow();
        }
    }

    #region setting UI
    private void OpenSettingWindow()
    {
        SettingWindow.SetActive(true);
        if (_stageManager != null) PauseStage();
    }

    private void CloseSettingWindow()
    {
        SettingWindow.SetActive(false);
        if (_stageManager != null) ResumeStage();
    }

    private void SetActiveSettingUIButtons(bool active)
    {
        _homeButton.gameObject.SetActive(active);
        _retryButton.gameObject.SetActive(active);
    }
    #endregion

    #region stage scene 로드
    public void LoadStageScene()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        //SceneManager.LoadScene("StageScene");
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
        _stageManager.OnClearStage += HandleStageClear;

        // button 초기화
        _homeButton.onClick.AddListener(ExitStage);
        _retryButton.onClick.AddListener(StartStage);

        //int stageId = 0;
        //Logger.Log($"{stageId}번째 스테이지 선택");
        //SelectStage(stageId);
    }
    #endregion

    #region Stage 상태 관리 메서드
    public void SelectStage(int id)
    {
        if (id > MaxClearStageId + 1)
        {
            Logger.Log("스테이지 입장 불가");
            return;
        }

        _stageManager.SelectStage(id);
        Logger.Log("젬 확인 델리게이트 추가");
        _scoreManager.OnCheckGemCount += _stageManager.HandleCheckGemCount;

        _audioManager.PlayClip(Define.SFX_SELECT);
        _audioManager.ChangeBackGroundMusic(Define.BGM_INPLAY);
        SetActiveSettingUIButtons(true);
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
        _scoreManager.CheckStageScore();
        _stageManager.ChangeGameState(GameState.Clear);
        _audioManager.PlayClip(Define.SFX_WIN);
    }

    public void StartNextStage()
    {
        _stageManager.ChangeGameState(GameState.Next);
        _scoreManager.ResetScoreFlags();
        //Logger.Log("젬 확인 델리게이트 제거");
        //_scoreManager.OnCheckGemCount -= _stageManager.HandleCheckGemCount;
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

        _audioManager.ChangeBackGroundMusic(Define.BGM_INTRO);
        SetActiveSettingUIButtons(false);
    }

    private void HandleStageClear()
    {
        StageClearInfo stageClearInfo = _stageManager.GetStageClearInfo();
        stageClearInfo.StageScore = _scoreManager.CurrentStageScore;
        _scoreManager.SaveStageClearInfo(stageClearInfo);
    }
    #endregion

    #region 점수 관련 메서드
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

    /// <summary>
    /// 스테이지 클리어 정보 삭제
    /// </summary>
    public void ResetData()
    {
        _scoreManager.ResetData();
    }

    /// <summary>
    /// 스테이지 타이머
    /// </summary>
    /// <returns></returns>
    public float GetTimer() => _stageManager.Timer;
    #endregion

    #region 젬 관련 메서드
    public void AddFireGem()
    {
        _scoreManager.AddFireGem();
    }

    public void AddWaterGem()
    {
        _scoreManager.AddWaterGem();
    }

    public int GetFireGemCount()
    {
        return _scoreManager.TotalFireGemCount;
    }

    public int GetWaterGemCount()
    {
        return _scoreManager.TotalWaterGemCount;
    }

    public void UseFireGem(int count)
    {
        _scoreManager.UseFireGem(count);
    }

    public void UseWaterGem(int count)
    {
        _scoreManager.UseWaterGem(count);
    }
    #endregion
}
