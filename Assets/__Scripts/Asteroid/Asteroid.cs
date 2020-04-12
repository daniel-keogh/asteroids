using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AsteroidMovement))]
[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Enemy))]
public class Asteroid : MonoBehaviour
{
    public const string TAG_NAME = "Asteroid";

    [Header("Destruction")]
    [SerializeField] private Asteroid breaksInto;
    [SerializeField] private int numHitsBeforeDesroy;
    [SerializeField] private GameObject destroyEffect;
    [SerializeField] private float destroyEffectDuration = 1f;
    [SerializeField] private AudioClip explosionSound;

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
                    BreakInTwo();
                }
            }

            if (laser.tag != Laser.PLAYER_LASER)
            {
                // Points should only be given if the player destroyed it
                GetComponent<Enemy>().ScoreValue = 0;
            }

            // Play a sound
            SoundController.FindSoundController()?.PlayOneShot(explosionSound);

            // Show an explosion
            GameObject explosion = Instantiate(destroyEffect, transform.position, transform.rotation);
            Destroy(explosion, destroyEffectDuration);
            Destroy(gameObject);

            PublishAsteroidDestroyedEvent();
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.GetComponent<Asteroid>())
            return; // ignore collisions with other Asteroids

        // Don't award any points for collisions
        GetComponent<Enemy>().ScoreValue = 0;
        PublishAsteroidDestroyedEvent();
    }

    private void BreakInTwo()
    {
        for (int i = 0; i < 2; i++)
        {
            var a = Instantiate(breaksInto, transform.position, transform.rotation);

            var movement = a.GetComponent<AsteroidMovement>();
            movement.Move(new Vector2(
                Random.Range(-1, 1),
                Random.Range(-1, 1)
            ));

            // Set a rotation
            float rotation = Random.Range(0f, 360f);
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, rotation));

            // Try to stop asteroids from sticking together
            a.transform.Translate(a.transform.up);
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
