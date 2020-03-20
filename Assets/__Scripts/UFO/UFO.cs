using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class UFO : MonoBehaviour
{
    public int ScoreValue
    {
        get { return scoreValue; }
    }

    [SerializeField] private int scoreValue;
    [SerializeField] private float speed;
    [SerializeField] private float rotation;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Add initial movement and rotation
        var force = new Vector2(0, speed);
        rb.AddRelativeForce(force);
        // rb.AddTorque(rotation);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var laser = other.GetComponent<Laser>();

        if (laser)
        {
            Destroy(laser.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        var asteroid = other.collider.GetComponent<Asteroid>();

        if (asteroid)
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }

    private void PublishUFODestroyedEvent()
    {

    }
}
