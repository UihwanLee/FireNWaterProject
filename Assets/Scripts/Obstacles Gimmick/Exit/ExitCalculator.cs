using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitCalculator : MonoBehaviour
{
    //리스트 중복 방지
    private HashSet<BaseController> players = new();

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
        if (players.Count == 2) GameManager.Instance.ExitStage();
    }
    
    public void DebugPlayers()
    {
        foreach (var bc in players)
        {
            Debug.Log($"{bc.name}");
        }   
    }
    
}
