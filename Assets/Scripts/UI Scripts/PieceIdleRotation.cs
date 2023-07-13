using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceIdleRotation : MonoBehaviour
{
    private Vector3 rotationDirection;
    private Transform pieceTransform;
    public float rotationSpeed;

    private void Awake() {
        rotationSpeed = 10f;
        rotationDirection = new Vector3(Random.Range(-5, 5), Random.Range(-5, 5), Random.Range(-5, 5));
        pieceTransform = transform;
    }

    private void Update() {
        pieceTransform.Rotate(rotationDirection * Time.deltaTime * rotationSpeed);
    }
}
