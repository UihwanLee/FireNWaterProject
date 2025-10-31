using UnityEngine;


public class StageController : MonoBehaviour
{
    [SerializeField] private Transform _spwanPoint;
    [SerializeField] private int _stageId;

    public int StageId => _stageId;

    private void Awake()
    {
        if (_spwanPoint == null)
        {
            transform.position = Vector3.zero;
        }
    }

    public void StartStage()
    {
        Logger.NotImpl();
    }

    public void PauseStage()
    {
        Logger.NotImpl();
    }

    public void ExitStage()
    {
        Logger.NotImpl();
    }

    public void GameOver()
    {
        Logger.NotImpl();
    }

    public void ResetStageInfo()
    {
        Logger.NotImpl();
    }

    public void ClearStage()
    {
        Logger.NotImpl();
    }

    public void CheckScore()
    {
        Logger.NotImpl();
    }
}
