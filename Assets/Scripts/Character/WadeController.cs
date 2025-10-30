using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WadeController : BaseController
{
    /// <summary>
    /// WadeController ��ũ��Ʈ
    /// 
    /// Wade ĳ���ʹ� 2p ĳ���ͷ� ����Ű Ű �Է��� �޾� �̵�
    /// </summary>
    protected override void HandleAction()
    {
        // Wade ĳ���ʹ� �¿� �̵��� �Ѵ�.
        float horizontal = 0f;

        if (Input.GetKey(KeyCode.LeftArrow)) horizontal = -1f;
        else if (Input.GetKey(KeyCode.RightArrow)) horizontal = 1f;

        moveDirection = new Vector2(horizontal, 0f).normalized;

        // Ember ���� : WŰ �Է�
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Jump();
        }
    }
}
