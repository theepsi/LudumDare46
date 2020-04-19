﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ModuleManager : MonoBehaviour
{
    public ModuleData[] availableModules;

    public float spawnRate = 2f;
    public int spawnAmount = 2;
    public float xyOffset = 1;

    private Coroutine spawner;
    private Camera mainCam;

    private void Start()
    {
        mainCam = Camera.main;
    }

    public void StartSpawner()
    {
        spawner = StartCoroutine(Spawner());
    }

    public void StopSpawner()
    {
        StopCoroutine(spawner);
        spawner = null;
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

            float randomX = Random.Range(-xyOffset, xyOffset + 1);
            float randomY = Random.Range(-xyOffset, xyOffset + 1);

            while (randomY <= 1 && randomY >= 0 && randomX <= 1 && randomX >= 0)
            {
                randomY = Random.Range(-xyOffset, xyOffset + 1);
            }

            module.transform.position = mainCam.ViewportToWorldPoint(new Vector3(randomX, randomY, mainCam.transform.position.y));

            module.transform.rotation = Random.rotation;
            Vector3 eulerAngles = module.transform.eulerAngles;
            eulerAngles.x = 0;
            module.transform.eulerAngles = eulerAngles;

            //Select module based on Rarity, then initialize it.
            int moduleRarityInt = Random.Range(0, (int)ModuleRarity.ALL);
            ModuleRarity moduleRarity = ModuleRarity.COMMON;

            if (moduleRarityInt < (int)ModuleRarity.RARE)
            {
                Debug.Log("Spawn RARE module");
                moduleRarity = ModuleRarity.RARE;
            } else if (moduleRarityInt < (int)ModuleRarity.UNCOMMON)
            {
                Debug.Log("Spawn UNCOMMON module");
                moduleRarity = ModuleRarity.UNCOMMON;
            } else
            {
                Debug.Log("Spawn COMMON module");
            }

            moduleRarity = ModuleRarity.COMMON;

            List<ModuleData> rarityModules = new List<ModuleData>();

            for (int j = 0; j < availableModules.Length; ++j)
            {
                if (availableModules[j].moduleRarity == moduleRarity) rarityModules.Add(availableModules[j]);
            }

            int randModule = Random.Range(0, rarityModules.Count);

            module.GetComponent<Module>().Init(rarityModules[randModule]);
        }
    }
}
