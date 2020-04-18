using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    public float spanwRate = 1f;

    private Coroutine spawner;

    public void StartSpawner()
    {

    }

    private IEnumerator Spawner()
    {
        for (;;)
        {
            yield return new WaitForSeconds(spanwRate);

            
        }
    }
}
