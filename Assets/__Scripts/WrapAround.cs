using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrapAround : MonoBehaviour
{
    private Vector3 sceneHeight, sceneWidth;
    private Vector2 viewport;

    void Start()
    {
        viewport = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        sceneHeight = new Vector2(0, viewport.y * 2);
        sceneWidth = new Vector2(viewport.x * 2, 0);
    }

    void Update()
    {
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
