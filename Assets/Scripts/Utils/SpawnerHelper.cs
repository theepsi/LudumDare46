using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SpawnerHelper
{
    public static Vector2 SpawnPosition(float normalizedExtraScreen)
    {
        float randomX = Random.Range(-normalizedExtraScreen, normalizedExtraScreen + 1);
        float randomY = Random.Range(-normalizedExtraScreen, normalizedExtraScreen + 1);

        while (randomY <= 1 && randomY >= 0 && randomX <= 1 && randomX >= 0)
        {
            randomY = Random.Range(-normalizedExtraScreen, normalizedExtraScreen + 1);
        }

        return new Vector2(randomX, randomY);
    }
    public static bool OffScreen(Vector2 position, float normalizedExtraScreen)
    {
        return position.x < -normalizedExtraScreen || position.x > 1 + normalizedExtraScreen || position.y < -normalizedExtraScreen || position.y > 1 + normalizedExtraScreen;
    }
}
