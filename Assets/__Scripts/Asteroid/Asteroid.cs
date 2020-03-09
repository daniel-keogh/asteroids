using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private int scoreValue;

    private void OnTriggerEnter2D(Collider2D other)
    {
        var laser = other.GetComponent<Laser>();

        if (laser)
        {
            Destroy(laser.gameObject);
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
}
