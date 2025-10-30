using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmberController : BaseController
{
    /// <summary>
    /// EmberController ��ũ��Ʈ
    /// 
    /// Ember ĳ���ʹ� 1p ĳ���ͷ� W,A,S,D Ű �Է��� �޾� �̵�
    /// </summary>
    protected override void HandleAction()
    {
        // Ember ĳ���ʹ� �¿� �̵��� �Ѵ�.
        float horizontal = 0f;
        
        if (Input.GetKey(KeyCode.A)) horizontal = -1f;
        else if (Input.GetKey(KeyCode.D)) horizontal = 1f;

        moveDirection = new Vector2(horizontal, 0f).normalized;

        // Ember ���� : WŰ �Է�
        if (Input.GetKeyDown(KeyCode.W))
        {
            Jump();
        }
    }
}
