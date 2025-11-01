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

        _stageManager.SelectStage(1);
    }

    #region Stage 관련 메서드
    public void SelectStage(int id)
    {
        _stageManager.SelectStage(id);
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
    }

    public void StartNextStage()
    {
        _stageManager.ChangeGameState(GameState.Next);
    }

    public void RetryStage()
    {
        _stageManager.ChangeGameState(GameState.Start);
    }

    public void ExitStage()
    {
        _stageManager.ChangeGameState(GameState.Exit);
    }
    #endregion
}
