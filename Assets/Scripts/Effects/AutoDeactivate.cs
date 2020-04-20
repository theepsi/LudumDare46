using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDeactivate : MonoBehaviour
{
    ParticleSystem ps = null;
    public bool isGas = false;

    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ps != null && !ps.IsAlive())
        {
            gameObject.SetActive(false);
            if (isGas)
            {
                Destroy(gameObject);
            }
        }
    }
}
