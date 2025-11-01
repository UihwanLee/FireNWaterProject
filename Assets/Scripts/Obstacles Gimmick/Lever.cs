using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Lever : MonoBehaviour, InteractWithController
{
    [Header("각도 한계(참조각 기준)")]
    [SerializeField] private float maxAngle = 45f;    // 오른쪽(+)
    [SerializeField] private float minAngle = -45f;   // 왼쪽(-)
    [SerializeField] private float neutralAngle = 0f; // 중립

    [Header("회전/필터")]
    [SerializeField] private float rotateSpeed = 6f;
    [SerializeField] private float velDeadZone = 0.1f;
    [SerializeField] private float limitEpsilon = 1.0f;     // 한계각 근처에서의 여유

    public float targetAngle { get; private set; }

    private BaseController _currentPusher; // 현재 밀고 있는 플레이어
    private PushSide _latchedSide = PushSide.None;   // 트리거 진입 시 결정, Exit 전엔 불변
    private bool _atLimit;                           // 한계각 잠금 여부

    private enum PushSide { None, Left, Right }
    
    
    public void Activate(BaseController bc)
    {
        
        
    }
    
    public void OnPusherEnter(BaseController bc)
    {
        
    }

    public void OnPusherStay(BaseController bc)
    {
        
    }

    public void OnPusherExit(BaseController bc)
    {
        
    }


}




/*Rigidbody2D rb2d = bc.GetComponent<Rigidbody2D>();
       //플레이어 움직임 여부
       var velocityX = rb2d.velocity.x;
       bool isMoving = Mathf.Abs(velocityX) > velDeadZone;

       // 플레이어 푸시 방향 판단
       float direction = 0;
       //오른쪽으로 움직이면 레버의 위치를 오른쪽으로 더 보정
       if(rb2d.velocity.x >0) direction = bc.transform.position.x - (transform.position.x+1) ;
       //왼쪽으로 움직이면~
       if(rb2d.velocity.x <0) direction = bc.transform.position.x - (transform.position.x -1);

       bool leftPush = direction > 0;

       //목표 각도 설정
       if (isMoving)
       {
           bool pushRtoL = leftPush && velocityX < 0;
           bool pushLtoR = !leftPush && velocityX > 0;

           if (pushRtoL) targetAngle = maxAngle;
           else if (pushLtoR) targetAngle = minAngle;
           else targetAngle = neutralAngle;
       }

       //회전
       float currentZ = transform.eulerAngles.z;
       float nextZ = Mathf.LerpAngle(currentZ, targetAngle, Time.deltaTime * rotateSpeed);
       transform.rotation = Quaternion.Euler(0f, 0f, nextZ);*/