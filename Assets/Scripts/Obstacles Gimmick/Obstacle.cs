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
        
        EmberController emberController = other.GetComponent<EmberController>();

        bool DeadCondition = false;
        
        if (other.CompareTag("Player"))
        {
            //플레이어 타입 검사 
            //플레이어에 enum type추가 후 주석 변경
           // if (type == ObstacleType.Poison || (type == ObstacleType.Lava && emberController.type == PlayerType.Water)
             //                               || (type == ObstacleType.Water && emberController.type == PlayerType.Fire))
            {
                Debug.Log("Dead Conditon on");
                DeadCondition = true;
            }
        }

        if (DeadCondition)
        {
            //playerdead함수 호출
            //emberController.PlayerDead();
        }
        
        
    }
}
