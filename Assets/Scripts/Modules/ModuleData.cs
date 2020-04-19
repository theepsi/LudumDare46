using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Module", menuName = "Ship Module")]
public class ModuleData : ScriptableObject
{
    public ModuleAction moduleAction;
    public ModuleRarity moduleRarity;

    public string moduleName;
    public string prefabName;
    public Material moduleMaterial;

    public bool textureEnabled;
    public Texture moduleTexture;

    public AudioClip onAttachedSound;
}
