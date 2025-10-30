using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmberController : BaseController
{
    /// <summary>
    /// EmberController 
    /// 
    /// Ember 캐릭터는 1p 플레이어로 W,A,S,D 입력 키를 받는다
    /// </summary>
    protected override void HandleAction()
    {
        // Ember 이동: 좌, 우로만 움직인다.
        float horizontal = 0f;
        
        if (Input.GetKey(KeyCode.A)) horizontal = -1f;
        else if (Input.GetKey(KeyCode.D)) horizontal = 1f;

        Vector2 dir = new Vector2(horizontal, 0f).normalized;


        // Ember 점프 : W키 입력
        if (Input.GetKeyDown(KeyCode.W))
        {
            Jump();
        }

        if (moveDirection == dir)
        {
            return;
        }

        moveDirection = dir;
    }
}
