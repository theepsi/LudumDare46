using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseParallax : MonoBehaviour
{
    public float parallaxSpeed = 0.5f;
    private void Update()
    {
        transform.position = Input.mousePosition * parallaxSpeed;
    }
}
