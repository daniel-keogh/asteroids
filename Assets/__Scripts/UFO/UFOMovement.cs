using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class UFOMovement : MonoBehaviour
{
    [SerializeField] private List<Transform> waypoints;
    [SerializeField] private float speed;
    [SerializeField] private float rotateSpeed = 200f;
    [SerializeField] private Transform eyeline;
    [SerializeField] private float sightDistance = 4.0f;
    [SerializeField] private LayerMask visibleObjects;

    private int wpIndex = 0;
    private Transform target;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (waypoints.Count > 0)
        {
            transform.position = waypoints[0].position;
        }
    }

    void Update()
    {
        if (waypoints.Count > 0)
        {
            MoveAlongPath();
        }

        Debug.DrawRay(
            eyeline.position,
            transform.up * sightDistance,
            Color.magenta
        );

        EnemyInSight();
    }

    private void FixedUpdate()
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
            target = hit.transform;
        }
    }

    private void MoveAlongPath()
    {
        var targetWaypoint = waypoints[wpIndex].position;

        transform.position = Vector2.MoveTowards(
            transform.position,
            targetWaypoint,
            speed
        );

        if (transform.position == targetWaypoint)
        {
            nextWaypoint();
        }
    }

    /*
     * Reference:
     * Binkan Salaryman - https://stackoverflow.com/a/33782325
     */
    private void nextWaypoint()
    {
        wpIndex++;
        wpIndex %= waypoints.Count; // clip index (turns to 0 if index == items.Count)
    }

    private void previousWaypoint()
    {
        wpIndex--;
        if (wpIndex < 0)
        {
            wpIndex = waypoints.Count - 1;
        }
    }
}
