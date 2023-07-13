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
    private Camera _cam;
    private Transform camTranformer;
    private Quaternion startRotationCamera, startRotationBoard;

    private int rotatingAlongAxis, rotatingClockwise, movingTo;

    private void Awake() {
        _cam = Camera.main;
        GlobalEventManager.OnCameraDefault += ReturnToDefault;
        camTranformer = _cam.gameObject.transform;
        startRotationCamera = camTranformer.rotation;
        startRotationBoard = transform.rotation;
    }

    private void Update() {
        
        if (!EscapeMenu.isPaused)
            HandleInputs();
    }

    private void HandleInputs()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GlobalEventManager.SendCameraDefault();
        }

        if (Input.GetMouseButton(1))
        {
            float mouseRotation = Input.GetAxis("Mouse Y");
            if (camTranformer.localEulerAngles.y >= 0 && camTranformer.localEulerAngles.y < 180)
                RotateBoard(-mouseRotation);
            else 
                RotateBoard(mouseRotation);
        }

        if (Input.GetKey(KeyCode.Q))
            RotateCamera(-1);
        if (Input.GetKey(KeyCode.E))
            RotateCamera(1);

        if (Input.GetAxis("Mouse ScrollWheel") != 0f)
            Zoom(Input.GetAxis("Mouse ScrollWheel"));

        if (Input.GetKey(KeyCode.W))
            MoveAlongBoard(90, 270, -1, false);
        else if (Input.GetKey(KeyCode.S))
            MoveAlongBoard(90, 270, 1, false);
        else if (Input.GetKey(KeyCode.A))
            MoveAlongBoard(0, 180, 1, false);
        else if (Input.GetKey(KeyCode.D))
            MoveAlongBoard(0, 180, -1, false);
        else MoveAlongBoard(0, 0, 1, true);
    }

    public void RotateBoard(float mouseRotation)
    {
        transform.Rotate(0, 0, mouseRotation * rotationSpeed * cameraReverse);  
    }

    public void RotateCamera(int clockwise)
    {
        camTranformer.Rotate(0, 0, clockwise * cameraSpeed * Time.deltaTime);
    }

    public void MoveAlongBoard(float minDegree, float maxDegree, int firstDirection, bool returnToDefault)
    {
        if (!returnToDefault)
        {
            int direction = 0;
            timerToReturnToCenter = timeToReturnToCenter;
            // 1 - direction towards WHITE spawn, -1 - direction towards BLACK spawn
            if (camTranformer.localEulerAngles.y > minDegree && camTranformer.localEulerAngles.y < maxDegree)
                direction = firstDirection;
            else
                direction = firstDirection * -1;

            camTranformer.localPosition = Vector3.Lerp(new Vector3(0, camTranformer.position.y, camTranformer.position.z), new Vector3(0, camTranformer.position.y, direction * 6), 0.01f);
        }
        else 
        {
            if (timerToReturnToCenter > 0) timerToReturnToCenter--;
            else ReturnToCenter();
        }
    }

    public void Zoom(float scrollSpeed)
    {
        camTranformer.position += -camTranformer.forward * zoomSpeed * -1 * scrollSpeed;
        camTranformer.position = new Vector3
        (
        camTranformer.position.x,
        Mathf.Clamp(camTranformer.position.y, bottomZoomEdge, topZoomEdge),
        camTranformer.position.z
        );
    }

    private void ReturnToCenter()
    {   
        camTranformer.localPosition = Vector3.Lerp(new Vector3(0, camTranformer.position.y, camTranformer.position.z), new Vector3(0, camTranformer.position.y, 0), 0.002f);
    }

    private void ReturnToDefault()
    {
        camTranformer.localPosition = new Vector3(0, 9f, 0);
        camTranformer.rotation = startRotationCamera;
        transform.rotation = startRotationBoard;
    }
}
