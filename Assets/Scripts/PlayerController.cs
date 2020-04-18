using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class PlayerController : MonoBehaviour
{
    public float impulseSpeed = 2.2f;
    public float rotateSpeed = 85f;

    public float minOxygen = 0;
    public float maxOxygen = 10;

    public float minHull = 0;
    public float maxHull = 4;

    [Tooltip("Deplition rate in seconds")]
    public float oxygenDeplitionRate = 1f;
    public float oxygenDeplitionAmount = 1f;

    private float currentHull;
    private float currentOxygen;

    private Rigidbody mRigidbody;

    private Coroutine oxygenDeplition;

    void Start()
    {
        mRigidbody = GetComponent<Rigidbody>();
        currentHull = maxHull;
        currentOxygen = maxOxygen;

        oxygenDeplition = StartCoroutine(OxygenDeplition());
    }

    void FixedUpdate()
    {
        RotateShip(Input.GetAxis("Horizontal"));

        ImpulseShip(Input.GetAxis("Vertical"));
    }

    private void RotateShip(float direction)
    {
        if (direction != 0)
        {
            Quaternion rotation = Quaternion.AngleAxis(direction * rotateSpeed * Time.deltaTime, Vector3.up);
            transform.forward = rotation * transform.forward;
        }
    }

    private void ImpulseShip(float acceleration)
    {
        if (acceleration != 0)
        {
            mRigidbody.velocity += transform.forward * impulseSpeed * Time.deltaTime * acceleration;
        }
    }

    public void DoDamage(int damageAmount)
    {
        currentHull -= damageAmount;

        if(currentHull <= 0)
        {
            currentHull = 0;
            EventManager.TriggerEvent(Statics.Events.gameOver);
        }

        EventManager.TriggerEvent(Statics.Events.hullDamaged, currentHull);
    }

    private IEnumerator OxygenDeplition()
    {
        for (; ; )
        {
            yield return new WaitForSeconds(oxygenDeplitionRate);

            currentOxygen -= oxygenDeplitionAmount;

            if (currentOxygen <= 0)
            {
                currentOxygen = 0;
                EventManager.TriggerEvent(Statics.Events.gameOver);
            }

            EventManager.TriggerEvent(Statics.Events.oxygenLost, currentOxygen);
        }
    }
}
