using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardRotation : MonoBehaviour
{
    public float rotationSpeed = 5f, cameraSpeed = 70f, zoomSpeed = 1f, moveAlongSpeed = 15f;
    public float bottomZoomEdge = 1, topZoomEdge = 15;
    private float timerToReturnToCenter;
    private const float timeToReturnToCenter = 100f;
    private int cameraReverse = 1;
    [SerializeField] private GameObject _cam;
    private Transform camTranformer;
    private Quaternion startRotationCamera, startRotationBoard;

    private void Awake() {
        GlobalEventManager.OnCameraDefault += ReturnToDefault;
        camTranformer = _cam.transform;
        startRotationCamera = camTranformer.rotation;
        startRotationBoard = transform.rotation;
    }

    private void Update() {
        Zoom();
        RotateBoard();
        RotateCamera();
        MoveAlongBoard();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GlobalEventManager.SendCameraDefault();
        }
    }

    private void RotateBoard()
    {
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
    }

    private void RotateCamera()
    {
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

    private void MoveAlongBoard()
    {
        int direction = 0;
        // 1 - direction towards WHITE spawn, -1 - direction towards BLACK spawn

        if (Input.GetKey(KeyCode.W))
        {
            timerToReturnToCenter = timeToReturnToCenter;
            if (camTranformer.localEulerAngles.y > 90 && camTranformer.localEulerAngles.y < 270)
                direction = -1;
            else
                direction = 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            timerToReturnToCenter = timeToReturnToCenter;
            if (camTranformer.localEulerAngles.y > 90 && camTranformer.localEulerAngles.y < 270)
                direction = 1;
            else
                direction = -1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            timerToReturnToCenter = timeToReturnToCenter;
            if (camTranformer.localEulerAngles.y > 0 && camTranformer.localEulerAngles.y < 180)
                direction = 1;
            else
                direction = -1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            timerToReturnToCenter = timeToReturnToCenter;
            if (camTranformer.localEulerAngles.y > 0 && camTranformer.localEulerAngles.y < 180)
                direction = -1;
            else
                direction = 1;
        }

        if (direction != 0)
            camTranformer.localPosition = Vector3.Lerp(new Vector3(0, camTranformer.position.y, camTranformer.position.z), new Vector3(0, camTranformer.position.y, direction * 6), 0.01f);
        else 
        {
            if (timerToReturnToCenter > 0) timerToReturnToCenter--;
            else ReturnToCenter();
        }
    }

    private void ReturnToCenter()
    {   
        camTranformer.localPosition = Vector3.Lerp(new Vector3(0, camTranformer.position.y, camTranformer.position.z), new Vector3(0, camTranformer.position.y, 0), 0.002f);
    }

    private void ReturnToDefault()
    {
        camTranformer.localPosition = new Vector3(0, 8f, 0);
        camTranformer.rotation = startRotationCamera;
        transform.rotation = startRotationBoard;
    }
}
