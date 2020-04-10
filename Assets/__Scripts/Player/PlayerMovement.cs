using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Speed")]
    [SerializeField] private float forwardSpeed = 8.0f;
    [SerializeField] private float rotationSpeed = 5.0f;

    private Rigidbody2D rb;
    private float forward, rotation;
    private ParticleSystem thrusters;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        thrusters = GetComponentInChildren<ParticleSystem>();
    }

    void Update()
    {
        rotation = Input.GetAxis("Horizontal");
        forward = Input.GetAxis("Vertical");

        // Enable/Disable the thruster ParticleEffect depending on whether or
        // not the player is moving
        if (thrusters && forward > 0)
        {
            thrusters.Play();
        }
        else
        {
            thrusters.Stop();
        }
    }

    void FixedUpdate()
    {
        forward = forward * forwardSpeed;
        var force = new Vector2(0, forward);
        rb.AddRelativeForce(force); // Go in whatever direction the player is facing

        rotation = rotation * rotationSpeed;
        rb.AddTorque(-rotation);
    }

    private void OnDisable()
    {
        // Prevent any movement/rotation on respawn
        forward = 0;
        rotation = 0;

        if (thrusters)
        {
            // Disable the thruster particle effect
            thrusters.Stop();
        }
    }
}
