using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Enemy))]
public class UFO : MonoBehaviour
{
    public const string TAG_NAME = "UFO";

    [Header("Death")]
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private float explosionDuration = 1f;
    [SerializeField] private AudioClip explosionSound;

    private void OnTriggerEnter2D(Collider2D other)
    {
        var laser = other.GetComponent<Laser>();

        if (laser)
        {
            // Only die if hit by the player
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
            // Also destroy the asteroid
            Destroy(asteroid.gameObject);
        }

        Die();
    }

    private void Die()
    {
        if (explosionEffect)
        {
            SoundController.FindSoundController()?.PlayOneShot(explosionSound);

            // Show an explosion
            GameObject explosion = Instantiate(
                explosionEffect,
                transform.position,
                transform.rotation
            );

            Destroy(explosion, explosionDuration);
        }

        Destroy(gameObject);
    }

    private void PublishUFODestroyedEvent()
    {
        // Make sure somebody is listening
        if (Enemy.EnemyDestroyedEvent != null)
        {
            Enemy.EnemyDestroyedEvent(GetComponent<Enemy>());
        }
    }
}
