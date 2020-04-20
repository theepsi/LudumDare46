using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleManager : MonoBehaviour
{
    public ModuleData[] availableModules;

    public float spawnRate = 2f;
    public int spawnAmount = 2;
    public float normalizedExtraScreen = 0.5f;

    private Coroutine spawner;
    private Camera mainCam;
    
    public void Init(Camera mainCam)
    {
        this.mainCam = mainCam;
    }

    public void StartSpawner()
    {
        spawner = StartCoroutine(Spawner());
    }

    public void StopSpawner()
    {
        if (spawner != null)
        {
            StopCoroutine(spawner);
            spawner = null;
        }
    }

    private IEnumerator Spawner()
    {
        for (; ; )
        {
            yield return new WaitForSeconds(spawnRate);

            Spawn();
        }
    }

    private void Spawn()
    {
        for (int i = 0; i < spawnAmount; ++i)
        {
            GameObject module = ObjectPooler.Instance.GetPooledObject("Module");

            Vector2 position = SpawnerHelper.SpawnPosition(normalizedExtraScreen);

            module.transform.position = mainCam.ViewportToWorldPoint(new Vector3(position.x, position.y, mainCam.transform.position.y));

            module.transform.rotation = Random.rotation;
            Vector3 eulerAngles = module.transform.eulerAngles;
            eulerAngles.x = 0;
            module.transform.eulerAngles = eulerAngles;

            //Select module based on Rarity, then initialize it.
            int moduleRarityInt = Random.Range(0, (int)ModuleRarity.ALL);
            ModuleRarity moduleRarity = ModuleRarity.COMMON;
            if (GameManager.Instance.onlyRare)
            {
                moduleRarity = ModuleRarity.RARE;
            }
            else
            {
                if (moduleRarityInt < (int)ModuleRarity.RARE)
                {
                    moduleRarity = ModuleRarity.RARE;
                }
                else if (moduleRarityInt < (int)ModuleRarity.UNCOMMON)
                {
                    moduleRarity = ModuleRarity.UNCOMMON;
                }
            }

            List<ModuleData> rarityModules = new List<ModuleData>();

            for (int j = 0; j < availableModules.Length; ++j)
            {
                if (availableModules[j].moduleRarity == moduleRarity) rarityModules.Add(availableModules[j]);
            }

            if (rarityModules.Count > 0)
            {
                int randModule = Random.Range(0, rarityModules.Count);

                module.GetComponent<Module>().Init(rarityModules[randModule]);
            }
        }
    }
}
