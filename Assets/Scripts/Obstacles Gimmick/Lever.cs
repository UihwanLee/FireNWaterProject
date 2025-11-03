using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Lever : MonoBehaviour, InteractWithController
{
    [Header("각도 한계(참조각 기준)")]
    [SerializeField] private float neutralAngle = 0f; // 기준 각
    [SerializeField] private float maxAngle = 45f;    // 오른쪽(+)
    [SerializeField] private float minAngle = -45f;   // 왼쪽(-)

    [Header("회전/필터")]
    [SerializeField] private float rotateSpeed = 6f;
    [SerializeField] private float velDeadZone = 0.1f;

    public Gate targetGate; // 타겟 게이트
    private BaseController _currentPusher; // 현재 밀고 있는 플레이어
    private PushSide _pushSide = PushSide.None;   // 트리거 진입 시 결정, Exit 전엔 불변
    private float _currentAngle;
    private float targetAngle;
    
    private bool _gateOn; // 게이트 신호
    
    private enum PushSide { None, Left, Right }

    


    //플레이어의 Push 방향을 확정.
    private PushSide GetSide(BaseController bc)
    {
        var rb2d = bc.GetComponent<Rigidbody2D>();
        
        float pos = bc.transform.position.x - transform.position.x;
        bool moveLeft = rb2d.velocity.x < 0;
        bool moveRight = rb2d.velocity.x > 0;

        PushSide side = PushSide.None;
        if(moveLeft&& pos>0) side = PushSide.Left;
        else if(moveRight&& pos<0) side = PushSide.Right;
        return side;
    }
    
    public void OnPusherEnter(BaseController bc)
    {
        //pusher & 방향 확정
        _currentPusher = bc;
        _pushSide = GetSide(_currentPusher);
        _currentAngle = transform.eulerAngles.z;
    }
    
    public void Activate(BaseController bc)
    {
        
        //플레이어가 움직임 여부 검사
        var rb2d = bc.GetComponent<Rigidbody2D>();
        float vx = rb2d.velocity.x;
        bool  isMoving = Mathf.Abs(vx) > velDeadZone;
        
        //한계각 도달시 일단 정지
        if (_pushSide == PushSide.Left && Mathf.Approximately(_currentAngle, minAngle)&& isMoving)
        {
            transform.rotation = Quaternion.Euler(0, 0, _currentAngle);   
        }
        else if (_pushSide == PushSide.Right&& Mathf.Approximately(_currentAngle, maxAngle) && isMoving)
        {
            transform.rotation = Quaternion.Euler(0, 0, _currentAngle);   
        }
        
        //목표 각도 설정
        if (isMoving)
        {
            if (_pushSide == PushSide.Left) targetAngle = minAngle;
            else if (_pushSide == PushSide.Right) targetAngle = maxAngle;
        }
        
        //회전
        float currentZ = transform.eulerAngles.z;
        float nextZ = Mathf.LerpAngle(currentZ, targetAngle*-1, Time.deltaTime * rotateSpeed);
        transform.rotation = Quaternion.Euler(0f, 0f, nextZ);

        //게이트 가동
        float a = Mathf.DeltaAngle(neutralAngle, currentZ);
        float onThreshold = maxAngle*0.6f;
        float offThreshold = minAngle*0.6f;
        if (!_gateOn && a > onThreshold)
        {
            _gateOn = true;
            targetGate.Interact(true);   
        }
        else if (_gateOn && a < offThreshold)
        {
            _gateOn = false;
            targetGate.Interact(false);  
        }
 
    }

    public void OnPusherExit(BaseController bc)
    {
        //초기화
        _currentPusher = null;
        _pushSide = PushSide.None;
    }
    
    
    
    
}




