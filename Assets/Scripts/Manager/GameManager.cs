using UnityEngine;

public enum GameState
{
    None,
    Ready,
    Play,
    Dead,
    Stop,
}

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance => _instance;

    // 추구 manager 추가 예정
    [Header("Managers")]
    [SerializeField] private ScoreManager _scoreManager;

    private GameState _currentGameState = GameState.None;
    public GameState CurrentGameState => _currentGameState;


    private void Awake()
    {
        // 매니저 초기화 (씬 전환 시 중복 방지를 위한 초기화 & 씬 전환 시 삭제 x)
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

    }
}
