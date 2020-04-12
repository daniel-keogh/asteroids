using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrapAround : MonoBehaviour
{
    void Update()
    {
        Wrap();
    }

    private void Wrap()
    {
        // The bottom-left of the viewport is (0,0); the top-right is (1,1)
        Vector2 viewport = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        // Measure screen width & height
        Vector3 sceneHeight = new Vector2(0, viewport.y * 2);
        Vector3 sceneWidth = new Vector2(viewport.x * 2, 0);

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
