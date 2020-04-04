using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [Header("Player Death")]
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private float explosionDuration = 1f;
    [SerializeField] private float repawnDelay = 3f;

    [Header("Respawn")]
    [Tooltip("The number of seconds the player will be invincible after respawning.")]
    [SerializeField] private float immunityDuration = 3f;

    private Rigidbody2D rb;
    private GameController gc;
    private ForceField forceField;

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
        var laser = other.GetComponent<Laser>();

        if (laser)
        {
            if (laser.tag == Laser.ENEMY_LASER)
            {
                Destroy(laser.gameObject);
                Die();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        var asteroid = other.collider.GetComponent<Asteroid>();
        var ufo = other.collider.GetComponent<UFO>();

        if (asteroid || ufo)
        {
            Destroy(other.gameObject);
            Die();
        }
    }

    private void Die()
    {
        if (explosionEffect)
        {
            GameObject explosion = Instantiate(explosionEffect, transform.position, transform.rotation);
            Destroy(explosion, explosionDuration);
        }

        gameObject.SetActive(false);
        Invoke("Respawn", repawnDelay);

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

            gameObject.SetActive(true);

            StartCoroutine(ImmunityCoroutine());
        }
    }

    private IEnumerator ImmunityCoroutine()
    {
        // Try & prevent from dying immediately
        forceField.Activate();
        yield return new WaitForSeconds(immunityDuration);
        forceField.Deactivate();
    }

    private void PublishPlayerKilledEvent()
    {
        PlayerKilledEvent?.Invoke();
    }
}
