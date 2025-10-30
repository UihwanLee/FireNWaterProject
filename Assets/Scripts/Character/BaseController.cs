using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BaseController : MonoBehaviour
{
    /// <summary>
    /// BaseController : 캐릭터 기본 움직임
    /// </summary>

    protected Rigidbody2D _rigidbody;

    [Header("Character Stat")]
    [Range(1f, 20f)][SerializeField] protected float moveSpeed = 5f;        // 캐릭터 이동 속도
    [Range(1f, 20f)][SerializeField] protected float jumpForce = 5f;        // 캐릭터 점프력
    protected Vector2 moveDirection = Vector2.zero;                         // 캐릭터 이동 방향 (좌, 우)

    [SerializeField] private bool isGrounded = false;                       // 캐릭터 Ground 판단 변수

    [Header("Character Component")]
    [SerializeField] protected SpriteRenderer characterRenderer;            // 캐릭터 Sprite Renderer

    private int groundLayer;                                                // Ground Layer

    protected void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        groundLayer = LayerMask.NameToLayer("Ground");
        isGrounded = false;
    }

    protected virtual void Update()
    {
        HandleAction();
        Rotate();
    }

    protected virtual void FixedUpdate()
    {
        Move();
    }

    /// <summary>
    /// 입력을 받아서 동작을 처리하는 함수
    /// 캐릭터마다 다르게 동작
    /// </summary>
    protected virtual void HandleAction()
    {

    }

    /// <summary>
    /// 방향에 따라 Move (좌, 우)
    /// </summary>
    protected virtual void Move()
    {
        // 좌 우 이동만 설정
        _rigidbody.velocity = new Vector2(moveDirection.x * moveSpeed, _rigidbody.velocity.y);
    }

    /// <summary>
    /// 점프 기능
    /// </summary>
    protected void Jump()
    {
        if(isGrounded)
        {
            // 땅에 착지된 상태라면 점프
            _rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
        }
    }

    /// <summary>
    /// 방향에 따라 Rotate (좌, 우)
    /// </summary>
    protected virtual void Rotate()
    {
        if (moveDirection.x != 0.0f)
        {
            // 움직임이 있을 때 flip 변환
            bool isMovingLeft = (moveDirection.x < 0.0f);
            characterRenderer.flipX = isMovingLeft;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 땅과 접촉하면 isGrounded 설정
        if (collision.gameObject.layer == groundLayer)
        {
            isGrounded = true;
        }
    }
}
