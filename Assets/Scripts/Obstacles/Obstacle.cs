using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObstacleType
{
    Lava,
    Water,
    Poison
}


public class Obstacle : MonoBehaviour
{
    [SerializeField] private ObstacleType type = ObstacleType.Lava;
    private void OnTriggerEnter2D(Collider2D other)
    {
        Temp_Player player = other.GetComponent<Temp_Player>();

        bool DeadCondition = false;
        
        if (other.CompareTag("Player"))
        {
            //플레이어 타입 검사 
            if( type == ObstacleType.Poison || (type == ObstacleType.Lava && player.type  == PlayerType.Water) 
               || (type == ObstacleType.Water && player.type == PlayerType.Fire)) DeadCondition = true;
        }

        if (DeadCondition)
        {
            //playerdead함수 호출
            player.PlayerDead();
        }
        
        
    }
}
