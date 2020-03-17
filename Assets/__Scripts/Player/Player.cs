using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private int explosionDuration;
    [SerializeField] private float timeToRepawn;

    private Rigidbody2D rb;

    // Delegate type to use for event
    public delegate void PlayerKilled(Player player);

    // Static method to be implemented in the listener
    public static PlayerKilled PlayerKilledEvent;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        var asteroid = other.collider.GetComponent<Asteroid>();

        if (asteroid)
        {
            Destroy(asteroid.gameObject);

            GameObject explosion = Instantiate(explosionEffect, transform.position, transform.rotation);
            Destroy(explosion, explosionDuration);

            gameObject.SetActive(false);
            Invoke("Respawn", timeToRepawn);

            PublishPlayerKilledEvent();
        }
    }

    private void Respawn()
    {
        rb.transform.position = new Vector2(0, 0);
        rb.velocity = new Vector2(0, 0);
        rb.transform.rotation = Quaternion.identity;

        gameObject.SetActive(true);
    }

    private void PublishPlayerKilledEvent()
    {
        // Make sure somebody is listening
        if (PlayerKilledEvent != null)
        {
            PlayerKilledEvent(this);
        }
    }
}
