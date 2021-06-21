using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int ScoreValue
    {
        get => scoreValue;
        set => scoreValue = Mathf.Abs(value);
    }

    [Header("Scoring")]
    [SerializeField] private int scoreValue = 0;

    // Delegate type to use for event
    public delegate void EnemyDestroyed(Enemy enemy);

    // Static method to be implemented in the listener
    public static EnemyDestroyed EnemyDestroyedEvent;
}
