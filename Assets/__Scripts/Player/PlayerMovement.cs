using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private float forward, rotation;

    [SerializeField] private float forwardSpeed = 8.0f;
    [SerializeField] private float rotationSpeed = 5.0f;

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
        forward = forward * forwardSpeed;
        var force = new Vector2(0, forward);
        rb.AddRelativeForce(force); // Go in whatever direction the player is facing

        rotation = rotation * rotationSpeed;
        rb.AddTorque(-rotation);
    }
}
