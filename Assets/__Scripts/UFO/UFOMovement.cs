using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class UFOMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private Transform waypoint;
    [SerializeField] private float speed;
    [SerializeField] private float rotateSpeed = 200f;

    [Header("Raycast")]
    [SerializeField] private Transform eyeline;
    [SerializeField] private float sightDistance = 25.0f;
    [SerializeField] private LayerMask visibleObjects;

    private int wpIndex = 0;
    private Transform target;
    private Rigidbody2D rb;
    private Vector3 viewport;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        viewport = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        waypoint.position = new Vector2(
            Random.Range(-viewport.x, viewport.x),
            Random.Range(-viewport.y, viewport.y)
        );
    }

    void Update()
    {
        Move();

        Debug.DrawRay(
            eyeline.position,
            transform.up * sightDistance,
            Color.magenta
        );

        EnemyInSight();
    }

    void FixedUpdate()
    {
        // https://www.youtube.com/watch?v=0v_H3oOR0aU
        if (target)
        {
            Vector2 direction = (Vector2)target.position - rb.position;

            direction.Normalize();

            float rotateAmount = Vector3.Cross(direction, transform.up).z;

            rb.angularVelocity = -rotateAmount * rotateSpeed;
            rb.velocity = transform.up * speed;
        }
    }

    private void EnemyInSight()
    {
        // Throw a raycast and see if it hits anything
        var hit = Physics2D.Raycast(
            eyeline.position,
            transform.up,
            sightDistance,
            visibleObjects
        );

        if (hit)
        {
            // Lock-on to the player
            target = hit.transform;
        }
    }

    private void Move()
    {
        transform.position = Vector2.MoveTowards(
            transform.position,
            waypoint.position,
            speed
        );

        if (Vector2.Distance(transform.position, waypoint.position) <= 0f)
        {
            waypoint.position = new Vector2(
                Random.Range(-viewport.x, viewport.x),
                Random.Range(-viewport.y, viewport.y)
            );
        }
    }
}
