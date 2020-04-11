using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    private const string RESPAWN_METHOD = "Respawn";

    [Header("Health")]
    [Tooltip("The number of times the player can be shot before dying.")]
    [SerializeField] private int shotsBeforeDeath = 2;

    [Header("Death")]
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private float explosionDuration = 1f;
    [SerializeField] private float repawnDelay = 3f;
    [SerializeField] private AudioClip explosionSound;

    [Header("Respawn")]
    [Tooltip("The number of seconds the player will be invincible after respawning.")]
    [SerializeField] private float immunityDuration = 3f;

    private Rigidbody2D rb;
    private GameController gc;
    private ForceField forceField;
    private int numTimesShot = 0;

    // Event for telling the system the player died
    public delegate void PlayerKilled();
    public static event PlayerKilled PlayerKilledEvent;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gc = FindObjectOfType<GameController>();
        forceField = GetComponentInChildren<ForceField>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (forceField.IsActivated)
            return; // Ignore

        var laser = other.GetComponent<Laser>();

        if (laser)
        {
            if (laser.tag == Laser.ENEMY_LASER)
            {
                if (++numTimesShot >= shotsBeforeDeath)
                {
                    Die();
                }

                Destroy(laser.gameObject);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (forceField.IsActivated)
            return; // Ignore

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
            GameObject explosion = Instantiate(explosionEffect, transform.position, transform.rotation);
            Destroy(explosion, explosionDuration);
        }

        // Disappear & respawn after a delay
        gameObject.SetActive(false);
        Invoke(RESPAWN_METHOD, repawnDelay);

        PublishPlayerKilledEvent();
    }

    private void Respawn()
    {
        // Only respawn if the player has any lives remaining
        if (gc.RemainingLives > 0)
        {
            // Respawn in the center of the scene
            rb.transform.position = new Vector2(0, 0);
            rb.velocity = new Vector2(0, 0);
            rb.transform.rotation = Quaternion.identity;

            numTimesShot = 0;

            gameObject.SetActive(true);

            // Try & prevent from dying immediately after respawning
            StartCoroutine(ImmunityCoroutine());
        }
    }

    private IEnumerator ImmunityCoroutine()
    {
        forceField.Activate();
        yield return new WaitForSeconds(immunityDuration);
        forceField.Deactivate();
    }

    private void PublishPlayerKilledEvent()
    {
        // Indicate the player has lost a life
        PlayerKilledEvent?.Invoke();
    }
}
