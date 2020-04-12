using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(PointEffector2D))]
[RequireComponent(typeof(Animator))]
public class ForceField : MonoBehaviour
{
    public bool IsActivated
    {
        get { return isActivated; }
    }

    [SerializeField] private AudioClip hitByLaserClip;

    private SpriteRenderer spriteRenderer;
    private CapsuleCollider2D capsuleCollider;
    private PointEffector2D pointEffector;
    private Animator animator;
    private bool isActivated;
    private SoundController sc;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        pointEffector = GetComponent<PointEffector2D>();
        animator = GetComponent<Animator>();

        sc = SoundController.FindSoundController();

        // Turn off by default
        Deactivate();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var laser = other.GetComponent<Laser>();

        if (laser)
        {
            // Destroy any Enemy lasers that hit the ForceField
            if (laser.tag == Laser.ENEMY_LASER)
            {
                sc?.PlayOneShot(hitByLaserClip);
                Destroy(laser.gameObject);
            }
        }
    }

    public void Activate()
    {
        SetComponentsEnabled(true);
        isActivated = true;
    }

    public void Deactivate()
    {
        SetComponentsEnabled(false);
        isActivated = false;
    }

    private void SetComponentsEnabled(bool status)
    {
        spriteRenderer.enabled = status;
        capsuleCollider.enabled = status;
        pointEffector.enabled = status;
        animator.enabled = status;
    }
}
