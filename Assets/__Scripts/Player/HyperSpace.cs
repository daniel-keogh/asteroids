using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(WeaponsController))]
[RequireComponent(typeof(PlayerMovement))]
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
    private SpriteRenderer spriteRenderer;
    private PolygonCollider2D polygonCollider;
    private WeaponsController weaponsController;
    private PlayerMovement playerMovement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        polygonCollider = GetComponent<PolygonCollider2D>();
        weaponsController = GetComponent<WeaponsController>();
        playerMovement = GetComponent<PlayerMovement>();

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

        DisableComonents();

        yield return new WaitForSeconds(duration);

        Move();

        EnableComponents();

        // Prevent user from spamming the 'H' key
        yield return new WaitForSeconds(cooldownDuration);
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

    private void DisableComonents()
    {
        SetComponentsEnabled(false);
    }

    private void EnableComponents()
    {
        SetComponentsEnabled(true);
    }

    private void SetComponentsEnabled(bool status)
    {
        spriteRenderer.enabled = status;
        polygonCollider.enabled = status;
        playerMovement.enabled = status;
        weaponsController.enabled = status;
    }

    private Vector3 GenerateRandomPosition()
    {
        return new Vector2(
            Random.Range(-sceneWidth, sceneWidth),
            Random.Range(-sceneHeight, sceneHeight)
        );
    }
}
