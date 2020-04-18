using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollSpace : MonoBehaviour
{
    Material meshRendererMaterial;

    public float parallax = 2f;

    private void Start()
    {
        meshRendererMaterial = GetComponent<MeshRenderer>().material;
    }

    void Update()
    {
        Vector2 offset = meshRendererMaterial.mainTextureOffset;

        offset.x = transform.position.x / transform.localScale.x / parallax;
        offset.y = transform.position.y / transform.localScale.y / parallax;

        meshRendererMaterial.mainTextureOffset = offset;
    }
}
