using System;
using UnityEngine;

public enum CharacterState
{
    Idle,
    Move,
    JumpUp,
    FallDown,
    Pause,
    Die
}

public class BaseController : MonoBehaviour
{
    /// <summary>
    /// BaseController : 캐릭터 기본 BaseController
    /// </summary>

    protected Rigidbody2D _rigidbody;

    [Header("Character Stat")]
    [SerializeField] protected float minSpeed = Define.MIN_SPEED;           // 캐릭터 최소 속도
    [SerializeField] protected float maxSpeed = Define.MAX_SPEED;           // 캐릭터 최대 속도

    [SerializeField] private float acceleration = Define.ACCELERATION;      // 캐릭터 가속도
    [SerializeField] private float deceleration = Define.DECELERATION;      // 캐릭터 감속도

    [SerializeField] protected float jumpForce = Define.JUMPFORCE;          // 캐릭터 점프력
    
    protected Vector2 moveDirection = Vector2.zero;                         // 캐릭터 이동 방향
    public Vector2 slopeNormal = Vector2.zero;

    [SerializeField] private bool isGrounded = false;                       // 캐릭터 Ground 체크 변수
    [SerializeField] private bool isClimbed = false;                        // 캐릭터 Climbed 체크 변수
    private CharacterState currentState;                                    // 캐릭터 상태 변수

    [Header("Character Component")]
    [SerializeField] protected SpriteRenderer characterRenderer;            // 캐릭터 Sprite Renderer
    [SerializeField] protected AnimationHandler animationHandler;           // 캐릭터 Animation 담당 클래스
    [SerializeField] private GroundAndSlopeHandler groundHandler;           // 캐릭터 Ground/Slope 충돌 처리 클래스

