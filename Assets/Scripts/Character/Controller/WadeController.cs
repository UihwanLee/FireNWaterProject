using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WadeController : BaseController
{
    /// <summary>
    /// WadeController
    /// 
    /// Wade 캐릭터는 2p 플레이어로 방향키를 받는다
    /// </summary>
    protected override void HandleAction()
    {
        // Wade 이동: 좌, 우로만 움직인다.
        float horizontal = 0f;

        if (Input.GetKey(KeyCode.LeftArrow)) horizontal = -1f;
        else if (Input.GetKey(KeyCode.RightArrow)) horizontal = 1f;

        moveDirection = new Vector2(horizontal, 0f).normalized;

        // Wade 점프 : W키 입력
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Jump();
        }
    }
}
