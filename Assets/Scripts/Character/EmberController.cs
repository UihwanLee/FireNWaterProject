using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmberController : BaseController
{
    /// <summary>
    /// EmberController 스크립트
    /// 
    /// Ember 캐릭터는 1p 캐릭터로 W,A,S,D 키 입력을 받아 이동
    /// </summary>
    protected override void HandleAction()
    {
        // Ember 캐릭터는 좌우 이동만 한다.
        float horizontal = 0f;
        
        if (Input.GetKey(KeyCode.A)) horizontal = -1f;
        else if (Input.GetKey(KeyCode.D)) horizontal = 1f;

        moveDirection = new Vector2(horizontal, 0f).normalized;

        // Ember 점프 : W키 입력
        if (Input.GetKeyDown(KeyCode.W))
        {
            Jump();
        }
    }
}
