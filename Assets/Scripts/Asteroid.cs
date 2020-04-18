using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AsteroidSize
{
    SMALL = 0,
    NORMAL,
    BIG
}

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public class Asteroid : MonoBehaviour
{
    public AsteroidSize currentSize;
    public Vector2 RandomForce;
    public Vector2 RandomOffsetFailure;

    private Rigidbody mRigidbody;

    private void Awake()
    {
        mRigidbody = GetComponent<Rigidbody>();
    }

    public void Init(AsteroidSize size, Vector3 target, bool fromBreak)
    {
        currentSize = size;
        ApplySize();

        if (!fromBreak)
        {
            Vector3 targetOffseted = target + new Vector3(Random.Range(RandomOffsetFailure[0], RandomOffsetFailure[1]), 0, Random.Range(RandomOffsetFailure[0], RandomOffsetFailure[1]));
            Vector3 targetDir = targetOffseted - transform.position;

            ApplyRandomDirectionAndSpeed(targetDir);
        }
    }

    private void ApplyRandomDirectionAndSpeed(Vector3 targetDir)
    {
        mRigidbody.AddForce(targetDir * Random.Range(RandomForce[0], RandomForce[1]));
    }

    private void ApplySize()
    {
        switch (currentSize)
        {
            case AsteroidSize.SMALL:
                transform.localScale = new Vector3(1, 1, 1);
                break;
            case AsteroidSize.NORMAL:
                transform.localScale = new Vector3(3, 3, 3);
                break;
            case AsteroidSize.BIG:
                transform.localScale = new Vector3(7, 7, 7);
                break;
        }
    }

    private void Break()
    {

    }
}
