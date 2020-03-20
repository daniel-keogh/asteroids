using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
// [RequireComponent(typeof(WrapAround))]
public class Asteroid : MonoBehaviour
{
    [SerializeField] private int scoreValue;
    [SerializeField] private Asteroid breaksInto;
    [SerializeField] private int numHitsBeforeDesroy;
    [SerializeField] private GameObject destroyEffect;
    [SerializeField] private float destroyEffectDuration;
    [SerializeField] private Color hitEffectColor;
    [SerializeField] private float hitEffectDuration;

    private int numHits;
    private SpriteRenderer spriteRenderer;
    // private WrapAround wrapAround;

    // Used from GameController enemy.ScoreValue
    public int ScoreValue
    {
        get { return scoreValue; }
    }

    // Delegate type to use for event
    public delegate void AsteroidDestroyed(Asteroid asteroid);

    // Static method to be implemented in the listener
    public static AsteroidDestroyed AsteroidDestroyedEvent;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // wrapAround = GetComponent<WrapAround>();
        // wrapAround.enabled = false;
    }

    // private void OnBecameVisible()
    // {
    //     wrapAround.enabled = true;
    // }

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
                    // show some kind of feedback indicating it was hit
                    StartCoroutine(ShowBlinkEffect());
                    return;
                }
                else
                {
                    Instantiate(breaksInto, transform.position, transform.rotation);
                    Instantiate(breaksInto, transform.position, transform.rotation);
                }
            }

            PublishAsteroidDestroyedEvent();

            GameObject explosion = Instantiate(destroyEffect, transform.position, transform.rotation);
            Destroy(explosion, destroyEffectDuration);
            Destroy(gameObject);
        }
    }

    private IEnumerator ShowBlinkEffect()
    {
        Color currentColor = spriteRenderer.material.color;

        spriteRenderer.material.color = hitEffectColor;
        yield return new WaitForSeconds(hitEffectDuration);
        spriteRenderer.material.color = Color.white;
    }

    private void PublishAsteroidDestroyedEvent()
    {
        // Make sure somebody is listening
        if (AsteroidDestroyedEvent != null)
        {
            AsteroidDestroyedEvent(this);
        }
    }
}
