using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class HyperSpaceEffect : MonoBehaviour
{
    private ParticleSystem particles;

    void Start()
    {
        particles = GetComponent<ParticleSystem>();
    }

    public void Play()
    {
        particles.Play();
    }
}