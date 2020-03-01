using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsController : MonoBehaviour
{
    private GameObject laserParent;
    private Coroutine firingCoroutine;
    private AudioSource audioSource;

    [SerializeField] private float laserSpeed = 20.0f;
    [SerializeField] private float fireRate = 0.3f;
    [SerializeField] private Laser laserPrefab;
    [SerializeField] private AudioClip shootClip;
    [SerializeField] [Range(0f, 1.0f)] private float shootVolume = 0.5f;

    void Start()
    {
        laserParent = GameObject.Find("LaserParent");

        if (!laserParent)
        {
            laserParent = new GameObject("LaserParent");
        }

        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            firingCoroutine = StartCoroutine(FireCoroutine());
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            StopCoroutine(firingCoroutine);
        }
    }

    private IEnumerator FireCoroutine()
    {
        while (true)
        {
            Laser laser = Instantiate(laserPrefab, laserParent.transform);
            laser.transform.position = transform.position;
            // Face the same direction as the player
            laser.transform.rotation = transform.rotation;

            Rigidbody2D rb = laser.GetComponent<Rigidbody2D>();
            // Shoot in whatever direction the player is facing
            rb.velocity = transform.up * laserSpeed;

            // Play a sound
            audioSource.PlayOneShot(shootClip, shootVolume);

            // Sleep for a short time
            yield return new WaitForSeconds(fireRate);
        }
    }
}
