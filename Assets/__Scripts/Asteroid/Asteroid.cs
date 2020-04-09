using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Enemy))]
public class Asteroid : MonoBehaviour
{
    [Header("Destruction")]
    [SerializeField] private Asteroid breaksInto;
    [SerializeField] private int numHitsBeforeDesroy;
    [SerializeField] private GameObject destroyEffect;
    [SerializeField] private float destroyEffectDuration;

    private int numHits;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var laser = other.GetComponent<Laser>();

        if (laser)
        {
            Destroy(laser.gameObject);

            if (breaksInto)
            {
                if (++numHits < numHitsBeforeDesroy)
                {
                    // Show some feedback indicating it was hit
                    ToggleIsShot(1);
                    return;
                }
                else
                {
                    // Break in two
                    Instantiate(breaksInto, transform.position, transform.rotation);
                    Instantiate(breaksInto, transform.position, transform.rotation);
                }
            }

            if (laser.tag != Laser.PLAYER_LASER)
            {
                // Points should only be given if the player destroyed it
                GetComponent<Enemy>().ScoreValue = 0;
            }

            // Show an explosion
            GameObject explosion = Instantiate(destroyEffect, transform.position, transform.rotation);
            Destroy(explosion, destroyEffectDuration);
            Destroy(gameObject);

            PublishAsteroidDestroyedEvent();
        }
    }

    private void ToggleIsShot(int flag)
    {
        // For animating the Asteroid when shot
        animator.SetBool("IsShot", flag == 1 ? true : false);
    }

    private void PublishAsteroidDestroyedEvent()
    {
        // Make sure somebody is listening
        if (Enemy.EnemyDestroyedEvent != null)
        {
            Enemy.EnemyDestroyedEvent(GetComponent<Enemy>());
        }
    }
}
