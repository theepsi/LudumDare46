﻿using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public float impulseSpeed = 2.2f;
    public float rotateSpeed = 85f;

    public float minOxygen = 0;
    public float maxOxygen = 20;

    public float minHull = 0;
    public float maxHull = 4;

    public float maxSpeed = 1f;

    [Tooltip("Deplition rate in seconds")]
    public float oxygenDeplitionRate = 1f;
    public float oxygenDeplitionAmount = 1f;

    private float currentHull;
    private float currentOxygen;

    private Rigidbody mRigidbody;

    private Coroutine oxygenDeplition;

    public GameObject moduleSlot1;
    public GameObject moduleSlot2;

    void Start()
    {
        mRigidbody = GetComponent<Rigidbody>();
        currentHull = maxHull;
        currentOxygen = maxOxygen;

        if (!GameManager.Instance.godMode) oxygenDeplition = StartCoroutine(OxygenDeplition());

        EventManager.StartListening(Statics.Events.moduleHitPlayer, (x) => OnModuleHitPlayer(x));
        EventManager.StartListening(Statics.Events.moduleHitAsteroid, (x) => OnModuleHitAsteroid(x));
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
            mRigidbody.velocity = new Vector3(Mathf.Min(mRigidbody.velocity.x, maxSpeed), 0, Mathf.Min(mRigidbody.velocity.z, maxSpeed));
        }
    }

    public void DoDamage(int damageAmount)
    {
        if (GameManager.Instance.godMode) return;

        currentHull -= damageAmount;

        if (currentHull <= 0)
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

    private void OnModuleHitPlayer(object value)
    {
        Module module = (Module)value;

        if (moduleSlot1.transform.childCount == 0)
        {
            Debug.Log($"Module 1 attached - {module.GetData().name}");

            module.OnAttached(this, moduleSlot1);
        }
        else if (moduleSlot2.transform.childCount == 0)
        {
            Debug.Log($"Module 2 attached - {module.GetData().name}");

            module.OnAttached(this, moduleSlot2);
        }
        else
        {
            Debug.Log("Cannot attach module");
        }
    }

    private void OnModuleHitAsteroid(object value)
    {
        Module module = (Module)value;

        module.attached = false;

        if (module == moduleSlot1.GetComponentInChildren<Module>() || module == moduleSlot2.GetComponentInChildren<Module>())
        {
            module.OnDettached();
        }
    }

    public void StopOxygenCoroutine()
    {
        if (oxygenDeplition != null)
        {
            StopCoroutine(oxygenDeplition);
            oxygenDeplition = null;
        }
    }

    public float GetCurrentOxygen()
    {
        return currentOxygen;
    }

    public void AddOxygenAmount(float amount)
    {
        currentOxygen += amount;

        EventManager.TriggerEvent(Statics.Events.oxygenLost, currentOxygen);
    }
}
