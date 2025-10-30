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

    // �߱� manager �߰� ����
    [Header("Managers")]
    [SerializeField] private ScoreManager _scoreManager;

    private GameState _currentGameState = GameState.None;
    public GameState CurrentGameState => _currentGameState;


    private void Awake()
    {
        // �Ŵ��� �ʱ�ȭ (�� ��ȯ �� �ߺ� ������ ���� �ʱ�ȭ & �� ��ȯ �� ���� x)
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
