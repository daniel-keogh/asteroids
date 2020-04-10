using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(UFOWeapons))]
public class UFOBehaviour : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float rotateSpeed = 200f;

    [Tooltip("The number of seconds the UFO will pause after reaching a Waypoint.")]
    [SerializeField] private float waitTimeOnArrival = 0.5f;

    [Tooltip("When the UFO is this distance from the player it will stop moving.")]
    [SerializeField] private float stoppingDistance = 10f;

    [Tooltip("When the UFO is this distance from the player it will start to move backwards.")]
    [SerializeField] private float retreatDistance = 7.5f;

    [Tooltip(@"The point where the UFO will move towards. The waypoint will be randomly
    re-positioned once the UFO reaches it.")]
    [SerializeField] private UFOWaypoint waypointPrefab;

    [Header("Vision")]
    [Tooltip("The UFO's point of vision.")]
    [SerializeField] private Transform eyeline;

    [Tooltip("How far the UFO can see.")]
    [SerializeField] private float sightDistance = 25.0f;

    [Tooltip("The objects that the UFO can see & react to.")]
    [SerializeField] private LayerMask visibleObjects;

    private float waitTime;
    private Rigidbody2D rb;
    private Transform target;
    private Transform waypoint;
    private UFOWeapons weapons;
    private Vector3 viewport;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        weapons = GetComponent<UFOWeapons>();

        viewport = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        // Create the UFOWaypoint
        waypoint = Instantiate(waypointPrefab).transform;

        // Place the waypoint at a random position
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

    private void OnEnable()
    {
        Player.PlayerKilledEvent += OnTargetDestroyEvent;
        WeaponsController.LaserFiredEvent += OnPlayerLaserFired;
    }

    private void OnDisable()
    {
        Player.PlayerKilledEvent -= OnTargetDestroyEvent;
        WeaponsController.LaserFiredEvent -= OnPlayerLaserFired;
    }

    private void OnDestroy()
    {
        // Make sure the UFOWaypoint is also destroyed
        if (waypoint)
        {
            Destroy(waypoint.gameObject);
        }
    }

    private void RotateToTarget()
    {
        // The UFO will always rotate towards the position of either the 
        // target Transform, or the waypoint transform (if target is null).
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
            weapons.StartShooting();
        }
        else
        {
            // Don't shoot unless looking at the player
            weapons.StopShooting();
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
                speed * Time.deltaTime
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
            // Follow the player and retreat if too close.
            // Reference:  Blackthornprod - https://www.youtube.com/watch?v=_Z1t7MNk0c4
            var distance = Vector2.Distance(transform.position, target.position);

            if (distance > stoppingDistance)
            {
                transform.position = Vector2.MoveTowards(
                    transform.position,
                    target.position,
                    speed * Time.deltaTime
                );
            }
            else if (distance < stoppingDistance && distance > retreatDistance)
            {
                transform.position = this.transform.position;
            }
            else
            {
                transform.position = Vector2.MoveTowards(
                    transform.position,
                    target.position,
                    -speed * Time.deltaTime
                );
            }
        }
    }

    private void OnTargetDestroyEvent()
    {
        // Stop targeting the player if they died.
        target = null;
    }

    private void OnPlayerLaserFired(WeaponsController weapons)
    {
        // Move towards wherever the shot came from
        waypoint.position = weapons.transform.position;
    }
}
