using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Laser : MonoBehaviour
{
    // Tags
    public const string PLAYER_LASER = "PlayerLaser";
    public const string ENEMY_LASER = "EnemyLaser";

    public void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
