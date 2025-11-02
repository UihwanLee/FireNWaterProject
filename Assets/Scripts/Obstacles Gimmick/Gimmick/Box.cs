using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class Box : MonoBehaviour , InteractWithController
{
    private Box _box;
    //검수
    public void Activate(BaseController bc)
    {
        // 플레이어와 물리적 상호작용 
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var bt = other.GetComponent<Button>();
        if (bt != null)
        {
           bt.Activate(_box);
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
