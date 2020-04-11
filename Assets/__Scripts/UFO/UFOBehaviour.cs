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

    [Tooltip("The number of seconds the UFO will pause movement after reaching a Waypoint.")]
    [SerializeField] private float waitTimeOnArrival = 0.5f;

    [Tooltip("When the UFO is this distance from the player it will stop moving.")]
    [SerializeField] private float stoppingDistance = 10f;

    [Tooltip("When the UFO is this distance from the player it will start to move backwards.")]
    [SerializeField] private float retreatDistance = 7.5f;

    [Tooltip("The point to which the UFO will move towards.\n\nThe UFOWaypoint will be randomly re-positioned once the UFO reaches it.")]
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

        // Place the waypoint at a random position within the scene
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

        LookForTarget();
    }

    void FixedUpdate()
    {
        RotateToTarget();
    }

    private void OnEnable()
    {
        Player.PlayerKilledEvent += OnTargetDestroyEvent;
        WeaponsController.LaserFiredEvent += OnPlayerLaserFiredEvent;
    }

    private void OnDisable()
    {
        Player.PlayerKilledEvent -= OnTargetDestroyEvent;
        WeaponsController.LaserFiredEvent -= OnPlayerLaserFiredEvent;
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
        // `target` Transform, or the `waypoint` transform (if `target` is null).
        //
        // Reference: Brackeys - "How to make a Homing Missile in Unity"
        // https://www.youtube.com/watch?v=0v_H3oOR0aU

        Vector2 direction;

        // Get the direction from the UFO to the target (or to the waypoint 
        // if the target is not set).
        if (target)
        {
            direction = (Vector2)target.position - rb.position;
        }
        else
        {
            direction = (Vector2)waypoint.position - rb.position;
        }

        direction.Normalize();

        // Get the cross product
        float rotateAmount = Vector3.Cross(direction, transform.up).z;

        // Rotate the UFO by `rotateAmount`
        rb.angularVelocity = -rotateAmount * rotateSpeed;
    }

    private void LookForTarget()
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
            // Lock-on to the player (set it as the target) & shoot at them
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
        if (target)
        {
            FollowTarget();
        }
        else
        {
            // If no target is set, move around at random
            FollowWaypoint();
        }
    }

    private void FollowWaypoint()
    {
        // Set a waypoint at a random position and then move towards it.
        //
        // Reference:  Blackthornprod - "Patrol AI with Unity and C#"
        // https://www.youtube.com/watch?v=8eWbSN2T8TE

        // Move towards the waypoint
        transform.position = Vector2.MoveTowards(
            transform.position,
            waypoint.position,
            speed * Time.deltaTime
        );

        // Check if the UFO has reached its destination
        if (Vector2.Distance(transform.position, waypoint.position) <= 0f)
        {
            // Check if it's time to move again
            if (waitTime <= 0)
            {
                // Set a new waypoint
                waypoint.position = new Vector2(
                    Random.Range(-viewport.x, viewport.x),
                    Random.Range(-viewport.y, viewport.y)
                );

                waitTime = waitTimeOnArrival; // Reset wait time
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }
    }

    private void FollowTarget()
    {
        // Follow the player and retreat if too close.
        //
        // Reference:  Blackthornprod - "Shooting/Follow/Retreat Enemy AI with Unity and C#"
        // https://www.youtube.com/watch?v=_Z1t7MNk0c4

        // Get the distance from the target
        var distance = Vector2.Distance(transform.position, target.position);

        if (distance > stoppingDistance)
        {
            // Move towards the target
            transform.position = Vector2.MoveTowards(
                transform.position,
                target.position,
                speed * Time.deltaTime
            );
        }
        else if (distance < stoppingDistance && distance > retreatDistance)
        {
            // Freeze the position (i.e. stop moving)
            transform.position = this.transform.position;
        }
        else
        {
            // Too close to the player - Retreat
            transform.position = Vector2.MoveTowards(
                transform.position,
                target.position,
                -speed * Time.deltaTime
            );
        }
    }

    private void OnTargetDestroyEvent()
    {
        // Stop targeting the player if they died 
        // (So they're not shot at immediately after respawning)
        target = null;
    }

    private void OnPlayerLaserFiredEvent(WeaponsController weapons)
    {
        // Move towards wherever the shot came from
        waypoint.position = weapons.transform.position;
    }
}
