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
        
        //래버 감지------------------------------밀 때 캐릭터 감속 구현
        if (lever != null) lever.OnPusherEnter(controller);
         
        //버튼 판정
        if (button != null)
        {
            //버튼밟을 시 감속 구현 ------------------------------------
            //button.Activate(controller);
            button.RegisterPusher(gameObject);
        }
        
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        Lever lever = other.GetComponent<Lever>();
        
        //레버 판정
        lever.Activate(controller);
        
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        Lever lever = other.GetComponent<Lever>();
        Button button = other.GetComponent<Button>();        
        //레버 판정
        if (lever != null) lever.OnPusherExit(controller);
        
        //버튼 판정
        //if (button != null) button.Recovery();
        if (button != null) button.UnregisterPusher(gameObject);
    }
}
