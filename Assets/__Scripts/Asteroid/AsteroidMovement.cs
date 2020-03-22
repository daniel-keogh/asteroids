using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class AsteroidMovement : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField] private float maxRotation;
    [SerializeField] private float speed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        rb.AddTorque(Random.Range(-maxRotation, maxRotation));
        // Add initial movement and rotation
        // var force = new Vector2(0, speed);
        // rb.AddForce(force);
    }

    public void Move(Vector2 direction)
    {
        rb.AddForce(direction * speed);
    }
}
