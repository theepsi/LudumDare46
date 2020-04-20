using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.VFX;

public static class EffectsHelper
{
    public static float SFX(string name)
    {
        AudioSource sfx = ObjectPooler.Instance.GetPooledObject("AudioSource").GetComponent<AudioSource>();
        sfx.clip = Resources.Load<AudioClip>("SFX/" + name);
        sfx.loop = false;
        sfx.gameObject.SetActive(true);
        sfx.Play();
        return sfx.clip.length;
    }

    public static AudioSource SFXLoop(string name)
    {
        AudioSource sfx = ObjectPooler.Instance.GetPooledObject("AudioSource").GetComponent<AudioSource>();
        sfx.clip = Resources.Load<AudioClip>("SFX/" + name);
        sfx.loop = true;
        sfx.gameObject.SetActive(true);
        sfx.Play();
        return sfx;
    }

    public static void Particles(string name, Vector3 position)
    {
        ParticleSystem particleSystem = ObjectPooler.Instance.GetPooledObject(name).GetComponent<ParticleSystem>();
        particleSystem.transform.position = position;
        particleSystem.gameObject.SetActive(true);
        particleSystem.Play();
    }
    public static VisualEffect GasParticles(GameObject parent, string name)
    {
        VisualEffect particleSystem = Resources.Load<VisualEffect>("Particles/" + name);
        VisualEffect ps = Object.Instantiate(particleSystem, parent.transform);

        ps.transform.position = parent.transform.position;
        ps.transform.rotation = parent.transform.rotation;

        ps.Stop();

        return ps;
    }

    public static AudioSource Music(string name) {
        AudioSource audioSource = ObjectPooler.Instance.GetPooledObject("AudioSource").GetComponent<AudioSource>();
        audioSource.clip = Resources.Load<AudioClip>("Music/" + name);
        audioSource.gameObject.SetActive(true);

        audioSource.loop = true;

        audioSource.Play();

        return audioSource;
    }
}
