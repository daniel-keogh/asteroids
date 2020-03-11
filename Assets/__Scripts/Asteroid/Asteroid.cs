using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private int scoreValue;
    [SerializeField] private AsteroidSize currentSize;

    // Used from GameController enemy.ScoreValue
    public int ScoreValue { get { return scoreValue; } }

    // Delegate type to use for event
    public delegate void EnemyKilled(Asteroid asteroid);

    // Static method to be implemented in the listener
    public static EnemyKilled EnemyKilledEvent;

    void Start()
    {
        currentSize = AsteroidSize.Large;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var laser = other.GetComponent<Laser>();

        if (laser)
        {
            Destroy(laser.gameObject);

            PublishEnemyKilledEvent();

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

    private void PublishEnemyKilledEvent()
    {
        // Make sure somebody is listening
        if (EnemyKilledEvent != null)
        {
            EnemyKilledEvent(this);
        }
    }
}
