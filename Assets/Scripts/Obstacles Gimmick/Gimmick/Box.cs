using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class Box : MonoBehaviour, InteractWithController, IObstacle
{
    private Box _box;

    private Vector3 _originPosition;
    private Quaternion _originRotation;

    public void Init()
    {
        Logger.Log("box transfrom 초기화");
        gameObject.transform.position = _originPosition;
        gameObject.transform.rotation = _originRotation;
    }

    private void Awake()
    {
        _originPosition = gameObject.transform.position;
        _originRotation = gameObject.transform.rotation;
    }

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
           bt.RegisterPusher(gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var bt = other.GetComponent<Button>();
        
        if (bt != null)
        {
            bt.UnregisterPusher(gameObject);
        }
    }
}
