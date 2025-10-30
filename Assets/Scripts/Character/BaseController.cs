using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BaseController : MonoBehaviour
{
    /// <summary>
    /// BaseController : ĳ���� �⺻ ������
    /// </summary>

    protected Rigidbody2D _rigidbody;

    [Header("Character Stat")]
    [Range(1f, 20f)][SerializeField] protected float moveSpeed = 5f;        // ĳ���� �̵� �ӵ�
    [Range(1f, 20f)][SerializeField] protected float jumpForce = 5f;        // ĳ���� ������
    protected Vector2 moveDirection = Vector2.zero;                         // ĳ���� �̵� ���� (��, ��)

    [SerializeField] private bool isGrounded = false;                       // ĳ���� Ground �Ǵ� ����

    [Header("Character Component")]
    [SerializeField] protected SpriteRenderer characterRenderer;            // ĳ���� Sprite Renderer

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
    /// �Է��� �޾Ƽ� ������ ó���ϴ� �Լ�
    /// ĳ���͸��� �ٸ��� ����
    /// </summary>
    protected virtual void HandleAction()
    {

    }

    /// <summary>
    /// ���⿡ ���� Move (��, ��)
    /// </summary>
    protected virtual void Move()
    {
        // �� �� �̵��� ����
        _rigidbody.velocity = new Vector2(moveDirection.x * moveSpeed, _rigidbody.velocity.y);
    }

    /// <summary>
    /// ���� ���
    /// </summary>
    protected void Jump()
    {
        if(isGrounded)
        {
            // ���� ������ ���¶�� ����
            _rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
        }
    }

    /// <summary>
    /// ���⿡ ���� Rotate (��, ��)
    /// </summary>
    protected virtual void Rotate()
    {
        if (moveDirection.x != 0.0f)
        {
            // �������� ���� �� flip ��ȯ
            bool isMovingLeft = (moveDirection.x < 0.0f);
            characterRenderer.flipX = isMovingLeft;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // ���� �����ϸ� isGrounded ����
        if (collision.gameObject.layer == groundLayer)
        {
            isGrounded = true;
        }
    }
}
