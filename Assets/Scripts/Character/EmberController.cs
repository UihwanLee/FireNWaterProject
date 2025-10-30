using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmberController : BaseController
{
    /// <summary>
    /// EmberController ??????
    /// 
    /// Ember ��????? 1p ��????? W,A,S,D ? ????? ??? ???
    /// </summary>
    protected override void HandleAction()
    {
        // Ember ��????? ?��? ????? ???.
        float horizontal = 0f;
        
        if (Input.GetKey(KeyCode.A)) horizontal = -1f;
        else if (Input.GetKey(KeyCode.D)) horizontal = 1f;

        moveDirection = new Vector2(horizontal, 0f).normalized;

        // Ember ???? : W? ???
        if (Input.GetKeyDown(KeyCode.W))
        {
            Jump();
        }
    }
}
