using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class Box : MonoBehaviour , InteractWithController
{
    public void Activate(BaseController bc)
    {
        // 플레이어와 물리적 상호작용 
    }

    public void Interact()
    {
        
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        var bt = other.GetComponent<Button>();
        if (bt != null)
        {
            bt.Interact();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var bt = other.GetComponent<Button>();
        if (bt != null)
        {
            bt.Recovery();
        }    }
}
