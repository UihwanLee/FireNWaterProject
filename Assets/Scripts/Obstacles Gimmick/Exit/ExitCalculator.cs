using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitCalculator : MonoBehaviour
{
    //리스트 중복 방지
    private HashSet<BaseController> players = new();
    private bool cleared;
    
    public void AddPlayer(BaseController player)
    {
        players.Add(player);
    }

    public void RemovePlayer(BaseController player)
    {
        players.Remove(player);
    }
    
    public void IsStageOver()
    {
        if (cleared) return;            // 이미 끝났으면 무시
        if (players.Count >= 2)
        {
            cleared = true;
            GameManager.Instance.ClearStage();  // 딱 한 번만 호출
        }
    }
    
    public void DebugPlayers()
    {
        foreach (var bc in players)
        {
            Debug.Log($"{bc.name}");
        }   
    }
    
}
