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
    COMMON = 50,
    UNCOMMON = 30,
    RARE = 20
}

[RequireComponent(typeof(BoxCollider))]
public class Module : MonoBehaviour
{
    private ModuleData data;
    public bool attached = false;

    public MeshRenderer pipeRenderer;

    public MeshRenderer[] decalRenderers;
    public GameObject decalParent;

    //TODO: rest of the properties

    //Oxygen
    private float oxygenModuleAmount = 10f;
    private Coroutine oxygenModule;
    private float oxygenRate = 0.5f;
    private float oxygenAmount = 1f;

    public void Init(ModuleData data)
    {
        this.data = data;

        pipeRenderer.material = data.moduleMaterial;
        decalParent.SetActive(data.textureEnabled);

        if (data.textureEnabled && data.moduleDecal != null)
        {
            for(int i = 0; i < decalRenderers.Length; ++i)
            {
                decalRenderers[i].material = data.moduleDecal;
            }
        }

        gameObject.SetActive(true);
    }

    private void Update()
    {
        if (CheckForDestruction() && !attached)
        {
            DestroyModule();
        }
    }

    private void DestroyModule()
    {
        gameObject.SetActive(false);
    }

    public ModuleData GetData()
    {
        return data;
    }

    public void OnAttached(PlayerController player, GameObject slot)
    {
        transform.SetParent(slot.transform, false);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        attached = true;

        //Play clip
        switch (data.moduleAction)
        {
            case ModuleAction.NONE:
                Debug.Log("Module NONE Attached");
                break;
            case ModuleAction.OXYGEN:
                Debug.Log("Module OXYGEN Attached");
                oxygenModuleAmount = 10f;
                oxygenModule = StartCoroutine(OxygenDeployment(player));

                break;
            case ModuleAction.ENERGY:
                Debug.Log("Module ENERGY Attached");
                break;
        }
    }

    public void OnDettached()
    {
        transform.SetParent(null);

        //Play clip?
        switch (data.moduleAction)
        {
            case ModuleAction.NONE:
                Debug.Log("Module NONE Dettached");
                break;
            case ModuleAction.OXYGEN:
                Debug.Log("Module OXYGEN Dettached");

                StopCoroutine(oxygenModule);
                oxygenModule = null;

                break;
            case ModuleAction.ENERGY:
                Debug.Log("Module ENERGY Dettached");
                break;
        }

        gameObject.SetActive(false);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            EventManager.TriggerEvent(Statics.Events.moduleHitPlayer, this);
        }
        else if (other.CompareTag("Asteroid") && attached)
        {
            other.GetComponent<Asteroid>().DestroyAsteroid();
            EventManager.TriggerEvent(Statics.Events.moduleHitAsteroid, this);
        }
    }

    private bool CheckForDestruction()
    {
        Vector2 vpPos = Camera.main.WorldToViewportPoint(transform.position);
        return vpPos.x < -1 || vpPos.x > 2 || vpPos.y < -1 || vpPos.y > 2;
    }

    #region OXYGEN

    private IEnumerator OxygenDeployment(PlayerController player)
    {
        while (oxygenModuleAmount > 0)
        {
            if (player.GetCurrentOxygen() + oxygenAmount <= player.maxOxygen)
            {
                oxygenModuleAmount -= oxygenAmount;
                player.AddOxygenAmount(oxygenAmount);
            }

            yield return new WaitForSeconds(oxygenRate);
        }

        OnDettached();
    }

    #endregion
}
