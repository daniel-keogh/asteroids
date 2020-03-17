﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Used from GameController enemy.ScoreValue
    public int ScoreValue
    {
        get { return scoreValue; }
    }

    // Delegate type to use for event
    public delegate void EnemyKilled(Asteroid asteroid);

    // Static method to be implemented in the listener
    public static EnemyKilled EnemyKilledEvent;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
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

            PublishEnemyKilledEvent();

            GameObject explosion = Instantiate(destroyEffect, transform.position, transform.rotation);
            Destroy(explosion, destroyEffectDuration);
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        var player = other.collider.GetComponent<PlayerMovement>();

        if (player)
        {
            Destroy(player.gameObject);
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

    private void PublishEnemyKilledEvent()
    {
        // Make sure somebody is listening
        if (EnemyKilledEvent != null)
        {
            EnemyKilledEvent(this);
        }
    }
}
