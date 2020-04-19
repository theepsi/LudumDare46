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

    private Module module1 = null;
    private Module module2 = null;

    void Start()
    {
        mRigidbody = GetComponent<Rigidbody>();
        currentHull = maxHull;
        currentOxygen = maxOxygen;

        oxygenDeplition = StartCoroutine(OxygenDeplition());

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

    private void OnModuleHitPlayer(object value)
    {
        Module module = (Module)value;

        if (module1 == null)
        {
            Debug.Log($"Module 1 attached - {module.GetData().name}");
            module1 = module;
            module1.transform.SetParent(transform, false);
            module1.attached = true;
        }
        else if (module2 == null)
        {
            Debug.Log($"Module 2 attached - {module.GetData().name}");
            module2 = module;
            module2.transform.SetParent(transform, false);
            module2.attached = true;
        }
        else
        {
            Debug.Log("Cannot attach module");
        }

        if (module.attached)
        {
            //Sound of module attaching, on finish do next:
            module.OnAttached();
        }
    }

    private void OnModuleHitAsteroid(object value)
    {
        Module module = (Module)value;
        
        if (module == module1)
        {
            module.transform.SetParent(null);
            module.OnDettached();
            module1 = null;
            module.gameObject.SetActive(false);
        }
        if (module == module2)
        {
            module.transform.SetParent(null);
            module.OnDettached();
            module2 = null;
            module.gameObject.SetActive(false);
        }
    }
}
