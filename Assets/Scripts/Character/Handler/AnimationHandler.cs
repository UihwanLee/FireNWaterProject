using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    private static readonly int IsMoving = Animator.StringToHash("IsMove");
    private static readonly int IsJumpingUp = Animator.StringToHash("IsJumpUp");
    private static readonly int IsFallingDown = Animator.StringToHash("IsFallDown");
    private static readonly int IsDie = Animator.StringToHash("IsDie");

    private Animator animator;

    private void Start()
    {
        this.animator = GetComponentInChildren<Animator>();
    }

    public void AnimationStopOrPlay(bool stop)
    {
        animator.speed = stop ? 0 : 1;
    }

    public void Move(bool isMove)
    {
        animator.SetBool(IsMoving, isMove);
    }

    public void JumpUp(bool isJumpUp)
    {
        animator.SetBool(IsJumpingUp, isJumpUp);
    }

    public void FallDown(bool isFallDown)
    {
        animator.SetBool(IsFallingDown, isFallDown);
    }

    public void Die(bool isDie)
    {
        animator.SetBool(IsDie, isDie);
    }
}
