using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class Box : MonoBehaviour
{

    [SerializeField] private float mass = 1.0f;

    private float boxspeed;

    private void Awake()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.mass = mass;


    }
}
