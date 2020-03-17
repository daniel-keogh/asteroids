using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerDeath : MonoBehaviour
{
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private int explosionDuration;
    [SerializeField] private float timeToRepawn;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnDestroy()
    {
        GameObject explosion = Instantiate(explosionEffect, transform.position, transform.rotation);
        Destroy(explosion, explosionDuration);

        Invoke("Repawn", timeToRepawn);
    }

    private void Respawn()
    {

    }
}
