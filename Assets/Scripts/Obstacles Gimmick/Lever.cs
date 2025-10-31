using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour, InteractWithController
{
    [Header("각도 한계(참조각 기준)")]
    [SerializeField] private float maxAngle = 45f;   
    [SerializeField] private float minAngle = -45f;  
    [SerializeField] private float neutralAngle = 0f;
    
    [Header("회전 속도(보간 강도)")]
    [SerializeField] private float rotateSpeed = 6f; // 클수록 더 빨리 목표각으로 수렴
    
    [Header("노이즈 제어")]
    [SerializeField] private float velDeadZone = 0.1f; // 이 속도 이하일 땐 "정지"로 간주

    public float targetAngle;
    
    public void Activate(BaseController bc)
    {
        BoxCollider2D box = gameObject.GetComponent<BoxCollider2D>();
        box.isTrigger = true;
        
        Rigidbody2D rb2d = bc.GetComponent<Rigidbody2D>(); 
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
        transform.rotation = Quaternion.Euler(0f, 0f, nextZ);
        
    }
}
