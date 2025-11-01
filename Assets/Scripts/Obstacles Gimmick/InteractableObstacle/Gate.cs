using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour, InteractableObstacle
{
    /*
     * 좌우 방
     */
    
    [Range((float)1,2)][SerializeField] private float moveRange = 1.0f;
   
    private void Awake()
    {
        moveRange = transform.localScale.x;
    }
    

    public void Interact(bool on)
    {
        Debug.Log("Interact with gate");
    }
}
