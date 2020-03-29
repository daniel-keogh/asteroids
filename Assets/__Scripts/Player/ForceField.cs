using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(PointEffector2D))]
[RequireComponent(typeof(Animator))]
public class ForceField : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private CircleCollider2D circleCollider;
    private PointEffector2D pointEffector;
    private Animator animator;
    private PolygonCollider2D parentCollider;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        circleCollider = GetComponent<CircleCollider2D>();
        pointEffector = GetComponent<PointEffector2D>();
        animator = GetComponent<Animator>();

        parentCollider = GetComponentInParent<PolygonCollider2D>();

        SetComponentsEnabled(false);
    }

    public void Activate()
    {
        parentCollider.enabled = false;
        SetComponentsEnabled(true);
    }

    public void Deactivate()
    {
        parentCollider.enabled = true;
        SetComponentsEnabled(false);
    }

    private void SetComponentsEnabled(bool status)
    {
        spriteRenderer.enabled = status;
        circleCollider.enabled = status;
        pointEffector.enabled = status;
        animator.enabled = status;
    }
}
