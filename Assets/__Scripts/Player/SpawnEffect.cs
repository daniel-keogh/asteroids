using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class SpawnEffect : MonoBehaviour
{
    private ParticleSystem particles;
    private ParticleSystem.SizeOverLifetimeModule sizeOverLifetime;

    void Start()
    {
        particles = GetComponent<ParticleSystem>();
        sizeOverLifetime = particles.sizeOverLifetime;

        sizeOverLifetime.enabled = true;
    }

    public void PlaySpawnIn()
    {
        AnimationCurve curve = new AnimationCurve();
        curve.AddKey(0f, 1f);
        curve.AddKey(1f, 0f);

        sizeOverLifetime.size = new ParticleSystem.MinMaxCurve(1.0f, curve);

        particles.Play();
    }

    public void PlaySpawnOut()
    {
        AnimationCurve curve = new AnimationCurve();
        curve.AddKey(1f, 0f);
        curve.AddKey(0f, 1f);

        sizeOverLifetime.size = new ParticleSystem.MinMaxCurve(1.0f, curve);

        particles.Play();
    }
}
