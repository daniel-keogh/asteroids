using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class HyperSpace : MonoBehaviour
{
    [Tooltip("Prevents the player being re-positioned too close to the edge.")]
    [SerializeField] private float borderPadding = 1.0f;
    [SerializeField] private bool disableRotation = false;
    [SerializeField] private float duration = 2.0f;
    [SerializeField] private float cooldownDuration = 3.0f;

    private float maxRotate = 360;
    private bool isCooledDown = true;
    private Rigidbody2D rb;
    private Animator animator;
    private HyperSpaceEffect hyperSpaceVFX;
    private Vector3 viewport;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        hyperSpaceVFX = GetComponentInChildren<HyperSpaceEffect>();

        // The bottom-left of the viewport is (0,0); the top-right is (1,1).
        viewport = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
        viewport.x -= borderPadding;
        viewport.y -= borderPadding;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            if (isCooledDown)
            {
                StartCoroutine(HyperSpaceCoroutine());
            }
        }
    }

    private void OnDisable()
    {
        // in case the player dies while hyperspacing
        isCooledDown = true;
    }

    private IEnumerator HyperSpaceCoroutine()
    {
        isCooledDown = false;

        animator.SetTrigger(HyperSpaceEffect.START_TRIGGER);

        yield return new WaitForSeconds(duration);

        Move();

        animator.SetTrigger(HyperSpaceEffect.END_TRIGGER);

        // Prevent user from spamming the 'H' key
        yield return new WaitForSeconds(cooldownDuration + duration);

        isCooledDown = true;
    }

    private void Move()
    {
        rb.transform.position = GenerateRandomPosition();

        if (!disableRotation)
        {
            rb.rotation = Random.Range(-maxRotate, maxRotate);
        }
    }

    private Vector3 GenerateRandomPosition()
    {
        return new Vector2(
            Random.Range(-viewport.y, viewport.y),
            Random.Range(-viewport.x, viewport.x)
        );
    }

    private void PlaySpawnEffect()
    {
        hyperSpaceVFX?.Play();
    }
}
