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


    // Obstacle Action 달라서 -> 이유 Interface의 이유 (Interface)

    // 핵심 Obstacle -> Player Layer 2개
    // Obstacle2 -> Player Layer 2개

    // Player -> Obstacle을 찾을 수 있는 Layer / 속성(인터페이스)를 가져와서 한번에 

    // 물/불

    // 다른 Obstcale

    public void DoSomething(BaseController player)
    {
        throw new NotImplementedException();

        player.Death();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Temp_Player player = other.GetComponent<Temp_Player>();

        bool DeadCondition = false;
        
        if (other.CompareTag("Player"))
        {
            //플레이어 타입 검사 
            if (type == ObstacleType.Poison || (type == ObstacleType.Lava && player.type == PlayerType.Water)
                                            || (type == ObstacleType.Water && player.type == PlayerType.Fire))
            {
                Debug.Log("Dead Conditon on");
                DeadCondition = true;
            }
        }

        if (DeadCondition)
        {
            //playerdead함수 호출
            player.PlayerDead();
        }
        
        
    }
}
