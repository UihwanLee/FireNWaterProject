using System;
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        Lava lava = other.GetComponent<Lava>();
        Water water = other.GetComponent<Water>();
        Poison poison = other.GetComponent<Poison>();
        FireGem fireGem = other.GetComponent<FireGem>();
        Watergem watergem = other.GetComponent<Watergem>();
        Lever lever = other.GetComponent<Lever>();
        Button button = other.GetComponent<Button>();
        
        //장애물 판정
        if(poison != null) poison.Activate(controller);
        if (lava != null)
        {
            controller.gameObject.layer = LayerMask.NameToLayer("Wade");
            lava.Activate(controller);
        }
        if (water != null)
        {
            controller.gameObject.layer = LayerMask.NameToLayer("Ember");
            water.Activate(controller);
        }

        //보석 판정
        if (fireGem != null)
        {
            controller.gameObject.layer = LayerMask.NameToLayer("Fire");
            fireGem.Activate(controller);
        }
        if (watergem != null)
        {
            controller.gameObject.layer = LayerMask.NameToLayer("Water");
            watergem.Activate(controller);
        }
        
        //버튼 판정
        if (button != null)
        {
            //버튼밟을 시 감속
            Rigidbody2D rb2d = controller.GetComponent<Rigidbody2D>();
            var currentVelocity = rb2d.velocity.magnitude;
            currentVelocity *= 0.5f;
            rb2d.velocity = rb2d.velocity.normalized * currentVelocity;
            Debug.Log("Velocity: " + rb2d.velocity);
            button.Activate(controller);
        }
        
        //래버 감지
        if (lever != null) lever.OnPusherEnter(controller);
            
        
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        Lever lever = other.GetComponent<Lever>();
        Button button = other.GetComponent<Button>();
        
        //레버 판정
        if (lever != null) lever.Activate(controller);
        
        //버튼 누름 판정
        if (button != null) button.Interact();
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        Lever lever = other.GetComponent<Lever>();
        Button button = other.GetComponent<Button>();
        
        //레버 판정
        if (lever != null) lever.OnPusherExit(controller);
        
        //버튼 판정
        if (button != null) button.Recovery(); 
    }
}
