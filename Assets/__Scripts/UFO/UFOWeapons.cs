﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFOWeapons : MonoBehaviour
{
    [SerializeField] private float laserSpeed = 20.0f;
    [SerializeField] private float fireRate = 0.5f;
    [SerializeField] private Transform turretTransform;
    [SerializeField] private Laser laserPrefab;
    [SerializeField] private AudioClip shootClip;
    [SerializeField] [Range(0f, 1.0f)] private float shootVolume = 0.5f;

    private GameObject laserParent;
    private Coroutine firingCoroutine;
    private AudioSource audioSource;
    private bool isShooting = false;

    void Start()
    {
        laserParent = GameObject.Find("LaserParent");

        if (!laserParent)
        {
            laserParent = new GameObject("LaserParent");
        }

        audioSource = GetComponent<AudioSource>();
    }

    public void StartShooting()
    {
        if (isShooting)
        {
            return;
        }
        else
        {
            firingCoroutine = StartCoroutine(FireCoroutine());
        }
    }

    public void StopShooting()
    {
        isShooting = false;

        if (firingCoroutine != null)
        {
            StopCoroutine(firingCoroutine);
        }
    }

    private void OnDisable()
    {
        StopShooting();
    }

    private IEnumerator FireCoroutine()
    {
        isShooting = true;

        while (isShooting)
        {
            Laser laser = Instantiate(laserPrefab, laserParent.transform);
            laser.transform.position = turretTransform.position;
            // Face the same direction as the UFO
            laser.transform.rotation = transform.rotation;

            Rigidbody2D rb = laser.GetComponent<Rigidbody2D>();
            // Shoot in whatever direction the player is facing
            rb.velocity = transform.up * laserSpeed;

            audioSource.PlayOneShot(shootClip, shootVolume);

            yield return new WaitForSeconds(fireRate);
        }
    }
}