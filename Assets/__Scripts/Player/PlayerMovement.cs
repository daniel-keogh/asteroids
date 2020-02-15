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
    [SerializeField] private float sceneHeight = 13.2f;
    [SerializeField] private float sceneWidth = 16.7f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update() {
        forward = Input.GetAxis("Vertical");
        rotation = Input.GetAxis("Horizontal");

        WrapAround();
    }

    void FixedUpdate()
    {
        rb.AddRelativeForce(new Vector2(0, forward) * forwardSpeed);
        rb.AddTorque(-rotation * rotationSpeed);
    }

    private void WrapAround()
    {
        var position = new Vector2(transform.position.x, transform.position.y);

        if (transform.position.y > sceneHeight || transform.position.y < -sceneHeight)
        {
            position.y = -transform.position.y;
        }
        if (transform.position.x > sceneWidth || transform.position.x < -sceneWidth)
        {
            position.x = -transform.position.x;
        }

        transform.position = position;
    }
}
