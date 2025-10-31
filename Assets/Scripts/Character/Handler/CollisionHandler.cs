using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    /*
     * 캐릭터에 대한 충돌 처리
     */

    private BaseController controller;
    private int groundLayer;

    private void Start()
    {
        controller = GetComponent<BaseController>();
        groundLayer = LayerMask.NameToLayer("Ground");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Ground Layer와 충돌 시 isGrounded 변경
        if (collision.gameObject.layer == groundLayer)
        {
            //controller.IsGrounded = true;
        }
    }
}
