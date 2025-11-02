using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour, InteractableObstacle
{
    /*
     * 길이는 2로 고정
     * 수평,수직인지 결정
     * 동작 시작점
     * 동작 종점
     * 가동 거리
     *
     *일단 신호 주고받는 구조 완성
     * 
     */
    
    [Range((float)1,2)][SerializeField] private float moveRange = 1.0f;
   
    private void Awake()
    {
        //박스 초기화
        moveRange = transform.localScale.x;
    }
    

    public void Interact(bool on)
    {
        Debug.Log("Interact with gate");
    }
}
