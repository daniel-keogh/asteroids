using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private float forward, rotation;

    [SerializeField] private float forwardSpeed = 6.0f;
    [SerializeField] private float rotationSpeed = 6.0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        forward = Input.GetAxis("Vertical");
        rotation = Input.GetAxis("Horizontal");
    }

    void FixedUpdate()
    {
        rb.AddRelativeForce(new Vector2(0, forward) * forwardSpeed);
        rb.AddTorque(-rotation * rotationSpeed);
    }
}
