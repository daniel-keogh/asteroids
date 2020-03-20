using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFOPath : MonoBehaviour
{
    [SerializeField] private List<Transform> waypoints;
    [SerializeField] private float speed;
    private int waypointIndex = 0;

    void Start()
    {
        transform.position = waypoints[waypointIndex].transform.position;
    }

    void Update()
    {
        if (waypointIndex < waypoints.Count)
        {
            var targetPos = waypoints[waypointIndex].transform.position;
            var movementThisFrame = speed * Time.deltaTime;

            transform.position = Vector2.MoveTowards(transform.position, targetPos, movementThisFrame);

            if (transform.position == targetPos)
            {
                waypointIndex++;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
