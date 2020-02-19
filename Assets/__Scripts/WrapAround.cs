﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrapAround : MonoBehaviour
{
    [SerializeField] private float sceneHeight = 13.2f;
    [SerializeField] private float sceneWidth = 16.7f;

    void Update()
    {
        var position = new Vector2(transform.position.x, transform.position.y);

        if (transform.position.y > sceneHeight || transform.position.y < -sceneHeight)
        {
            position.y = -transform.position.y;
        }
        if (transform.position.x > sceneWidth || transform.position.x < -sceneWidth)
        {
            position.x = -transform.position.x;
        }

        transform.position = position;
    }
}
