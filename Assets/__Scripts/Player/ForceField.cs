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

    private SpriteRenderer spriteRenderer;
    private CapsuleCollider2D capsuleCollider;
    private PointEffector2D pointEffector;
    private Animator animator;
    private bool isActivated;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        pointEffector = GetComponent<PointEffector2D>();
        animator = GetComponent<Animator>();

        Deactivate();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var laser = other.GetComponent<Laser>();

        if (laser)
        {
            if (laser.tag == Laser.ENEMY_LASER)
            {
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
