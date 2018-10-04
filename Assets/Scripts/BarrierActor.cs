using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierActor : MonoBehaviour {
    private MeshRenderer render;
    private Material renderMaterial;
    private new GameObject collider;

    public float offOpacity = 0F;
    public float onOpacity = .20F;

    public float activationTime = 1F;
    public float activatedTime = 5F;

    private float progress = -1F;
    private float activatedTimer = 0F;

    public bool isActive = false;

    private void Awake()
    {
        collider = GetComponentInChildren<Collider>().gameObject;
        render = GetComponent<MeshRenderer>();
        renderMaterial = render.material;
        SetOpacity(offOpacity);
    }

    private void Update()
    {
        if(progress == -2F)
        {
            activatedTimer -= Time.deltaTime;
            if (activatedTimer <= 0F)
            {
                progress = -1F;
                collider.SetActive(false);
                SetOpacity(offOpacity);
                isActive = false;
            }

            return;
        }
        if (progress == -1F)
            return;

        progress += Time.deltaTime;
        SetOpacity(Mathf.Lerp(offOpacity, onOpacity, progress / activationTime));
        Debug.Log(progress + " " + Mathf.Lerp(offOpacity, onOpacity, progress / activationTime));

        if (progress >= activationTime)
        {
            progress = -2F;
            activatedTimer = activatedTime;
            collider.SetActive(true);
            isActive = true;
        }
    }

    public void Activate()
    {
        progress = 0F;
    }

    private void SetOpacity(float opacity)
    {
        render.enabled = opacity != 0F;
        Color c = renderMaterial.color;
        c.a = opacity;
        renderMaterial.color = c;
    }
}
