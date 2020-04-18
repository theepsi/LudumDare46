using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class PlayerController : MonoBehaviour
{
    public float impulseSpeed = 2.2f;
    public float rotateSpeed = 85f;

    private Rigidbody mRigidbody;

    void Start()
    {
        mRigidbody = GetComponent<Rigidbody>();
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
}
