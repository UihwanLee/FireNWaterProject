using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
                                                                                                                                             
public class BaseController : MonoBehaviour
{
    /// <summary>
    /// BaseController : 캐릭터 기본 BaseController
    /// </summary>

    protected Rigidbody2D _rigidbody;

    [Header("Character Stat")]
    [SerializeField] protected float minSpeed = 1f;          // 캐릭터 최소 속도
    [SerializeField] protected float maxSpeed = 5f;         // 캐릭터 최대 속도

    [SerializeField] private float acceleration = 30f;       // 캐릭터 가속도
    [SerializeField] private float deceleration = 0.5f;       // 캐릭터 감속도

    [Range(1f, 20f)][SerializeField] protected float jumpForce = 5f;        // 캐릭터 점프력
    protected Vector2 moveDirection = Vector2.zero;                         // 캐릭터 이동 방향

    [SerializeField] private bool isGrounded = false;                       // 캐릭터 Ground 체크 변수

    [Header("Character Component")]
    [SerializeField] protected SpriteRenderer characterRenderer;            // 캐릭터 Sprite Renderer

    private int groundLayer;                                                // Ground Layer

    protected void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
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

    #region 캐릭터 동작 처리

    /// <summary>
    /// 사용자 입력에 따른 동작처리
    /// 캐릭터마다 다른 입력을 받아 처리한다.
    /// </summary>
    protected virtual void HandleAction()
    {

    }

    /// <summary>
    /// 캐릭터 Move (좌, 우)
    /// </summary>
    protected virtual void Move()
    {
        bool isInput = moveDirection.x != 0f;             // 입력 체크
        bool isMovingLeft = (moveDirection.x < 0.0f);     // 방향 전환 체크

        // 1. 입력 받고 있는지 체크
        if (isInput)
        {
            float movePosX = moveDirection.x * acceleration;

            Vector2 moveVector = new Vector2(movePosX, _rigidbody.velocity.y);
            _rigidbody.AddForce(moveVector, ForceMode2D.Force);

            if (_rigidbody.velocity.x > maxSpeed)
            {
                _rigidbody.velocity = _rigidbody.velocity.normalized * maxSpeed;
            }
        }
        else
        {
            _rigidbody.velocity = Vector2.Lerp(_rigidbody.velocity, Vector2.zero, deceleration * Time.fixedDeltaTime);
        }
    }

    /// <summary>
    /// 캐릭터 Jump
    /// </summary>
    protected void Jump()
    {
        if(isGrounded)
        {
            // 캐릭터가 땅에 착지된 상태라면 점프
            _rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
        }
    }

    /// <summary>
    /// 캐릭터 Rotate (좌, 우)
    /// </summary>
    protected virtual void Rotate()
    {
        if (moveDirection.x != 0.0f)
        {
            // 캐릭터 이동 방향에 따른 flip 변경
            bool isMovingLeft = (moveDirection.x < 0.0f);
            characterRenderer.flipX = isMovingLeft;
        }
    }

    #endregion

    #region 캐릭터 상태 처리

    /// <summary>
    /// 캐릭터 Die
    /// </summary>
    public virtual void Death()
    {
        _rigidbody.velocity = Vector2.zero;

        // Death 로직 처리
        Debug.Log($"{gameObject.name}가 죽었습니다!");
    }

    #endregion

    // 프로퍼티
    public bool IsGrounded { get { return isGrounded; } set { isGrounded = value; } }
}
