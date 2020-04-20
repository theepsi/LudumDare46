using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class EffectsHelper
{
    public static void SFX(string name)
    {
        AudioSource crash = ObjectPooler.Instance.GetPooledObject("AudioSource").GetComponent<AudioSource>();
        crash.clip = Resources.Load<AudioClip>("SFX/"+ name);
        crash.gameObject.SetActive(true);
        crash.Play();
    }
    public static void Particles(string name, Vector3 position)
    {
        ParticleSystem moduleCrash = ObjectPooler.Instance.GetPooledObject(name).GetComponent<ParticleSystem>();
        moduleCrash.transform.position = position;
        moduleCrash.gameObject.SetActive(true);
        moduleCrash.Play();
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
