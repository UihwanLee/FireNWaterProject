using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour, InteractableObstacle
{
    [Range((float)0.5,2)][SerializeField] private float Scale = 1.0f;
    [SerializeField] private float moveSpeed = 2.0f;
    
    private float moveRange;
    private void Awake()
    {
        transform.localScale = new Vector3(Scale, 1f, 1f);
        moveRange = transform.localScale.x;
    }


    public void Interact()
    {
      Vector3 currentPos =  transform.position;
      currentPos.x += moveRange;
      transform.position = currentPos;
    }
}
