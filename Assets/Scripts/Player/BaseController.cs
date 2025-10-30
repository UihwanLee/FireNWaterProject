using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BaseController : MonoBehaviour
{
    /// <summary>
    /// BaseController : ĳ���� �⺻ ������
    /// </summary>

    private Rigidbody2D _rigidbody;

    [Header("Character Stat")]
    [Range(1f, 20f)][SerializeField] private float moveSpeed = 5f;      // ĳ���� �̵� �ӵ�
    private Vector2 moveDirection = Vector2.zero;                       // ĳ���� �̵� ���� (��, ��)

    [Header("Character Component")]
    [SerializeField] private SpriteRenderer characterRenderer;          // ĳ���� Sprite Renderer

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
    /// �Է��� �޾Ƽ� ������ ó���ϴ� �Լ�
    /// ĳ���͸��� �ٸ��� ����
    /// </summary>
    protected virtual void HandleAction()
    {

    }

    /// <summary>
    /// ���⿡ ���� Move (��, ��)
    /// </summary>
    protected virtual void Move(Vector2 moveDirection)
    {

    }

    /// <summary>
    /// ���⿡ ���� Rotate (��, ��)
    /// </summary>
    protected virtual void Rotate(Vector2 moveDirection)
    {

    }
}
