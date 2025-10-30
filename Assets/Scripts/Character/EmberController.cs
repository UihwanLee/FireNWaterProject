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
        float horizontal = Input.GetAxis("Horizontal");
        moveDirection = new Vector2(horizontal, 0f).normalized;

        // Ember ���� : WŰ �Է�
        if (Input.GetKeyDown(KeyCode.W))
        {
            Jump();
        }
    }
}
