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
    COMMON = 60,
    UNCOMMON = 30,
    RARE = 10,
    ALL = COMMON + UNCOMMON + RARE
}

[RequireComponent(typeof(BoxCollider))]
public class Module : MonoBehaviour
{
    private ModuleData data;
    public bool attached = false;

    public MeshRenderer pipeRenderer;

    public MeshRenderer[] decalRenderers;
    public GameObject decalParent;

    private float normalizedExtraScreen;

    //TODO: rest of the properties

    //Oxygen
    private float oxygenModuleAmount = 10f;
    private Coroutine oxygenModule;
    private float oxygenRate = 0.5f;
    private float oxygenAmount = 1f;

    //Energy
    private Coroutine energyModule;
    private LineRenderer lineRenderer = null;

    public void Init(ModuleData data, float normalizedExtraScreen)
    {
        this.data = data;

        this.normalizedExtraScreen = normalizedExtraScreen;

        pipeRenderer.material = data.moduleMaterial;
        decalParent.SetActive(data.textureEnabled);

        if (data.textureEnabled && data.moduleDecal != null)
        {
            for (int i = 0; i < decalRenderers.Length; ++i)
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
        EventManager.TriggerEvent(Statics.Events.moduleDistroy);
    }

    public ModuleData GetData()
    {
        return data;
    }

    public void OnAttached(PlayerController player, GameObject slot)
    {
        float duration = EffectsHelper.SFX("_AttachModule");

        transform.SetParent(slot.transform, false);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        attached = true;

        switch (data.moduleAction)
        {
            case ModuleAction.NONE:
                break;
            case ModuleAction.OXYGEN:
                Debug.Log("Module OXYGEN Attached");
                StartCoroutine(StartSFXDelayed("_OxygenModule", duration/2));
                oxygenModuleAmount = 10f;
                oxygenModule = StartCoroutine(OxygenDeployment(player));
                break;
            case ModuleAction.ENERGY:
                Debug.Log("Module ENERGY Attached");
                StartCoroutine(StartSFXDelayed("_EnergyModule", duration / 2));
                if (energyModule == null) energyModule = StartCoroutine(EnergyRadar(player));
                break;
        }
    }

    public void OnDettached()
    {
        float duration = EffectsHelper.SFX("_DettachModule");
        transform.SetParent(null);

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
                StopCoroutine(energyModule);
                Destroy(lineRenderer.gameObject);
                energyModule = null;
                lineRenderer = null;
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
            EffectsHelper.SFX("_AsteroidModuleCrash");
            EffectsHelper.Particles("ModuleCrash", transform.position);

            other.GetComponent<Asteroid>().DestroyAsteroid();
            EventManager.TriggerEvent(Statics.Events.moduleHitAsteroid, this);
        }
        else if (other.CompareTag("Base"))
        {
            DestroyModule();
        }
    }

    private bool CheckForDestruction()
    {
        return SpawnerHelper.OffScreen(Camera.main.WorldToViewportPoint(transform.position), normalizedExtraScreen);
    }

    private IEnumerator StartSFXDelayed(string name, float duration)
    {
        yield return new WaitForSeconds(duration);

        EffectsHelper.SFX(name);
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

    #region ENERGY

    private IEnumerator EnergyRadar(PlayerController player)
    {
        GameObject line = Instantiate(GameManager.Instance.linePrefab.gameObject);

        lineRenderer = line.GetComponent<LineRenderer>();

        for (; ; )
        {
            //Point nearest base
            lineRenderer.SetPosition(0, player.transform.position);
            lineRenderer.SetPosition(1, GameManager.Instance.NearestBasePosition());

            line.SetActive(true);

            yield return new WaitForEndOfFrame();
        }
    }

    #endregion
}
