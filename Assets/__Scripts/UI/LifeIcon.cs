using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class LifeIcon : MonoBehaviour
{
    public void SetAnimationEnabled(bool status)
    {
        GetComponent<Animator>().enabled = status;
    }
}
