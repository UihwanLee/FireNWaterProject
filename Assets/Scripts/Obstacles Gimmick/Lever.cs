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

    float targetAngle;
    
    public void Activate(BaseController bc)
    {
        Rigidbody2D rb2d = bc.GetComponent<Rigidbody2D>();
        //플레이어 움직임 여부
        var velocityX = rb2d.velocity.x;
        bool isMoving = Mathf.Abs(velocityX) > 0.05f;
        
        // 플레이어 푸시 방향 판단
        var direction = transform.position.x - bc.transform.position.x;
        bool leftMove = direction < 0;
        
        //목표 각도 설정
        if (isMoving)
        {
            bool pushRtoL = !leftMove && velocityX < 0;
            bool pushLtoP = leftMove && velocityX > 0;

            if (pushRtoL) targetAngle = minAngle;
            else if (pushLtoP) targetAngle = maxAngle;
            else targetAngle = neutralAngle;
        }

        //회전
        float currentZ = transform.eulerAngles.z;
        float nextZ = Mathf.LerpAngle(currentZ, targetAngle, Time.deltaTime * rotateSpeed);
        transform.rotation = Quaternion.Euler(0f, 0f, nextZ);
        
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
         * 레버 움직임 종료 판정
         * 
         */
        
        
        
        
        
        
    
    
    
}
