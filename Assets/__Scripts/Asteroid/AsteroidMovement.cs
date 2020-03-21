using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class AsteroidMovement : MonoBehaviour
{
    public float Speed
    {
        get { return speed; }
    }

    private Rigidbody2D rb;

    [SerializeField] private float maxRotation;
    [SerializeField] private float speed;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        speed = Mathf.Abs(speed);

        // Add initial rotation
        rb.AddTorque(Random.Range(-maxRotation, maxRotation));
    }
}
