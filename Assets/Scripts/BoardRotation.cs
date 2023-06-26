using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardRotation : MonoBehaviour
{
    public float rotationSpeed = 5f, cameraSpeed = 70f, zoomSpeed = 1f;
    public float bottomZoomEdge = 5, topZoomEdge = 15;
    private int cameraReverse = 1;
    [SerializeField] private GameObject _cam;
    private Transform camTranformer;

    private void Awake() {
        camTranformer = _cam.transform;
    }

    private void Update() {
        Zoom();
        if (Input.GetMouseButton(1))
        {
            float mouseRotation = Input.GetAxis("Mouse Y");
            if (camTranformer.localEulerAngles.y >= 0 && camTranformer.localEulerAngles.y < 180)
            {
                transform.Rotate(0, 0, -mouseRotation * rotationSpeed * cameraReverse);  
            }
            else
                transform.Rotate(0, 0, mouseRotation * rotationSpeed * cameraReverse);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            camTranformer.Rotate(0, 0, -1 * cameraSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.E))
        {
            camTranformer.Rotate(0, 0, 1 * cameraSpeed * Time.deltaTime);
        }
    }

    private void Zoom()
    {
        camTranformer.position += -camTranformer.forward * zoomSpeed * -1 * Input.GetAxis("Mouse ScrollWheel");
        camTranformer.position = new Vector3
        (
        camTranformer.position.x,
        Mathf.Clamp(camTranformer.position.y, bottomZoomEdge, topZoomEdge),
        camTranformer.position.z
        );

    }
}
