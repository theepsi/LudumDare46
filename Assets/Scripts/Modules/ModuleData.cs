using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Module", menuName = "Ship Module")]
public class ModuleData : ScriptableObject
{
    public ModuleAction moduleAction;
    public ModuleRarity moduleRarity;

    public string moduleName;
    public Material moduleMaterial;

    public bool textureEnabled;
    public Material moduleDecal;

    public AudioClip onAttachedSound;
}