    protected void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        isGrounded = false;
        currentState = CharacterState.Idle;
    }

    protected virtual void Update()
    {
        HandleAction();
        AnimationHandle();
        Rotate();
    }

    protected virtual void FixedUpdate()
    {
        Move();
        CheckGroundedOrClimb();
        SetFrictionIfClimb();
        CheckVelocityStateIsJumpingOrFall();
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
        if (currentState == CharacterState.Pause || currentState == CharacterState.Die) return;

        bool isInput = moveDirection.x != 0f;             // 입력 체크

        if (isInput)
        {
            if (isClimbed)
            {
                // 이동 벡터는 경사면의 평면벡터에서 가속도를 곱한다
                Vector2 moveVector =  slopeNormal * acceleration;
                
                // 이동벡터 만큼 rigidbody AddForce
                _rigidbody.AddForce(moveVector, ForceMode2D.Force);

                // velocity.x가 maxSpeed를 넘지 않도록 예외처리
                if (Mathf.Abs(_rigidbody.velocity.x) > maxSpeed)
                {
                    _rigidbody.velocity = _rigidbody.velocity.normalized * maxSpeed;
                }
            }
            else
            {
                // 지면 위에서는 좌우 방향 값에 가속도를 곱한다.
                float movePosX = moveDirection.x * acceleration;
                float movePosY = _rigidbody.velocity.y;

                // 이동 방향 만큼 AddForce
                Vector2 moveVector = new Vector2(movePosX, movePosY);
                _rigidbody.AddForce(moveVector, ForceMode2D.Force);

                // velocity.x가 maxSpeed를 넘지 않도록 예외처리
                if (_rigidbody.velocity.x > maxSpeed)
                {
                    _rigidbody.velocity = _rigidbody.velocity.normalized * maxSpeed;
                }
            }
        }
        else
        {
            // 이동 입력을 멈출 시 천천히 멈추도록 Lerp
            _rigidbody.velocity = Vector2.Lerp(_rigidbody.velocity, Vector2.zero, deceleration * Time.fixedDeltaTime);
        }
    }

    /// <summary>
    /// 캐릭터 Jump
    /// </summary>
    protected void Jump()
    {
        if (currentState == CharacterState.Pause || currentState == CharacterState.Die) return;
        if (isGrounded || isClimbed)
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
        if (currentState == CharacterState.Pause || currentState == CharacterState.Die) return;
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
    /// 캐릭터 상태 변경
    /// </summary>
    private void ChangeState(CharacterState changeState)
    {
        currentState = changeState;
    }

    /// <summary>
    /// 현재 캐릭터가 점프하고 있는 상태인지 떨어지고 있는 상태인지 체크
    /// </summary>
    private void CheckVelocityStateIsJumpingOrFall()
    {
        if (currentState == CharacterState.Pause || currentState == CharacterState.Die) return;

        float currentVelocityX = _rigidbody.velocity.x;
        float currentVelocityY = _rigidbody.velocity.y;

        if(currentVelocityX != 0.0f && currentVelocityY == 0.0f)
        {
            ChangeState(CharacterState.Move);
        }
        else if (currentVelocityY < -0.1f)
        {
            ChangeState(CharacterState.FallDown);
        }
        else if (currentVelocityY > 0.1f)
        {
            ChangeState(CharacterState.JumpUp);
        }
        else if(currentVelocityY == 0.0f)
        {
            ChangeState(CharacterState.Idle);
        }
    }

    /// <summary>
    /// Animation처리
    /// </summary>
    private void AnimationHandle()
    {
        bool isAnimationStop = (currentState == CharacterState.Pause || currentState == CharacterState.Die);
        animationHandler.AnimationStopOrPlay(isAnimationStop);

        animationHandler.Move((currentState == CharacterState.Move));
        animationHandler.JumpUp((currentState == CharacterState.JumpUp));
        animationHandler.FallDown((currentState == CharacterState.FallDown));
    }

    public event Action OnPlayerDied;

    /// <summary>
    /// 캐릭터 Die
    /// </summary>
    public virtual void Death()
    {
        _rigidbody.velocity = Vector2.zero;

        // Death 로직 처리
        Debug.Log($"{gameObject.name}가 죽었습니다!");

        OnPlayerDied?.Invoke();
        ChangeState(CharacterState.Die);
    }

    /// <summary>
    /// 캐릭터 멈춤
    /// </summary>
    public virtual void Pause()
    {
        _rigidbody.velocity = Vector2.zero;
        ChangeState(CharacterState.Pause);
        Logger.Log($"{gameObject.name} 일시 정지");
    }

    /// <summary>
    /// 캐릭터 멈춤 취소
    /// </summary>
    public virtual void CancelPause()
    {
        ChangeState(CharacterState.Idle);
        Logger.Log($"{gameObject.name} 일시 정지 취소");
    }

    /// <summary>
    /// 캐릭터 부활
    /// </summary>
    public virtual void Revive()
    {
        ChangeState(CharacterState.Idle);
        Logger.Log($"{gameObject.name} 부활");
    }
    #endregion

    #region 캐릭터 지면 충돌 처리: Ground / Climb Slope

    /// <summary>
    /// 캐릭터가 현재 지면에 있는지 경사면에 있는지 체크
    /// </summary>
    private void CheckGroundedOrClimb()
    {
        isGrounded = groundHandler.CheckGround();
        isClimbed = groundHandler.CheckSlope();
    }

    /// <summary>
    /// 캐릭터가 현재 경사면에 있다면 마찰을 줄이고 maxSpeed를 줄이기
    /// </summary>
    private void SetFrictionIfClimb()
    {
        if (isClimbed)
        {
            _rigidbody.gravityScale = Define.SLOPE_GRAVITY_SCALE;
            this.maxSpeed = Define.SLOPE_SPEED;
            this.acceleration = Define.SLOPE_ACCELERATION;
        }
        else
        {
            _rigidbody.gravityScale = Define.BASE_GRAVITY_SCALE;
            this.maxSpeed = Define.MAX_SPEED;
            this.acceleration = Define.ACCELERATION;
        }
    }

    #endregion

    // 프로퍼티
    public bool IsGrounded { get { return isGrounded; } set { isGrounded = value; } }
    public Vector2 MoveDirection { get { return moveDirection; } }
}
