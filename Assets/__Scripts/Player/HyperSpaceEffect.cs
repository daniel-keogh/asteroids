using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class HyperSpaceEffect : MonoBehaviour
{
    // Animation triggers
    public const string START_TRIGGER = "Start";
    public const string END_TRIGGER = "End";

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