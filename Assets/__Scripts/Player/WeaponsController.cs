﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsController : MonoBehaviour
{
    [Header("Shooting")]
    [SerializeField] private float laserSpeed = 20.0f;
    [SerializeField] private float fireRate = 0.3f;
    [SerializeField] private Transform turretTransform;
    [SerializeField] private Laser laserPrefab;

    [Header("Audio")]
    [SerializeField] private AudioClip shootClip;
    [SerializeField] [Range(0f, 1.0f)] private float shootVolume = 0.5f;

    private GameObject laserParent;
    private Coroutine firingCoroutine;
    private SoundController sc;

    public delegate void LaserFired(WeaponsController weapons);
    public static LaserFired LaserFiredEvent;

    void Start()
    {
        laserParent = GameObject.Find(Laser.LASER_PARENT);

        if (!laserParent)
        {
            laserParent = new GameObject(Laser.LASER_PARENT);
        }

        sc = SoundController.FindSoundController();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            firingCoroutine = StartCoroutine(FireCoroutine());
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            StopFireCoroutine();
        }
    }

    private void OnDisable()
    {
        StopFireCoroutine();
    }

    private IEnumerator FireCoroutine()
    {
        while (true)
        {
            Laser laser = Instantiate(laserPrefab, laserParent.transform);
            laser.transform.position = turretTransform.position;
            // Face the same direction as the player
            laser.transform.rotation = transform.rotation;

            Rigidbody2D rb = laser.GetComponent<Rigidbody2D>();
            // Shoot in whatever direction the player is facing
            rb.velocity = transform.up * laserSpeed;

            // Play a sound
            sc?.PlayOneShot(shootClip, shootVolume);

            PublishLaserFiredEvent();

            // Sleep for a short time
            yield return new WaitForSeconds(fireRate);
        }
    }

    private void StopFireCoroutine()
    {
        if (firingCoroutine != null)
        {
            StopCoroutine(firingCoroutine);
        }
    }

    private void PublishLaserFiredEvent()
    {
        // Indicate a Laser has been fired by the Player
        LaserFiredEvent?.Invoke(this);
    }
}
