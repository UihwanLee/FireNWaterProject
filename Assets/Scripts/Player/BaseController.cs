using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BaseController : MonoBehaviour
{
    /// <summary>
    /// BaseController : 캐릭터 기본 움직임
    /// </summary>

    private Rigidbody2D _rigidbody;

    [Header("Character Stat")]
    [Range(1f, 20f)][SerializeField] private float moveSpeed = 5f;      // 캐릭터 이동 속도
    private Vector2 moveDirection = Vector2.zero;                       // 캐릭터 이동 방향 (좌, 우)

    [Header("Character Component")]
    [SerializeField] private SpriteRenderer characterRenderer;          // 캐릭터 Sprite Renderer

    protected void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
    }

    protected virtual void FixedUpdate()
    {

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
    protected virtual void Move(Vector2 moveDirection)
    {

    }

    /// <summary>
    /// 방향에 따라 Rotate (좌, 우)
    /// </summary>
    protected virtual void Rotate(Vector2 moveDirection)
    {

    }
}
