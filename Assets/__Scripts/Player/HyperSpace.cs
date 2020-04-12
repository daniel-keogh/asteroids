using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(WeaponsController))]
[RequireComponent(typeof(SpriteRenderer))]
public class HyperSpace : MonoBehaviour
{
    [Header("Repositioning")]
    [Tooltip("Prevents the player being re-positioned too close to the edge.")]
    [SerializeField] private float borderPadding = 1.0f;

    [Header("Duration")]
    [SerializeField] private float duration = 2.0f;
    [Tooltip("Sets the minimum time between hyperspace jumps.")]
    [SerializeField] private float cooldownDuration = 3.0f;

    [Header("Audio")]
    [SerializeField] private AudioClip sound;

    private float maxRotate = 360;
    private bool isCooledDown = true;
    private Rigidbody2D rb;
    private Animator animator;
    private HyperSpaceEffect hyperSpaceVFX;
    private Vector3 viewport;
    private Vector3 startScale;
    private SoundController sc;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        hyperSpaceVFX = GetComponentInChildren<HyperSpaceEffect>();

        sc = SoundController.FindSoundController();

        // The bottom-left of the viewport is (0,0); the top-right is (1,1).
        viewport = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        // Add some padding
        viewport.x -= borderPadding;
        viewport.y -= borderPadding;

        startScale = transform.localScale;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            // Prevent the user from spamming the 'H' key
            if (isCooledDown)
            {
                StartCoroutine(HyperSpaceCoroutine());
            }
        }
    }

    private void OnDisable()
    {
        // Reset everything in case the player died while hyperspacing
        isCooledDown = true;

        GetComponent<PlayerMovement>().enabled = true;
        GetComponent<WeaponsController>().enabled = true;
        GetComponent<SpriteRenderer>().enabled = true;

        transform.localScale = startScale;
    }

    private IEnumerator HyperSpaceCoroutine()
    {
        isCooledDown = false;

        animator.SetTrigger(HyperSpaceEffect.START_TRIGGER);

        sc?.PlayOneShot(sound);

        yield return new WaitForSeconds(duration);

        Move();

        animator.SetTrigger(HyperSpaceEffect.END_TRIGGER);

        // Prevent user from spamming the 'H' key
        yield return new WaitForSeconds(cooldownDuration + duration);

        isCooledDown = true;
    }

    private void Move()
    {
        // Set a new position & rotation
        rb.rotation = Random.Range(-maxRotate, maxRotate);

        rb.transform.position = new Vector2(
            Random.Range(-viewport.y, viewport.y),
            Random.Range(-viewport.x, viewport.x)
        );
    }

    private void PlayVFX()
    {
        // Animation event
        hyperSpaceVFX?.Play();
    }
}
