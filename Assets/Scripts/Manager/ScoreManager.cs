using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class StageClearInfoWrapper
{
    public List<StageClearInfo> StageClearInfos;

    public StageClearInfoWrapper()
    {
        StageClearInfos = new();
    }

    public StageClearInfoWrapper(int i)
    {
        StageClearInfos = new(i);
    }
}

public class ScoreManager : MonoBehaviour
{
    // Score Flags
    private bool _isStageCleared = false;
    private bool _isWithinTimeLimit = true;
    private bool _isAllGemsCollected = false;
    public bool IsStageCleared => _isStageCleared;
    public bool IsWithinTimeLimit => _isWithinTimeLimit;
    public bool IsAllGemsCollected => _isAllGemsCollected;

    // Score
    private StageScore _currentStageScore = StageScore.None;
    public StageScore CurrentStageScore => _currentStageScore;

    // Current Stage Info
    private int _currentGemCount = 0;

    public event Func<int, bool> OnCheckGemCount;

    // Stage 정보 저장
    private string SavePath;
    private StageClearInfoWrapper _saveData = new();

    // todo: 누적 젬 개수
    private int _fireGemCount;
    private int _waterGemCount;
    public int FireGemCount
    {
        get { return _fireGemCount; }
        private set
        {
            _fireGemCount = value;
            SaveFireGemCount();
        }
    }
    public int WaterGemCount
    {
        get { return _waterGemCount; }
        private set
        {
            _waterGemCount = value;
            SaveWaterGemCount();
        }
    }

    private void Awake()
    {
        SavePath = Path.Combine(Application.persistentDataPath, "SaveStageData.json");
        Load();
    }

    public void CheckStageScore()
    {
        _isStageCleared = true;
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
        else if (_isWithinTimeLimit || _isAllGemsCollected)
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
    #region 데이터 저장/로드
    public void SaveStageClearInfo(StageClearInfo newClearInfo)
    {
        // todo: 가장 높은 등급 / 점수 비교 -> 저장
        int id = newClearInfo.StageId;
        List<StageClearInfo> stageClearInfos = _saveData.StageClearInfos;

        if (id >= stageClearInfos.Count)
        {
            Logger.Log("Stage id Index 벗어남");
            return;
        }

        StageClearInfo stageClearInfo = stageClearInfos[id];

        // StageScore: A = 0 / B = 1 / C = 2 / None = 3
        // 점수가 더 낮거나, 클리어 시간이 길 경우 저장 x
        if (newClearInfo.StageScore > stageClearInfo.StageScore)
        {
            Logger.Log("등급 갱신 실패");
            return;
        }
        if (newClearInfo.StageScore == stageClearInfo.StageScore
            && newClearInfo.ClearTime >= stageClearInfo.ClearTime)
        {
            Logger.Log("클리어 시간 단축 실패");
            return;
        }

        Logger.Log("클리어 정보 저장\n " +
            $"[ id: {id}, " +
            $"stage score: {newClearInfo.StageScore}, " +
            $"clear time: {newClearInfo.ClearTime}");
        stageClearInfos[id] = newClearInfo;
        Save();
    }

    private void Save()
    {
        string json = JsonUtility.ToJson(_saveData, true);
        File.WriteAllText(SavePath, json);
    }

    private void Load()
    {
        if (File.Exists(SavePath))
        {
            string json = File.ReadAllText(SavePath);
            _saveData = JsonUtility.FromJson<StageClearInfoWrapper>(json) ??
                new StageClearInfoWrapper(GameManager.STAGE_NUM);
        }
        else
        {
            _saveData = new StageClearInfoWrapper(GameManager.STAGE_NUM);
        }

        for (int i = _saveData.StageClearInfos.Count; i < GameManager.STAGE_NUM; i++)
        {
            _saveData.StageClearInfos.Add(new StageClearInfo(i));
        }
    }

    public List<StageClearInfo> GetSaveData() => _saveData.StageClearInfos;

    public void ResetData()
    {
        if (File.Exists(SavePath)) File.Delete(SavePath);
    }
    #endregion

    #region 젬 관련 메서드
    public void AddWaterGem()
    {
        WaterGemCount++;
        Logger.Log($"현재 Water 젬 개수: {WaterGemCount}");
        AddGem();
    }

    public void AddFireGem()
    {
        FireGemCount++;
        Logger.Log($"현재 Fire 젬 개수: {FireGemCount}");
        AddGem();
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

    public void UseWaterGem(int count)
    {
        WaterGemCount -= count;
    }

    public void UseFireGem(int count)
    {
        FireGemCount -= count;
    }

    private readonly string WaterGemKey = "WaterGem";
    private readonly string FireGemKey = "FireGem";

    private void SaveWaterGemCount()
    {
        PlayerPrefs.SetInt(WaterGemKey, WaterGemCount);
    }

    private void SaveFireGemCount()
    {
        PlayerPrefs.SetInt(FireGemKey, FireGemCount);
    }
    #endregion
}
