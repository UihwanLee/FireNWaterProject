using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum JamType
{
    ForFire,
    ForWater,
}
public class Jem : MonoBehaviour
{
    [SerializeField] private JamType type =  JamType.ForFire;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!other.CompareTag("Player")) return;
        BaseController bc = other.GetComponent<BaseController>();
        
        /*타입 검사
        if(bc.type == PlayerType.Fire && type ==  JamType.ForFire)
        {
            jam 사라지고
            점수 시스템에 신호
        }
        if(bc.type == PlayerType.water && type == JamType.ForWater)
        {
            jam 사라지고
           점수 시스템에 신호
        }
        */

    }
}
