using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class HyperSpace : MonoBehaviour
{
    [SerializeField] private bool disableRotation;

    private float maxRotate = 360;
    private float sceneHeight, sceneWidth;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // The bottom-left of the viewport is (0,0); the top-right is (1,1).
        var viewport = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        sceneHeight = viewport.y * 2;
        sceneWidth = viewport.x * 2;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            rb.transform.position = GenerateRandomLocation();

            if (!disableRotation)
            {
                rb.rotation = Random.Range(-maxRotate, maxRotate);
            }
        }
    }

    private Vector2 GenerateRandomLocation()
    {
        return new Vector2(
            Random.Range(-sceneWidth, sceneWidth),
            Random.Range(-sceneHeight, sceneHeight)
        );
    }
}
