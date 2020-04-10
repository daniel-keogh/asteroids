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

    [SerializeField] private float gizmoRadius = 0.25f;

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, gizmoRadius);
    }
}
