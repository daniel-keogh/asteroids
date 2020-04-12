using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    // Tags
    public const string TOP = "SpawnPointTop";
    public const string BOTTOM = "SpawnPointBottom";
    public const string LEFT = "SpawnPointLeft";
    public const string RIGHT = "SpawnPointRight";

    [Tooltip("The SpawnPoint's offset from the edge of the scene.\n\nStops things from Wrapping immediately as they are spawned.")]
    [SerializeField] private float borderPadding = 0.01f;
    [SerializeField] private float gizmoRadius = 0.25f;

    void Update()
    {
        SnapToEdge();
    }

    private void SnapToEdge()
    {
        var viewport = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        // Make sure SpawnPoints are always situated on the edge of the screen, 
        // even if the screen resolution changes.
        switch (this.tag)
        {
            case SpawnPoint.TOP:
                transform.position = new Vector2(transform.position.x, viewport.y - borderPadding);
                break;
            case SpawnPoint.BOTTOM:
                transform.position = new Vector2(transform.position.x, -viewport.y + borderPadding);
                break;
            case SpawnPoint.LEFT:
                transform.position = new Vector2(-viewport.x + borderPadding, transform.position.y);
                break;
            case SpawnPoint.RIGHT:
                transform.position = new Vector2(viewport.x - borderPadding, transform.position.y);
                break;
            default:
                break;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, gizmoRadius);
    }
}
