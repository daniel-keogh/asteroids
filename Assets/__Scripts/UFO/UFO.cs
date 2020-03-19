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

    private int scoreValue;

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

    private void PublishUFODestroyedEvent()
    {

    }
}
