using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class HyperSpace : MonoBehaviour
{
    [SerializeField] private bool disableRotation;
    [SerializeField] private float duration;
    [SerializeField] private float cooldownDuration;

    private float maxRotate = 360;
    private bool isCooledDown = true;
    private float sceneHeight;
    private float sceneWidth;
    private Rigidbody2D rb;
    private Animator animator;

    private const string START_TRIGGER = "Start";
    private const string END_TRIGGER = "End";

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // The bottom-left of the viewport is (0,0); the top-right is (1,1).
        var viewport = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        sceneHeight = viewport.y * 2;
        sceneWidth = viewport.x * 2;
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

    private IEnumerator HyperSpaceCoroutine()
    {
        isCooledDown = false;

        animator.SetTrigger(START_TRIGGER);
        yield return new WaitForSeconds(duration);

        Move();

        animator.SetTrigger(END_TRIGGER);
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
            Random.Range(-sceneWidth, sceneWidth),
            Random.Range(-sceneHeight, sceneHeight)
        );
    }
}
