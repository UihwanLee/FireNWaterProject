using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    // Score Flags
    private bool _isStageCleared = false;
    private bool _isWithinTimeLimit = true;
    private bool _isAllGemsCollected = false;

    // Score
    private StageScore _currentStageScore = StageScore.None;
    public StageScore CurrentStageScore => _currentStageScore;

    // Current Stage Info
    private int _currentGemCount = 0;

    public event Func<int, bool> OnCheckGemCount;

    public void CheckStageScore()
    {
        if (!_isStageCleared)
        {
            Logger.Log("클리어 실패");
            return;
        }

        //  제한 시간 통과 & 모든 젬 획득
        if (_isWithinTimeLimit && _isAllGemsCollected)
        {
            _currentStageScore = StageScore.A;
        }
        // 제한 시간 미통과 & 모든 젬 획득 || 재한 시간 통과 & 모든 젬 미획득
        else if (_isWithinTimeLimit && !_isAllGemsCollected ||
            !_isWithinTimeLimit && _isAllGemsCollected)
        {
            _currentStageScore = StageScore.B;
        }
        else
        {
            _currentStageScore = StageScore.C;
        }
        Logger.Log($"클리어 등급: {_currentStageScore}");
    }

    public void ResetScoreFlags()
    {
        _isStageCleared = false;
        _isWithinTimeLimit = true;
        _isAllGemsCollected = false;
        _currentStageScore = StageScore.None;
        _currentGemCount = 0;
    }

    public void HandleTimeLimitFailed()
    {
        Logger.Log("제한 시간 오버");
        _isWithinTimeLimit = false;
    }

    public void AddGem()
    {
        _currentGemCount++;
        Logger.Log($"현재 젬 개수: {_currentGemCount}");

        if (OnCheckGemCount?.Invoke(_currentGemCount) ?? false)
        {
            Logger.Log("모든 젬 획득");
            _isAllGemsCollected = true;
        }
    }
}
