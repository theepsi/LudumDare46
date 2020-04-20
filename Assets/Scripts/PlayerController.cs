using System.Collections;
using UnityEngine;
using UnityEngine.VFX;
using static UnityEngine.ParticleSystem;

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

    public GameObject frontGas01;
    public GameObject frontGas02;
    public GameObject frontLeft;
    public GameObject frontRight;
    public GameObject backGas01;
    public GameObject backGas02;

    private VisualEffect frontGas01PS;
    private VisualEffect frontGas02PS;
    private VisualEffect frontLeftPS;
    private VisualEffect frontRightPS;
    private VisualEffect backGas01PS;
    private VisualEffect backGas02PS;

    private bool gasOn;
    private bool leftOn;
    private bool rightOn;
    private bool frontOn;

    private AudioSource frontGas1SsfxSource;
    private AudioSource frontGas2SsfxSource;
    private AudioSource frontGasLSsfxSource;
    private AudioSource frontGasRSsfxSource;
    private AudioSource frontGasAsfxSource;
    private AudioSource frontGasDsfxSource;
    private AudioSource backGassfxSource;

    void Start()
    {
        gasOn = false;
        leftOn = false;
        rightOn = false;
        frontOn = false;

        mRigidbody = GetComponent<Rigidbody>();
        currentHull = maxHull;
        currentOxygen = maxOxygen;

        if (!GameManager.Instance.godMode) oxygenDeplition = StartCoroutine(OxygenDeplition());

        EventManager.StartListening(Statics.Events.moduleHitPlayer, (x) => OnModuleHitPlayer(x));
        EventManager.StartListening(Statics.Events.moduleHitAsteroid, (x) => OnModuleHitAsteroid(x));

        InstantiateGases();
    }

    private void InstantiateGases()
    {
        frontGas01PS = EffectsHelper.GasParticles(frontGas01, "Gas");
        frontGas02PS = EffectsHelper.GasParticles(frontGas02, "Gas");
        frontLeftPS = EffectsHelper.GasParticles(frontLeft, "Gas");
        frontRightPS = EffectsHelper.GasParticles(frontRight, "Gas");
        backGas01PS = EffectsHelper.GasParticles(backGas01, "BigSmoke");
        backGas02PS = EffectsHelper.GasParticles(backGas02, "BigSmoke");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && moduleSlot1.GetComponentInChildren<Module>() != null)
        {
            moduleSlot1.GetComponentInChildren<Module>().OnDettached();
        }

        if (Input.GetKeyDown(KeyCode.E) && moduleSlot2.GetComponentInChildren<Module>() != null)
        {
            moduleSlot2.GetComponentInChildren<Module>().OnDettached();
        }

        if (Input.GetKeyUp(KeyCode.S) && gasOn)
        {
            gasOn = false;
            StopGas(frontGas01PS, frontGas1SsfxSource);
            StopGas(frontGas02PS, frontGas2SsfxSource);
            StopGas(frontLeftPS, frontGasLSsfxSource);
            StopGas(frontRightPS, frontGasRSsfxSource);
            StopGas(backGas01PS, backGassfxSource, backGas02PS);
        }

        if (Input.GetKeyDown(KeyCode.S) && !gasOn)
        {
            gasOn = true;
            frontGas1SsfxSource = StartFrontGas(frontGas01PS);
            frontGas2SsfxSource = StartFrontGas(frontGas02PS);
            frontGasLSsfxSource = StartFrontGas(frontLeftPS);
            frontGasRSsfxSource = StartFrontGas(frontRightPS);
            backGassfxSource = StartBackGas(backGas01PS, backGas02PS);
        }

        if (Input.GetKeyDown(KeyCode.A) && !leftOn)
        {
            leftOn = true;
            frontGasAsfxSource = StartFrontGas(frontRightPS);
        }

        if (Input.GetKeyUp(KeyCode.A) && leftOn)
        {
            leftOn = false;
            StopGas(frontRightPS, frontGasAsfxSource);
        }

        if (Input.GetKeyDown(KeyCode.D) && !rightOn)
        {
            rightOn = true;
            frontGasDsfxSource = StartFrontGas(frontLeftPS);
        }

        if (Input.GetKeyUp(KeyCode.D) && rightOn)
        {
            rightOn = false;
            StopGas(frontLeftPS, frontGasDsfxSource);
        }

        if (Input.GetKeyDown(KeyCode.W) && !frontOn)
        {
            frontOn = true;
            backGassfxSource = StartBackGas(backGas01PS, backGas02PS);
        }

        if (Input.GetKeyUp(KeyCode.W) && frontOn)
        {
            frontOn = false;
            backGas01PS.Stop();
            backGas02PS.Stop();
            StopGas(backGas01PS, backGassfxSource, backGas01PS);
        }
    }

    void FixedUpdate()
    {
        RotateShip(Input.GetAxis("Horizontal"));

        ImpulseShip(Input.GetAxis("Vertical"));
    }

    public Vector3 GetVelocityDirection()
    {
        return mRigidbody.velocity.normalized;
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
        if (acceleration == 1)
        {
            mRigidbody.velocity += transform.forward * impulseSpeed * Time.deltaTime * (acceleration * 2);
            float velocityX = 0;
            if (mRigidbody.velocity.x < 0)
            {
                velocityX = Mathf.Max(mRigidbody.velocity.x, -maxSpeed);
            }
            else
            {
                velocityX = Mathf.Min(mRigidbody.velocity.x, maxSpeed);
            }
            float velocityZ = 0;
            if (mRigidbody.velocity.z < 0)
            {
                velocityZ = Mathf.Max(mRigidbody.velocity.z, -maxSpeed);
            }
            else
            {
                velocityZ = Mathf.Min(mRigidbody.velocity.z, maxSpeed);
            }
            mRigidbody.velocity = new Vector3(velocityX, 0, velocityZ);
        }
        else if (acceleration == -1)
        {
            mRigidbody.velocity = mRigidbody.velocity * 0.95f;
            Vector3 newVelocity = mRigidbody.velocity;

            if (Mathf.Abs(newVelocity.x) < 0.001f)
            {
                newVelocity.x = 0;
            }

            if (Mathf.Abs(newVelocity.z) < 0.001f)
            {
                newVelocity.z = 0;
            }
            mRigidbody.velocity = newVelocity;
        }
    }

    public void DoDamage(int damageAmount)
    {
        if (GameManager.Instance.godMode) return;

        EffectsHelper.SFX("_AsteroidShipCrash");
        EffectsHelper.Particles("ShipCrash", transform.position);

        currentHull -= damageAmount;

        if (currentHull <= 0)
        {
            StopAllGases();
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
                StopAllGases();
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
            module.OnAttached(this, moduleSlot1);
        }
        else if (moduleSlot2.transform.childCount == 0)
        {
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Base"))
        {
            EventManager.TriggerEvent(Statics.Events.baseFound);
        }
    }

    private AudioSource StartFrontGas(VisualEffect gas)
    {
        gas.Play();
        return EffectsHelper.SFXLoop("_VaporDavantLoop_mig");
    }

    private AudioSource StartBackGas(VisualEffect gas1, VisualEffect gas2)
    {
        gas1.Play();
        gas2.Play();
        return EffectsHelper.SFXLoop("_MotorEspacialLoop");
    }

    private void StopGas(VisualEffect gas, AudioSource sfxSource, VisualEffect gas2 = null)
    {
        gas.Stop();
        if (gas2 != null) gas2.Stop();
        sfxSource.Stop();
    }

    private void StopAllGases()
    {
        frontGas1SsfxSource?.Stop();
        frontGas2SsfxSource?.Stop();
        frontGasLSsfxSource?.Stop();
        frontGasRSsfxSource?.Stop();
        frontGasAsfxSource?.Stop();
        frontGasDsfxSource?.Stop();
        backGassfxSource?.Stop();
    }
}
