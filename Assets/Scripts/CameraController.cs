using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public GameObject primaryCamera;
    public GameObject secondaryCamera;

    public Vector2 sensitivity = new Vector2(20F, 10F);

    public Vector2 clamp;

    private void Awake()
    {
        primaryCamera.SetActive(true);
        secondaryCamera.SetActive(false);
    }

    private void LateUpdate()
    {
        GameObject cam = Input.GetMouseButton(1) ? secondaryCamera : primaryCamera;

        if (!cam)
            return;

        GameObject other = primaryCamera == cam ? secondaryCamera : primaryCamera;

        if (other.activeInHierarchy)
        {
            cam.transform.SetPositionAndRotation(other.transform.position, other.transform.rotation);
            cam.SetActive(true);
            other.SetActive(false);
        }

        float horizontal = Input.GetAxis("Mouse X");
        float vertical = Input.GetAxis("Mouse Y");

        cam.transform.Rotate((Vector3.up * horizontal) * sensitivity.x);
        cam.transform.Translate((Vector3.up * vertical) * sensitivity.y * Time.deltaTime);

        Vector3 pos = cam.transform.localPosition;
        pos.y = Mathf.Clamp(pos.y, clamp.x, clamp.y);
        cam.transform.localPosition = pos;
    }
}
