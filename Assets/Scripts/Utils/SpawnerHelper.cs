using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpawnLocation
{
    GENERAL = 33,
    DIRECTION = 67,
    ALL = GENERAL + DIRECTION
}

public static class SpawnerHelper
{
    public static Vector2 SpawnPosition(float normalizedExtraScreen)
    {
        Vector3 playerVelocityDirection = GameManager.Instance.player.GetVelocityDirection();

        int spawnLocationInt = Random.Range(0, (int)SpawnLocation.ALL);
        SpawnLocation spawnLocation = SpawnLocation.DIRECTION;
        if (!(GameManager.Instance.onlySpawnFrontWhenMoving && playerVelocityDirection != Vector3.zero))
        {
            if (spawnLocationInt < (int)SpawnLocation.GENERAL || playerVelocityDirection == Vector3.zero)
            {
                spawnLocation = SpawnLocation.GENERAL;
            }
        }

        float minX = -normalizedExtraScreen;
        float minY = -normalizedExtraScreen * 2;
        float maxX = normalizedExtraScreen + 1;
        float maxY = normalizedExtraScreen * 2 + 1;

        if (spawnLocation == SpawnLocation.DIRECTION)
        {
            float alpha = 45f;

            Vector3 normDir1 = new Vector3(Mathf.Cos(alpha) * playerVelocityDirection.x - Mathf.Sin(alpha) * playerVelocityDirection.z, 0, Mathf.Sin(alpha) * playerVelocityDirection.x + Mathf.Cos(alpha) * playerVelocityDirection.z).normalized;
            Vector3 normDir2 = new Vector3(Mathf.Cos(-alpha) * playerVelocityDirection.x - Mathf.Sin(-alpha) * playerVelocityDirection.z, 0, Mathf.Sin(-alpha) * playerVelocityDirection.x + Mathf.Cos(-alpha) * playerVelocityDirection.z).normalized;

            float minDirX = Mathf.Min(normDir1.x, normDir2.x);
            float minDirY = Mathf.Min(normDir1.z, normDir2.z);
            float maxDirX = Mathf.Max(normDir1.x, normDir2.x);
            float maxDirY = Mathf.Max(normDir1.z, normDir2.z);

            // Allow only points on X and Y that are between those directions (infinite)
            if (minDirX == maxDirX || (minDirX > 0 && maxDirX > 0) || (minDirX < 0 && maxDirX < 0))
            {
                if (minDirX > 0)
                {
                    minX = (minX + maxX) / 2;
                }
                else
                {
                    maxX = (minX + maxX) / 2;
                }
            }
            else
            {
                if (minDirX != -1)
                {
                    if (minDirX > 0)
                    {
                        minX = minDirX + normalizedExtraScreen / 2;
                    }
                    else if (minDirX < 0)
                    {
                        minX = minDirX - normalizedExtraScreen / 2;
                    }
                    else
                    {
                        minX = normalizedExtraScreen / 2;
                    }
                }
                if (maxDirX != 1)
                {
                    if (maxDirX > 0)
                    {
                        maxX = maxDirX + normalizedExtraScreen / 2;
                    }
                    else if (maxDirX < 0)
                    {
                        maxX = maxDirX - normalizedExtraScreen / 2;
                    }
                    else
                    {
                        maxX = normalizedExtraScreen / 2;
                    }
                }
            }

            if (minDirY == maxDirY || (minDirY > 0 && maxDirY > 0) || (minDirY < 0 && maxDirY < 0))
            {
                if (minDirY > 0)
                {
                    minY = (minX + maxX) / 2;
                }
                else
                {
                    maxY = (minX + maxX) / 2;
                }
            }
            else
            {
                if (minDirY != -1)
                {
                    if (minDirY > 0)
                    {
                        minY = minDirY + normalizedExtraScreen;
                    }
                    else if (minDirY < 0)
                    {
                        minY = minDirY - normalizedExtraScreen;
                    }
                    else
                    {
                        minY = normalizedExtraScreen;
                    }
                }
                if (maxDirY != 1)
                {
                    if (maxDirY > 0)
                    {
                        maxY = maxDirY + normalizedExtraScreen;
                    }
                    else if (maxDirY < 0)
                    {
                        maxY = maxDirY - normalizedExtraScreen;
                    }
                    else
                    {
                        maxY = normalizedExtraScreen;
                    }
                }
            }
        }

        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);

        while (randomY <= 1 && randomY >= 0 && randomX <= 1 && randomX >= 0)
        {
            randomY = Random.Range(minY, maxY);
        }

        return new Vector2(randomX, randomY);
    }
    public static bool OffScreen(Vector2 position, float normalizedExtraScreen)
    {
        return position.x < -normalizedExtraScreen || position.x > 1 + normalizedExtraScreen || position.y < -normalizedExtraScreen * 2 || position.y > 1 + normalizedExtraScreen * 2;
    }
}
