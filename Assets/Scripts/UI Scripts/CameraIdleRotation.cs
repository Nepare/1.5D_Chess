using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraIdleRotation : MonoBehaviour
{
    private Transform camTransform;
    public float rotationSpeed = 2f;

    private void Awake() {
        camTransform = transform;
    }

    void Update()
    {
        float finalRotationSpeed = rotationSpeed * Time.deltaTime;
        transform.Rotate(finalRotationSpeed, finalRotationSpeed, finalRotationSpeed);
    }
}
