using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class UFO : MonoBehaviour
{
    public int ScoreValue
    {
        get { return scoreValue; }
    }

    [Header("Scoring")]
    [SerializeField] private int scoreValue = 200;

    [Header("Death")]
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private float explosionDuration = 1f;

    // Delegate type to use for event
    public delegate void UFODestroyed(UFO ufo);

    // Static method to be implemented in the listener
    public static UFODestroyed UFODestroyedEvent;

    private void OnTriggerEnter2D(Collider2D other)
    {
        var laser = other.GetComponent<Laser>();

        if (laser)
        {
            if (laser.tag == Laser.PLAYER_LASER)
            {
                Destroy(laser.gameObject);
                Die();

                PublishUFODestroyedEvent();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        var asteroid = other.collider.GetComponent<Asteroid>();

        if (asteroid)
        {
            Destroy(asteroid.gameObject);
        }

        Die();
    }

    private void Die()
    {
        GameObject explosion = Instantiate(explosionEffect, transform.position, transform.rotation);
        Destroy(explosion, explosionDuration);

        Destroy(gameObject);
    }

    private void PublishUFODestroyedEvent()
    {
        // Make sure somebody is listening
        if (UFODestroyedEvent != null)
        {
            UFODestroyedEvent(this);
        }
    }
}
