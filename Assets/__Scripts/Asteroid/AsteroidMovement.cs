using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class AsteroidMovement : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField] private float rotation;
    [SerializeField] private float speed;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Add initial movement and rotation
        var force = new Vector2(0, speed);
        rb.AddForce(force);
        rb.AddTorque(rotation);
    }
}
