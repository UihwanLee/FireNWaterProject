using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour, InteractWithController
{
    [SerializeField] private float maxAngle = 45f;

    public void Activate(BaseController bc)
    {
        
        if (bc.transform.position.x < transform.position.x)
        {
            
           // transform.rotation = Quaternion.Euler(0,0,-maxAngle,);
        }

        if (bc.transform.position.x > transform.position.x)
        {
            //왼쪽으로 기울어지는 함수
        }

    }


    
        /*
         * basecontroller에서 레버 감지
         *
         * 레버 ~액션! -> Activate함수 켜짐
         *
         * Activate:
         *
         * 플레이어 충돌방향 판단
         * 
         * 레버 움직임 범위 설정
         * 
         * 레버 움직임 제어
         * 
         */
        
        
        
        
        
        
    
    
    
}
