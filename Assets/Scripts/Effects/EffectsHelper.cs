using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class EffectsHelper
{
    public static float SFX(string name)
    {
        AudioSource sfx = ObjectPooler.Instance.GetPooledObject("AudioSource").GetComponent<AudioSource>();
        sfx.clip = Resources.Load<AudioClip>("SFX/"+ name);
        sfx.gameObject.SetActive(true);
        sfx.Play();
        return sfx.clip.length;
    }
    public static void Particles(string name, Vector3 position)
    {
        ParticleSystem particleSystem = ObjectPooler.Instance.GetPooledObject(name).GetComponent<ParticleSystem>();
        particleSystem.transform.position = position;
        particleSystem.gameObject.SetActive(true);
        particleSystem.Play();
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
