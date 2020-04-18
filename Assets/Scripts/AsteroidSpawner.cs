using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    private float spawnRate = 2f;
    private int spawnAmount = 2;
    private float xyOffset = 1;

    private Coroutine spawner;
    private Camera mainCam;

    private void Start()
    {
        mainCam = Camera.main;
    }

    public void StartSpawner(float spawnRate, int spawnAmount, float xyOffset = 1)
    {
        this.spawnRate = spawnRate;
        this.spawnAmount = spawnAmount;
        spawner = StartCoroutine(Spawner());
    }

    public void StopSpawner()
    {
        StopCoroutine(spawner);
        spawner = null;
    }

    private IEnumerator Spawner()
    {
        for (;;)
        {
            yield return new WaitForSeconds(spawnRate);

            Spawn();
        }
    }

    private void Spawn()
    {
        for (int i = 0; i < spawnAmount; ++i)
        {
            GameObject asteroid = ObjectPooler.Instance.GetPooledObject("Asteroid");

            float randomX = Random.Range(-xyOffset, xyOffset + 1);
            float randomY = Random.Range(-xyOffset, xyOffset + 1);

            while (randomY <= 1 && randomY >= 0 && randomX <= 1 && randomX >= 0)
            {
                randomY = Random.Range(-xyOffset, xyOffset + 1);
            }

            asteroid.transform.position = mainCam.ViewportToWorldPoint(new Vector3(randomX, randomY, mainCam.transform.position.y));
            AsteroidSize randomSize = (AsteroidSize)Random.Range(1, System.Enum.GetNames(typeof(AsteroidSize)).Length);
            asteroid.GetComponent<Asteroid>().Init(randomSize, GameManager.Instance.player.transform.position, false);
        }
    }
}
