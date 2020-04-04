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

    [SerializeField] private int scoreValue = 200;

    void Start()
    {

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

    }

    private void PublishUFODestroyedEvent()
    {

    }
}
