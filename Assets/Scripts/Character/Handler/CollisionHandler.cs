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
        Watergem waterGem = other.GetComponent<Watergem>();
        Lever lever = other.GetComponent<Lever>();
        Button button = other.GetComponent<Button>();
        ExitEmber  exitEmber = other.GetComponent<ExitEmber>();
        
        //장애물 판정
        if(poison != null) poison.Activate(controller);
        if (lava != null)
        {
            if(controller.gameObject.layer == LayerMask.NameToLayer("Wade")) lava.Activate(controller);
        }
        if (water != null)
        {
            if(controller.gameObject.layer == LayerMask.NameToLayer("Ember")) water.Activate(controller);
        }

        //보석 판정
        if (fireGem != null)
        {
            if(controller.gameObject.layer == LayerMask.NameToLayer("Ember")) fireGem.Activate(controller);
        }
        if (waterGem != null)
        {
            if(controller.gameObject.layer == LayerMask.NameToLayer("Wade")) waterGem.Activate(controller);
        }
        
        if (lever != null) lever.OnPusherEnter(controller);
         
        //버튼 판정
        if (button != null)
        {
            button.RegisterPusher(gameObject);
        }

        if (exitEmber != null)
        {
            exitEmber.Activate(controller);
        }
        
        
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        Lever lever = other.GetComponent<Lever>();

        //레버 판정
        if (lever != null) lever.Activate(controller);
        
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        Lever lever = other.GetComponent<Lever>();
        Button button = other.GetComponent<Button>();        
        ExitEmber exitEmber = other.GetComponent<ExitEmber>();
        //레버 판정
        if (lever != null) lever.OnPusherExit(controller);
        
        //버튼 판정
        //if (button != null) button.Recovery();
        if (button != null) button.UnregisterPusher(gameObject);

        if (exitEmber != null)
        {
            exitEmber.Deactivate();
        }
    }
}
