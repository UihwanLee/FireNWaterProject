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
    [SerializeField] private GameObject[] _stages;
    private GameObject _stage;


    private GameState _currentGameState = GameState.None;
    public GameState CurrentGameState => _currentGameState;

    public void Init(GameManager gameManager)
    {
        _gameManager = gameManager;
    }
}
