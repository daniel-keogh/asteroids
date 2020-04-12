using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class AsteroidMovement : MonoBehaviour
{
    [SerializeField] private float rotation = 100f;
    [SerializeField] private float minSpeed = 150f;
    [SerializeField] private float maxSpeed = 300f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        // Add inital rotation
        rb.AddTorque(Random.Range(-rotation, rotation));
    }

    public void Move(Vector2 direction)
    {
        rb.AddRelativeForce(direction * Random.Range(minSpeed, maxSpeed));
    }
}
