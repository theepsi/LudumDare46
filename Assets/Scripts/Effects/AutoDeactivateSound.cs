using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDeactivateSound : MonoBehaviour
{
    AudioSource ase = null;

    // Start is called before the first frame update
    void Start()
    {
        ase = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ase != null && !ase.isPlaying) gameObject.SetActive(false);
    }
}
