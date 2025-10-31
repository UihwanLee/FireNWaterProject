using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundAndSlopeHandler : MonoBehaviour
{
    // 캐릭터 Ground 체크와 경사면 처리 클래스

    [Header("Controller")]
    [SerializeField] private BaseController controller;                     // 캐릭터 Controller

    [Header("Collider")]
    [SerializeField] BoxCollider2D boxCollider2D;                           // 캐릭터 BoxCollider

    [Header("Ground")]
    [SerializeField] private float groundRayCastExtraHeight = 0.1f;         // Ground RayCast 추가 Height
                                                                            
    [Header("Slope")]
    [SerializeField] private float slopeRayCastExtraWidth = 0.1f;           // Slope RayCast 추가 Width


    /// <summary>
    /// Ground 체크는 Ray를 아래로 써서 판단
    /// </summary>
    public bool CheckGround()
    {
        float extraHeight = 0.1f;
        RaycastHit2D raycastHit = Physics2D.Raycast(boxCollider2D.bounds.center, Vector2.down, boxCollider2D.bounds.extents.y + extraHeight, Define.LayerMask.GROUND);

        Color rayColor;
        if (raycastHit.collider != null)
        {
            rayColor = Color.green;
        }
        else
        {
            rayColor = Color.red;
        }

        //Debug.DrawRay(boxCollider2D.bounds.center, Vector2.down * (boxCollider2D.bounds.extents.y + extraHeight), rayColor);
        
        return (raycastHit.collider != null);
    }

    /// <summary>
    /// Slope 체크는 Ray를 앞으로 써서 판단
    /// </summary>
    public bool CheckSlope()
    {
        // 현재 캐릭터가 어느 방향을 보고 있는지 체크
        Vector2 dir = (controller.MoveDirection.x > 0.0f) ? Vector2.right : Vector2.left;

        RaycastHit2D raycastHit = Physics2D.Raycast(boxCollider2D.bounds.center, Vector2.down, boxCollider2D.bounds.extents.x + slopeRayCastExtraWidth, Define.LayerMask.SLOPE);

        Vector2 normal = raycastHit.normal;
        Debug.DrawRay(raycastHit.point, normal, Color.green);

        Vector2 slopeDir = Vector2.Perpendicular(normal).normalized;

        Debug.DrawRay(raycastHit.point, slopeDir, Color.red);

        controller.slopeNormal = slopeDir;

        Color rayColor;
        if (raycastHit.collider != null)
            rayColor = Color.green;
        else
            rayColor = Color.red;

        //Debug.DrawRay(boxCollider2D.bounds.center, dir * (boxCollider2D.bounds.extents.x + slopeRayCastExtraWidth), rayColor);

        return (raycastHit.collider != null);
    }
}
