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
        Vector2 center = boxCollider2D.bounds.center;
        float halfHeight = boxCollider2D.bounds.extents.y;

        // 지면 체크 박스 영역 외부에 얼마나 더 늘릴 것인지
        // 값이 너무 크면 점프 시에도 땅에 있다고 판정을 내리므로 주의해야함
        float extraHeight = 0.1f;

        // 정해진 boxCollider 내에 아래로 RayCast를 쏜다.
        // 만약 hit된 collider layer가 Ground면 지면에 있다고 판정
        RaycastHit2D raycastHit = Physics2D.Raycast(center, Vector2.down, halfHeight + extraHeight, Define.LayerMask.GROUND);

        // 디버그 용 : Ground 체크 Ray Draw
        //DrawRay(raycastHit, center, Vector2.down * (halfHeight + extraHeight));

        return (raycastHit.collider != null);
    }

    /// <summary>
    /// Slope 체크는 Ray를 앞으로 써서 판단
    /// </summary>
    public bool CheckSlope()
    {
        // 현재 캐릭터가 어느 방향을 보고 있는지 체크
        Vector2 dir = (controller.MoveDirection.x > 0.0f) ? Vector2.right : Vector2.left;
        Vector2 center = boxCollider2D.bounds.center;
        float halfWidth = boxCollider2D.bounds.extents.x;
        float height = boxCollider2D.bounds.extents.y * 2 + 0.15f;

        // Ray를 나가는 위치를 x는 boxCollider 양끝으로 설정, y위치는 height 만큼
        Vector2 origin = new Vector2(center.x + (dir.x * halfWidth), center.y + height);
        Debug.DrawRay(origin, Vector2.down * 0.1f, Color.blue);

        // 정한 위치(origin)에서 법선 벡터를 구하기 위해 아래로 RayCast
        RaycastHit2D raycastHit = Physics2D.Raycast(origin, Vector2.down, 0.4f, Define.LayerMask.SLOPE);

        // 법선 벡터는 raycatsHit.normal로 구할 수 있음
        Vector2 normal = raycastHit.normal;
        Debug.DrawRay(raycastHit.point, normal, Color.green);

        //if(raycastHit.collider != null) Debug.Log(normal);

        // 평면 벡터 구하는 함수 Perpendicular를 이용하여 정규화하여 Controller에게 전달
        Vector2 slopeDir = Vector2.Perpendicular(normal).normalized;

        // Slop 방향이 캐릭터 이동 방향과 반대라면 Slope 방향을 반대로 바꿔줌
        if (controller.MoveDirection.x > 0 && slopeDir.x < 0)
            slopeDir *= -1;
        else if(controller.MoveDirection.x < 0 && slopeDir.x > 0)
            slopeDir *= -1;

        controller.slopeNormal = slopeDir;

        // 디버그 용 : Slope 체크 Ray Draw
        //DrawRay(raycastHit, origin, Vector2.down * 0.4f);

        return (raycastHit.collider != null);
    }

    private void DrawRay(RaycastHit2D rayCastHit2D, Vector2 origin, Vector2 dir)
    {
        Color rayColor;
        if (rayCastHit2D.collider != null)
            rayColor = Color.green;
        else
            rayColor = Color.red;

        Debug.DrawRay(origin, dir, rayColor);
    }
}
