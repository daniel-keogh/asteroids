using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class LifeIcon : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void SetAnimatorEnabled(bool status)
    {
        GetComponent<Animator>().enabled = status;
    }
}
