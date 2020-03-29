using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(PointEffector2D))]
[RequireComponent(typeof(Animator))]
public class ForceField : MonoBehaviour
{
    [Tooltip("The number of seconds the force field will be active.")]
    [SerializeField] private float activeDuration = 3.0f;

    private SpriteRenderer spriteRenderer;
    private CircleCollider2D circleCollider;
    private PointEffector2D pointEffector;
    private Animator animator;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        circleCollider = GetComponent<CircleCollider2D>();
        pointEffector = GetComponent<PointEffector2D>();
        animator = GetComponent<Animator>();

        SetComponentsEnabled(false);
    }

    public void ActivateForceField()
    {
        StartCoroutine(ForceFieldCoroutine());
    }

    private IEnumerator ForceFieldCoroutine()
    {
        SetComponentsEnabled(true);
        yield return new WaitForSeconds(activeDuration);
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
