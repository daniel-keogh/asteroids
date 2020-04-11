using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrapAround : MonoBehaviour
{
    private Vector3 sceneHeight, sceneWidth;
    private Vector2 viewport;

    void Start()
    {
        // The bottom-left of the viewport is (0,0); the top-right is (1,1)
        viewport = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        // Measure screen width & height
        sceneHeight = new Vector2(0, viewport.y * 2);
        sceneWidth = new Vector2(viewport.x * 2, 0);
    }

    void Update()
    {
        Wrap();
    }

    private void Wrap()
    {
        // If the `transform.position` is off-screen, 
        // try to place the object on the other side.

        if (transform.position.y > viewport.y)
        {
            transform.position -= sceneHeight;
        }
        else if (transform.position.y < -viewport.y)
        {
            transform.position += sceneHeight;
        }

        if (transform.position.x > viewport.x)
        {
            transform.position -= sceneWidth;
        }
        else if (transform.position.x < -viewport.x)
        {
            transform.position += sceneWidth;
        }
    }
}
