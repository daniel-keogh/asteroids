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
    [SerializeField] private float waitTimeOnArrival = 1f;
    [SerializeField] private float stoppingDistance = 10f;
    [SerializeField] private float retreatDistance = 7.5f;

    [Header("Raycast")]
    [SerializeField] private Transform eyeline;
    [SerializeField] private float sightDistance = 25.0f;
    [SerializeField] private LayerMask visibleObjects;

    private float waitTime;
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

        waitTime = waitTimeOnArrival;
    }

    void Update()
    {
        Move();

        Debug.DrawRay(
            eyeline.position,
            transform.up * sightDistance,
            Color.magenta
        );

        TargetPlayer();
    }

    void FixedUpdate()
    {
        RotateToTarget();
    }

    private void RotateToTarget()
    {
        // The UFO will always rotate towards the position of either the 
        // target Transform, or the waypoint (if target is null).
        // Reference: Brackeys - How to make a Homing Missile in Unity
        // https://www.youtube.com/watch?v=0v_H3oOR0aU

        Vector2 direction;

        if (target)
        {
            direction = (Vector2)target.position - rb.position;
        }
        else
        {
            direction = (Vector2)waypoint.position - rb.position;
        }

        direction.Normalize();

        float rotateAmount = Vector3.Cross(direction, transform.up).z;

        rb.angularVelocity = -rotateAmount * rotateSpeed;
    }

    private void TargetPlayer()
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
        if (!target)
        {
            // Set a waypoint and move towards it.
            // Reference:  Blackthornprod - https://www.youtube.com/watch?v=8eWbSN2T8TE
            transform.position = Vector2.MoveTowards(
                transform.position,
                waypoint.position,
                speed
            );

            if (Vector2.Distance(transform.position, waypoint.position) <= 0f)
            {
                if (waitTime <= 0)
                {
                    waypoint.position = new Vector2(
                        Random.Range(-viewport.x, viewport.x),
                        Random.Range(-viewport.y, viewport.y)
                    );

                    waitTime = waitTimeOnArrival;
                }
                else
                {
                    waitTime -= Time.deltaTime;
                }
            }
        }
        else
        {
            // Follow/Retreat.
            // Reference:  Blackthornprod - https://www.youtube.com/watch?v=_Z1t7MNk0c4

            if (Vector2.Distance(transform.position, target.position) > stoppingDistance)
            {
                transform.position = Vector2.MoveTowards(transform.position, target.position, speed);
            }
            else if (Vector2.Distance(transform.position, target.position) < stoppingDistance && Vector2.Distance(transform.position, target.position) > retreatDistance)
            {
                transform.position = this.transform.position;
            }
            else
            {
                transform.position = Vector2.MoveTowards(
                    transform.position,
                    target.position,
                    -speed
                );
            }
        }
    }
}
