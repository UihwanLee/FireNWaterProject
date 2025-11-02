using UnityEngine;



public class ScoreManager : MonoBehaviour
{
    private bool _isStageCleared = false;
    private bool _isWithinTimeLimit = true;
    private bool _isAllElementsCollected = false;
    private StageScore _stageScore = StageScore.None;

    public void CheckStageScore()
    {
        if (!_isStageCleared)
        {
            Logger.Log("클리어 실패");
            return;
        }

        //  제한 시간 통과 & 모든 젬 획득
        if (!_isWithinTimeLimit && _isAllElementsCollected)
        {
            _stageScore = StageScore.A;
        }
        // 제한 시간 미통과 & 모든 젬 획득 || 재한 시간 통과 & 모든 젬 미획득
        else if (_isWithinTimeLimit && _isAllElementsCollected ||
            !_isWithinTimeLimit && _isAllElementsCollected)
        {
            _stageScore = StageScore.B;
        }
        else
        {
            _stageScore = StageScore.C;
        }
    }

    public void ResetScoreFlags()
    {
        _isStageCleared = false;
        _isWithinTimeLimit = true;
        _isAllElementsCollected = false;
        _stageScore = StageScore.None;
    }

    public void HandleTimeLimitFailed()
    {
        Logger.Log("제한 시간 오버");
        _isWithinTimeLimit = false;
    }

    public void HandleAllElementsCollected()
    {
        Logger.Log("모든 젬 획득");
        _isAllElementsCollected = true;
    }
}
