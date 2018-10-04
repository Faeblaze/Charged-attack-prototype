using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRig : MonoBehaviour {
    public float sensitivity = 20F;

    void LateUpdate()
    {
        float axis = Input.GetAxis("Mouse X");

        transform.Rotate(Vector3.up * axis * sensitivity);
    }
}
