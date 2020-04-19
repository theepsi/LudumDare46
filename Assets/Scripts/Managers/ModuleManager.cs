using System.Collections;
using System.Collections.Generic;
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

            //GetRandom module based on Rarity, then initialize it.
            //module.GetComponent<Module>().Init();
            module.GetComponent<Module>().Init(availableModules[0]);
        }
    }
}
