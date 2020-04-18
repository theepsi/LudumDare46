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
    public AsteroidSize currentSize = AsteroidSize.BIG;
    public Vector2 RandomForce;
    public Vector2 RandomOffsetFailure;

    private Rigidbody mRigidbody;

    private bool ready = false;
    private Vector3 targetDir;

    private void Awake()
    {
        mRigidbody = GetComponent<Rigidbody>();
        ready = false;
    }

    public void Init(AsteroidSize size, Vector3 target, bool fromBreak)
    {
        currentSize = size;
        ApplySize();

        if (!fromBreak)
        {
            Vector3 targetOffseted = target; //+ new Vector3(Random.Range(RandomOffsetFailure[0], RandomOffsetFailure[1]), 0, Random.Range(RandomOffsetFailure[0], RandomOffsetFailure[1]));
            targetDir = targetOffseted - transform.position;

            ready = true;
        }

        gameObject.SetActive(true);
    }

    private void FixedUpdate()
    {
        if (ready)
        {
            mRigidbody.AddForce(targetDir * Random.Range(RandomForce[0], RandomForce[1]), ForceMode.Impulse);
            ready = false;
        }

        if (CheckForDestruction())
        {
            ready = false;
            gameObject.SetActive(false);
        }
    }

    private void ApplySize()
    {
        switch (currentSize)
        {
            case AsteroidSize.SMALL:
                transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
                break;
            default:
            case AsteroidSize.NORMAL:
                transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
                break;
            case AsteroidSize.BIG:
                transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
                break;

        }
    }

    private void Break()
    {

    }

    private bool CheckForDestruction()
    {
        Vector2 vpPos = Camera.main.WorldToViewportPoint(transform.position);
        return vpPos.x < -1 || vpPos.x > 2 || vpPos.y < -1 || vpPos.y > 2;
    }
}
