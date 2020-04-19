using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ModuleAction
{
    NONE = 0,
    OXYGEN,
    ENERGY
}

public enum ModuleRarity
{
    COMMON = 0,
    UNCOMMON,
    RARE
}

[RequireComponent(typeof(BoxCollider))]
public class Module : MonoBehaviour
{
    private ModuleData data;
    public bool attached = false;

    //TODO: rest of the properties

    public void Init(ModuleData data)
    {
        this.data = data;

        //Initialize module object, waiting for art
    }

    public ModuleData GetData()
    {
        return data;
    }

    public void OnAttached()
    {
        //Play clip
        switch (data.moduleAction)
        {
            case ModuleAction.NONE:
                Debug.Log("Module NONE Attached");
                break;
            case ModuleAction.OXYGEN:
                Debug.Log("Module OXYGEN Attached");
                break;
            case ModuleAction.ENERGY:
                Debug.Log("Module ENERGY Attached");
                break;
        }
    }

    public void OnDettached()
    {
        //Play clip
        switch (data.moduleAction)
        {
            case ModuleAction.NONE:
                Debug.Log("Module NONE Dettached");
                break;
            case ModuleAction.OXYGEN:
                Debug.Log("Module OXYGEN Dettached");
                break;
            case ModuleAction.ENERGY:
                Debug.Log("Module ENERGY Dettached");
                break;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            EventManager.TriggerEvent(Statics.Events.moduleHitPlayer, this);
        }
        else if (other.CompareTag("Asteroid") && attached)
        {
            EventManager.TriggerEvent(Statics.Events.moduleHitAsteroid, this);
        }
    }
}
