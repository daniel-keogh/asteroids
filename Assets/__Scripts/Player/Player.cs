using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [Header("Player Death")]
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private int explosionDuration;
    [SerializeField] private float repawnDelay;

    private Rigidbody2D rb;
    private GameController gc;

    // event for telling the player died
    public delegate void PlayerKilled();
    public static event PlayerKilled PlayerKilledEvent;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gc = FindObjectOfType<GameController>();
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
            rb.transform.position = new Vector2(0, 0);
            rb.velocity = new Vector2(0, 0);
            rb.transform.rotation = Quaternion.identity;

            gameObject.SetActive(true);
        }
    }

    private void PublishPlayerKilledEvent()
    {
        PlayerKilledEvent?.Invoke();
    }
}
